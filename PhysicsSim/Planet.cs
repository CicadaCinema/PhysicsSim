using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace PhysicsSim
{
    public class Planet
    {
        public int drawLevel = 0;
        Vector2 position;
        public int radius;
        public int mass;
        Vector2 velocity;
        Vector2 acceleration;

        public void CreateUpdate()
        {
            if (drawLevel == 1)
            {
                position = Simulator.currentMouseVector;
                radius = Convert.ToInt32((Math.Tanh(Simulator.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
            } else if (drawLevel == 2)
            {
                velocity = Simulator.currentMouseVector - position;
            } else if (drawLevel == 3)
            {
                mass = Convert.ToInt32(Math.Pow(radius, Globals.massGlobals[0]))*Globals.massGlobals[1] + Globals.massGlobals[2];
                Simulator.planets.Add(Simulator.newPlanet);
                Simulator.newPlanet = new Planet();
                Simulator.currentMode = new ModeIdle();
            }
        }

        public void Update()
        {
            // if the planet is on the final drawLevel and the game is unpaused, update its velocity
            if (drawLevel == 3 && !KeyboardControls.pausedMode)
            {
                // gravity!
                // acceleration due to gravity = (constant*m)/(r^2) - this does not depend on the mass of the body being accelerated
                acceleration = new Vector2();
                foreach (Planet planet in Simulator.planets)
                {
                    if (!Equals(planet, this) && ((planet.position - position).Length() > (planet.radius + radius)))
                    {
                        float accelerationMagnitude = planet.mass / Vector2.Subtract(planet.position, position).LengthSquared();
                        Vector2 accelerationDirection = planet.position - position;
                        acceleration += accelerationDirection * (accelerationMagnitude / accelerationDirection.Length());
                    }
                }
                
                // scale both acceleration and velocity
                velocity += acceleration * 1000;
                position += velocity/50;
            }
			
            // unless the planet hasn't entered creation mode, draw it at its position
            if (drawLevel > 0)
            {
                Simulator.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, Color.Red, radius);
                Simulator.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Color.White, 3);
            }
        }
    }
}