using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace PhysicsSim
{
    // set the requires methods for this interface
    public interface IGridHandler
    {
        // update the mouse state vector in the Simulator class
        void UpdateMousePosition();
        // draw the required grid
        void DrawGrid();
    }

    public static class GridLineHelper
    {
        // general function for rounding the mouse position given the spacing between the gridlines
        public static Vector2 RoundMousePosition(float spacing)
        {
            double x = Math.Round(Simulator.currentMouseState.X/spacing) * spacing;
            double y = Math.Round(Simulator.currentMouseState.Y/spacing) * spacing;
            return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
        }
        
        // general function for drawing a grid given the screen size and the spacing
        public static void Draw(int spacing, int windowWidth, int windowHeight, Color colour)
        {
            // draw horizontal gridlines
            for (int y = 0; y <= windowHeight; y += spacing)
            {
                Simulator.spriteBatch.DrawLine(0, y, windowWidth, y, colour);
            }
				
            // draw vertical gridlines
            for (int x = 0; x <= windowWidth; x += spacing)
            {
                Simulator.spriteBatch.DrawLine(x, 0, x, windowHeight, colour);
            }
        }
    }

    public class FreeMovement : IGridHandler
    {
        public void UpdateMousePosition()
        {
            // don't round the mouse positon
            Simulator.currentMouseVector = new Vector2(Simulator.currentMouseState.X, Simulator.currentMouseState.Y);
        }

        public void DrawGrid()
        {
            // no grid needs to be drawn
        }
    }
    
    public class SmallGrid : IGridHandler
    {
        public void UpdateMousePosition()
        {
            // round the mouse position
            Simulator.currentMouseVector = GridLineHelper.RoundMousePosition(20);
        }
        public void DrawGrid()
        {
            // draw a small grid
            GridLineHelper.Draw(20, Simulator.currentWindowWidth, Simulator.currentWindowHeight, Simulator.colourPalette["Grid"]);
        }
    }
    
    public class LargeGrid : IGridHandler
    {
        public void UpdateMousePosition()
        {
            // round the mouse position
            Simulator.currentMouseVector = GridLineHelper.RoundMousePosition(50);
        }
        public void DrawGrid()
        {
            // draw a large grid
            GridLineHelper.Draw(50, Simulator.currentWindowWidth, Simulator.currentWindowHeight, Simulator.colourPalette["Grid"]);
        }
    }
}