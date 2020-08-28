using System;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    // facilitated keyboard controls
    public class KeyboardControls
    {
        // default controls
        public static Keys[] controls = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4 };
        public static string[] controlNames = { "Pause", "New", "Grid", "Clear", "Debug" };

        static bool[] currentState = new bool[controls.Length];
        static bool[] previousState = new bool[controls.Length];
        
        // switches that can be toggled at any time
        public static bool pausedMode = false;
        public static bool debugView = false;
        static int gridLineVisibility = 0;

        public static void UpdateState()
        {
            Array.Copy(currentState, previousState, currentState.Length);
            for (int i = 0; i < controls.Length; i++)
            {
                currentState[i] = Keyboard.GetState().IsKeyDown(controls[i]);
            }
        }

        public static void UpdateSwitches()
        {
            // toggle pausing
            if (KeyInfo(0) == "just_pressed")
            {
                pausedMode = !pausedMode;
            }
            
            // toggle debug view
            if (KeyInfo(4) == "just_pressed")
            {
                debugView = !debugView;
            }
            
            // cycle through the grid levels
            if (KeyInfo(2) == "just_pressed")
            {
                gridLineVisibility += 1;
                switch (gridLineVisibility) 
                {
                    case 0:
                        Simulator.currentMouseMode = new FreeMovement();
                        break;
                    case 1:
                        Simulator.currentMouseMode = new SmallGrid();
                        break;
                    case 2:
                        Simulator.currentMouseMode = new LargeGrid();
                        break;
                    case 3:
                        Simulator.currentMouseMode = new FreeMovement();
                        gridLineVisibility = 0;
                        break;
                }
            }
        }

        public static string KeyInfo(int key)
        {
            string result = (previousState[key], currentState[key]) switch
            {
                (false, false) => "held_released",
                (false, true) => "just_pressed",
                (true, false) => "just_released",
                (true, true) => "held_pressed",
            };

            return result;
        }		

    }
}