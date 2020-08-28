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
                position = Game1.currentMouseVector;
                radius = Convert.ToInt32((Math.Tanh(Game1.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
            } else if (drawLevel == 2)
            {
                velocity = Game1.currentMouseVector - position;
            } else if (drawLevel == 3)
            {
                mass = Convert.ToInt32(Math.Pow(radius, Game1.planetMassConstants[0]))*Game1.planetMassConstants[1] + Game1.planetMassConstants[2];
                Game1.planets.Add(Game1.newPlanet);
                Game1.newPlanet = new Planet();
                Game1.currentMode = new ModeIdle();
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
                foreach (Planet planet in Game1.planets)
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
                Game1.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, Color.Red, radius);
                Game1.spriteBatch.DrawLine(position, Vector2.Add(position, velocity), Color.White, 3);
            }
        }
    }
}