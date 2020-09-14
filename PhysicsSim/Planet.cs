using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using MonoGame.Extended;

namespace PhysicsSim
{
    public class Planet
    {
        // drawLevel is 0 for a placeholder planet
        public int drawLevel = 0;
        
        public Color colour = Simulator.colourPalette["New Planet"]; // a planet will stay a default colour during creation
        public int radius;
        public int mass;
        
        public Vector2 position;
        public Vector2 velocity;
        Vector2 accelerationThisFrame; // acceleration is calculated anew for each frame
        
        // stores the co-ordinates of points making up this planet's trail
        List<short[]> trail = new List<short[]>();
        
        // check if a number is within (0), above (1) or below (-1) a range
        int TestRange(int testValue, int[] range)
        {
            if (testValue < range[0])
            {
                return -1;
            } else if (testValue > range[1])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        // generic function for edge bouncing in 1 dimension
        (int, int) BounceEdge(int position, int velocity, int[] range)
        {
            // add position to velocity
            position += velocity;
            
            // test to see if position is within range
            int rangeTestResult = TestRange(position, range);
            
            // if position is outside range, take action
            while (rangeTestResult != 0)
            {
                // the velocity direction must be changed
                velocity *= -1;
                
                switch (rangeTestResult)
                {
                    // position is below 0
                    case -1:
                        position = (position-range[0]) * -1 + range[0];
                        break;
                    // position is above the current screen resolution
                    case 1:
                        position = (position-range[1]) * -1 + range[1];
                        break;
                }
                
                // test to see if position is within range
                rangeTestResult = TestRange(position, range);
            }
            
            // return the position and velocity
            return (position, velocity);
        }

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
                    // set the planet's colour to some bright colour
                    colour = new Color(Simulator.rng.Next(100,256), Simulator.rng.Next(100,256), Simulator.rng.Next(100,256));
                    
                    // export the planet to the main planet list and reset the placeholder planet object and the current mode
                    Simulator.planets.Add(Simulator.newPlanet);
                    Simulator.newPlanet = new Planet();
                    Simulator.currentMode = new ModeIdle();
                    break;
            }
        }

        public void Update()
        {
            // if the planet is on the final drawLevel and the game is unpaused, update its position and velocity
            if (drawLevel == 3 && !Switches.pausedMode)
            {
                // begin collecting the total acceleration for this frame in a new variable
                accelerationThisFrame = new Vector2();
                
                // GRAVITY!
                // loop through all the created planets and get their affect on the acceleration of this planet
                foreach (Planet planet in Simulator.planets)
                {
                    // only perform the gravity calculation if the two planets are not touching, distinct and gravity is enabled
                    if (Switches.gravityEnabled && !Equals(planet, this) && ((planet.position - position).Length() > (planet.radius + radius)))
                    {
                        // acceleration due to gravity = (constant*m)/(r^2) - this does not depend on the mass of the body being accelerated
                        float accelerationMagnitude = planet.mass / Vector2.Subtract(planet.position, position).LengthSquared();
                        
                        // apply gravitational constant
                        accelerationMagnitude *= Switches.gravitationalConstant;
                        
                        // the direction of the acceleration is always towards the other planet
                        Vector2 accelerationDirection = planet.position - position;
                        
                        // calculate the acceleration given the direction and magnitude and add it to the total acceleration vector
                        accelerationThisFrame += accelerationDirection * (accelerationMagnitude / accelerationDirection.Length());
                    }
                }
                
                // apply acceleration, scaled velocity value to the relevant field
                velocity += accelerationThisFrame;
                position += velocity/50;
                
                // TODO: handle the special case where the planet is spawned outside the virtual screen bounds

                // virtual screen bounds (eg virtual screen goes from radius to virtualScreenW/H), accounting for the radius of the planet
                int virtualScreenWidth = Simulator.currentWindowWidth - radius;
                int virtualScreenHeight = Simulator.currentWindowHeight - radius;
                
                // update position and velocity, bouncing off edges
                (position.X, velocity.X) = BounceEdge(Convert.ToInt32(position.X), Convert.ToInt32(velocity.X), new []{radius, virtualScreenWidth});
                (position.Y, velocity.Y) = BounceEdge(Convert.ToInt32(position.Y), Convert.ToInt32(velocity.Y), new []{radius, virtualScreenHeight});

                // add the current position of the planet to its trail
                trail.Add(new short[2] {Convert.ToInt16(position.X), Convert.ToInt16(position.Y)});
                
                // remove the oldest element of the trail if it is too long
                if (trail.Count > Switches.trailDuration)
                {
                    trail.RemoveAt(0);
                }
            }
			
            // unless the planet hasn't entered creation mode, draw it at its position
            if (drawLevel > 0)
            {
                Simulator.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, colour, radius);
                
                // indicate velocity in the correct style
                if (drawLevel == 2)
                {
                    // if user is still configuring velocity
                    Simulator.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Simulator.colourPalette["UI"], 2);
                } else if (Switches.debugView)
                {
                    // if user is debugging
                    Simulator.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Simulator.colourPalette["Debug UI"], 3);
                }
                
                // if the planet is fully drawn
                if (drawLevel == 3 && Switches.trailVisibility)
                {
                    // draw its entire trail
                    foreach (short[] trailElement in trail)
                    {
                        Simulator.spriteBatch.DrawPoint(trailElement[0], trailElement[1], Simulator.colourPalette["Trails"]);
                    }
                }
            }
        }
    }
}