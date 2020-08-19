namespace PhysicsSim
{
    public interface IMode
    {
        string Name { get; }
        void Update();
    }
    
    public class ModeIdle : IMode
    {
        public string Name { get; } = "idle";

        public void Update()
        {
            if (KeyboardControls.KeyInfo(1) == "just_pressed")
            {
                Game1.newPlanet = new Planet
                {
                    drawLevel = 1
                };
                Game1.currentMode = new ModeCreatePlanet();
            }
        }
    }

    public class ModeCreatePlanet : IMode
    {
        public string Name { get; } = "create planet";

        public void Update()
        {
            if (KeyboardControls.KeyInfo(1) == "just_pressed")
            {
                Game1.newPlanet.drawLevel = 2;
                Game1.currentMode = new ModeIdle();
            }
        }
    }
}