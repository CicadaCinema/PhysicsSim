using System;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    class Globals
    {
        // the default globals for mass calculation
        public static int[] massGlobals = new int[] {1, 1, 1};
        static string[] massGlobalsNames = new string[] {"index", "coefficient", "constant"};
        
        // user-togglable switches
        public static bool pausedMode = false;
        public static bool debugView = false;
        public static int gridLineVisibility = 0;

        // read the xml config file and update the default global values if needed
        public static void ReadConfig()
        {
            // read bindings and config from xml file
            using (XmlReader reader = XmlReader.Create("config.xml"))
            {
                while (reader.Read())
                {
                    // only read xml start tags
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            // found a key binding
                            case "Binding":
                                // find index of bind
                                int bindingIndex = Array.IndexOf(KeyboardControls.controlNames, reader["name"]);
                                
                                // if bind name is valid
                                if (bindingIndex != -1)
                                {
                                    // convert binding string to a Keys object and replace the default at that position
                                    Keys keyCode = (Keys) Enum.Parse(typeof(Keys), reader["key"]);
                                    KeyboardControls.controls[bindingIndex] = keyCode;
                                }
                                
                                break;
                            // found a global to change planet mass calculation
                            case "PMglobal":
                                // find index of global
                                int PMglobalIndex = Array.IndexOf(massGlobalsNames, reader["name"]);
                                
                                // if global name is valid
                                if (PMglobalIndex != -1)
                                {
                                    // replace the default global at that position
                                    massGlobals[PMglobalIndex] = Convert.ToInt32(reader["value"]);
                                }

                                break;
                        }
                    }
                }
            }
        }
        
        // update user-toggleable switches
        public static void UpdateSwitches()
        {
            // toggle pausing
            if (KeyboardControls.KeyInfo(0) == "just_pressed")
            {
                pausedMode = !pausedMode;
            }
            
            // toggle debug view
            if (KeyboardControls.KeyInfo(4) == "just_pressed")
            {
                debugView = !debugView;
            }
            
            // cycle through the grid levels
            if (KeyboardControls.KeyInfo(2) == "just_pressed")
            {
                gridLineVisibility += 1;
                switch (gridLineVisibility) 
                {
                    // change the object in currentMouseMode according to gridLineVisibility
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
                        // reset gridLineVisibility back to 0
                        Simulator.currentMouseMode = new FreeMovement();
                        gridLineVisibility = 0;
                        break;
                }
            }
        }
    }
}