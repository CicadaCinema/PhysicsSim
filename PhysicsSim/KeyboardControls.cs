using System;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    // facilitated keyboard controls
    public class KeyboardControls
    {
        // TODO: add alternative configuration for keyboards with no numberpad
        static Keys[] controls = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3 };

        static bool[] currentState = new bool[controls.Length];
        static bool[] previousState = new bool[controls.Length];
        
        // global switches
        public static bool pausedMode = false;
        static int gridLineVisibility = 0;

        public static void UpdateState()
        {
            Array.Copy(currentState, previousState, currentState.Length);
            for (int i = 0; i < controls.Length; i++)
            {
                currentState[i] = Keyboard.GetState().IsKeyDown(controls[i]);
            }
        }

        public static void UpdateGlobalSwitches()
        {
            if (KeyInfo(0) == "just_pressed")
            {
                pausedMode = !pausedMode;
            }
            
            // cycle through the grid levels
            if (KeyInfo(2) == "just_pressed")
            {
                gridLineVisibility += 1;
                switch (gridLineVisibility) 
                {
                    case 0:
                        Game1.currentMouseMode = new FreeMovement();
                        break;
                    case 1:
                        Game1.currentMouseMode = new SmallGrid();
                        break;
                    case 2:
                        Game1.currentMouseMode = new LargeGrid();
                        break;
                    case 3:
                        Game1.currentMouseMode = new FreeMovement();
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