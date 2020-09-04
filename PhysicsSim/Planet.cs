﻿using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace PhysicsSim
{
    public class Planet
    {
        // drawLevel is 0 for a placeholder planet
        public int drawLevel = 0;
        
        public int radius;
        public int mass;
        
        Vector2 position;
        Vector2 velocity;
        Vector2 accelerationThisFrame; // acceleration is calculated anew for each frame

        public void CreateUpdate()
        {
            switch (drawLevel)
            {
                case 1:
                    // update the new planet's radius according to the position of the mouse and the scrollwheel
                    position = Simulator.currentMouseVector;
                    radius = Convert.ToInt32((Math.Tanh(Simulator.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
                    
                    // perform the mass calculation on-the-fly based on the globals
                    mass = Convert.ToInt32(Math.Pow(radius, Switches.massGlobals[0]))*Switches.massGlobals[1] + Switches.massGlobals[2];
                    break;
                case 2:
                    // update the new planet's velocity according to the position of the mouse
                    velocity = Simulator.currentMouseVector - position;
                    break;
                case 3:
                    // export the planet to the main planet list and reset the placeholder planet object and the current mode
                    Simulator.planets.Add(Simulator.newPlanet);
                    Simulator.newPlanet = new Planet();
                    Simulator.currentMode = new ModeIdle();
                    break;
            }
        }

        public void Update()
        {
            // if the planet is on the final drawLevel and the game is unpaused, update its velocity
            if (drawLevel == 3 && !Switches.pausedMode)
            {
                // GRAVITY!

                // begin collecting the total acceleration for this frame in a new variable
                accelerationThisFrame = new Vector2();
                
                // loop through all the created planets and get their affect on the acceleration of this planet
                foreach (Planet planet in Simulator.planets)
                {
                    // only perform the gravity calculation if the two planets are not touching and distinct
                    if (!Equals(planet, this) && ((planet.position - position).Length() > (planet.radius + radius)))
                    {
                        // acceleration due to gravity = (constant*m)/(r^2) - this does not depend on the mass of the body being accelerated
                        // TODO: add the gravitational constant to config.xml
                        float accelerationMagnitude = planet.mass / Vector2.Subtract(planet.position, position).LengthSquared();
                        accelerationMagnitude *= 1000;
                        
                        // the direction of the acceleration is always towards the other planet
                        Vector2 accelerationDirection = planet.position - position;
                        
                        // calculate the acceleration given the direction and magnitude and add it to the total acceleration vector
                        accelerationThisFrame += accelerationDirection * (accelerationMagnitude / accelerationDirection.Length());
                    }
                }
                
                // scale both acceleration and velocity before applying them to the relevant fields
                velocity += accelerationThisFrame;
                position += velocity/50;
            }
			
            // unless the planet hasn't entered creation mode, draw it at its position
            if (drawLevel > 0)
            {
                Simulator.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, Color.Red, radius);
                if (Switches.debugView)
                {
                    // indicate velocity if user is debugging
                    Simulator.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Color.White, 3);
                } else if (drawLevel == 2)
                {
                    // if user is still configuring velocity, draw an indicator line of another colour
                    Simulator.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Color.Yellow, 2);
                }
            }
        }
    }
}