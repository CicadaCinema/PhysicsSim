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

        // read the xml config file and update the default global values if needed
        public static void ReadConfig()
        {
            // read bindings and config from xml file
            using (XmlReader reader = XmlReader.Create("config.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            // found a key binding
                            case "Binding":
                                // find 
                                int bindingIndex = Array.IndexOf(KeyboardControls.controlNames, reader["name"]);
                                
                                // if key name valid
                                if (bindingIndex != -1)
                                {
                                    Keys keyCode = (Keys) Enum.Parse(typeof(Keys), reader["key"]);
                                    KeyboardControls.controls[bindingIndex] = keyCode;
                                }
                                
                                break;
                            // found a global to change planet mass calculation
                            case "PMglobal":
                                int PMglobalIndex = Array.IndexOf(massGlobalsNames, reader["name"]);
                                
                                // if key name valid
                                if (PMglobalIndex != -1)
                                {
                                    massGlobals[PMglobalIndex] = Convert.ToInt32(reader["value"]);
                                }

                                break;
                        }
                    }
                }
            }
        }
    }
}