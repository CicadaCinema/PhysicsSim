using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace PhysicsSim
{
    // get correct mouse input for the desired snapping level
    public interface IGridHandler
    {
        void UpdateMousePosition();
        void DrawGrid();
    }

    public static class GridLineHelper
    {
        public static Vector2 RoundMousePosition(float spacing)
        {
            double x = Math.Round(Simulator.currentMouseState.X/spacing) * spacing;
            double y = Math.Round(Simulator.currentMouseState.Y/spacing) * spacing;
            return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
        }
        public static void Draw(int spacing)
        {
            // draw horizontal gridlines
            for (int y = 0; y <= Simulator.currentWindowHeight; y += spacing)
            {
                Simulator.spriteBatch.DrawLine(0, y, Simulator.currentWindowWidth, y, Color.Green);
            }
				
            // draw horizontal gridlines
            for (int x = 0; x <= Simulator.currentWindowWidth; x += spacing)
            {
                Simulator.spriteBatch.DrawLine(x, 0, x, Simulator.currentWindowHeight, Color.Green);
            }
        }
    }

    public class FreeMovement : IGridHandler
    {
        public void UpdateMousePosition()
        {
            Simulator.currentMouseVector = new Vector2(Simulator.currentMouseState.X, Simulator.currentMouseState.Y);
        }

        public void DrawGrid()
        {
            // don't need to draw anything
        }
    }
    
    public class SmallGrid : IGridHandler
    {
        public void UpdateMousePosition()
        {
            // bring these three lines into the gridlines class

            Simulator.currentMouseVector = GridLineHelper.RoundMousePosition(20);


        }
        public void DrawGrid()
        {
            GridLineHelper.Draw(20);
        }
    }
    
    public class LargeGrid : IGridHandler
    {
        public void UpdateMousePosition()
        {
            Simulator.currentMouseVector = GridLineHelper.RoundMousePosition(50);
        }
        public void DrawGrid()
        {
            GridLineHelper.Draw(50);
        }
    }
}