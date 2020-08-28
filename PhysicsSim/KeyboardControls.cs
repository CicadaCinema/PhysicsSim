using System;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    // facilitated keyboard controls
    public class KeyboardControls
    {
        public static void GetXmlResult()
        {
            using (XmlReader reader = XmlReader.Create("config.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Binding":
                                string keyName = reader["name"];
                                string keyCodeString = reader["key"];
                                int index = Array.IndexOf(controlNames, keyName);
                                
                                // if key name valid
                                if (index != -1)
                                {
                                    Keys keyCode = (Keys) Enum.Parse(typeof(Keys), keyCodeString);
                                    controls[index] = keyCode;
                                }
                                
                                break;
                        }
                    }
                }
            }
        }
        
        // default controls
        static Keys[] controls = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3 };
        static string[] controlNames = { "Pause", "New", "Grid", "Clear" };

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