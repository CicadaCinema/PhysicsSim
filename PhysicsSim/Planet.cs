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
        Vector2 velocity = new Vector2(2, 2);

        public void CreateUpdate()
        {
            if (drawLevel == 1)
            {
                position = new Vector2(Game1.currentMouseState.X, Game1.currentMouseState.Y);
                radius = Convert.ToInt32((Math.Tanh(Game1.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
            } else if (drawLevel == 2)
            {
                Game1.planets.Add(Game1.newPlanet);
                Game1.newPlanet = new Planet();
            }
        }

        public void Update()
        {
            // if the planet is on the final drawLevel and the game is unpaused, update its velocity
            if (drawLevel == 2 && !Game1.pausedMode)
            {
                position = Vector2.Add(position, velocity);
            }
			
            // unless the planet hasn't entered creation mode, draw it at its position
            if (drawLevel > 0)
            {
                Game1.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, Color.Red, radius);
            }
        }
    }
}