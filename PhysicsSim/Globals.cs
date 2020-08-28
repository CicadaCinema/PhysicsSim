using System;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace PhysicsSim
{
    class Globals
    {
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
                            case "Binding":
                                int bindingIndex = Array.IndexOf(KeyboardControls.controlNames, reader["name"]);
                                
                                // if key name valid
                                if (bindingIndex != -1)
                                {
                                    Keys keyCode = (Keys) Enum.Parse(typeof(Keys), reader["key"]);
                                    KeyboardControls.controls[bindingIndex] = keyCode;
                                }
                                
                                break;
                            case "PMglobal":
                                int PMglobalIndex = Array.IndexOf(Game1.planetMassConstantsNames, reader["name"]);
                                
                                // if key name valid
                                if (PMglobalIndex != -1)
                                {
                                    Game1.planetMassConstants[PMglobalIndex] = Convert.ToInt32(reader["value"]);
                                }

                                break;
                        }
                    }
                }
            }
        }
    }
}