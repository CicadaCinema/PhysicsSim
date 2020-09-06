using System;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    // facilitated keyboard controls
    public class KeyboardControls
    {
        // default controls
        public static Keys[] controls = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5 };
        public static string[] controlNames = { "Pause", "New", "Grid", "Clear", "Debug", "Trail" };

        // state of the chosen controls this frame and the previous frame
        static bool[] currentState = new bool[controls.Length];
        static bool[] previousState = new bool[controls.Length];
        
        // update the keyboard states
        public static void UpdateState()
        {
            // make a copy (instead of a reference) to the previous state of the controls
            Array.Copy(currentState, previousState, currentState.Length);
            // record the new state of the controls
            for (int i = 0; i < controls.Length; i++)
            {
                currentState[i] = Keyboard.GetState().IsKeyDown(controls[i]);
            }
        }

        // check the state of one key based on its history for the past frame
        // eg to see if it has just been pressed this frame
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