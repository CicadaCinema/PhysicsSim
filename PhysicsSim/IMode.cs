using System.Collections.Generic;

namespace PhysicsSim
{
    // each mode must have a name and an update method
    public interface IMode
    {
        string Name { get; }
        void Update();
    }
    
    public class ModeIdle : IMode
    {
        // read-only mode name
        public string Name { get; } = "idle";

        public void Update()
        {
            // allow user to enter planet creation mode
            if (KeyboardControls.KeyInfo(1) == "just_pressed")
            {
                // update the state of the placeholder planet so it can be shown
                Simulator.newPlanet = new Planet
                {
                    drawLevel = 1
                };
                Simulator.currentMode = new ModeCreatePlanet();
            }
            
            // allow user to clear all the planets
            if (KeyboardControls.KeyInfo(3) == "just_pressed")
            {
                Simulator.planets = new List<Planet>();
            }
        }
    }

    public class ModeCreatePlanet : IMode
    {
        // read-only mode name
        public string Name { get; } = "create planet";

        public void Update()
        {
            // allow user to increase the drawLevel by 1 when needed
            if (KeyboardControls.KeyInfo(1) == "just_pressed")
            {
                Simulator.newPlanet.drawLevel += 1;
            }
            
            // allow user to cancel planet creation
            if (KeyboardControls.KeyInfo(3) == "just_pressed")
            {
                Simulator.newPlanet = new Planet();
                Simulator.currentMode = new ModeIdle();
            }
        }
    }
}