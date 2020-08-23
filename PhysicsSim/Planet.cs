using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace PhysicsSim
{
    public class Planet
    {
        public int drawLevel = 0;
        Vector2 position = new Vector2();
        public int radius;
        Vector2 velocity;

        public void CreateUpdate()
        {
            if (drawLevel == 1)
            {
                position = Game1.currentMouseVector;
                radius = Convert.ToInt32((Math.Tanh(Game1.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
            } else if (drawLevel == 2)
            {
                velocity = Vector2.Subtract(Game1.currentMouseVector, position);
            } else if (drawLevel == 3)
            {
                Game1.planets.Add(Game1.newPlanet);
                Game1.newPlanet = new Planet();
                Game1.currentMode = new ModeIdle();
            }
        }

        public void Update()
        {
            // if the planet is on the final drawLevel and the game is unpaused, update its velocity
            if (drawLevel == 3 && !Game1.pausedMode)
            {
                position = Vector2.Add(position, velocity);
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