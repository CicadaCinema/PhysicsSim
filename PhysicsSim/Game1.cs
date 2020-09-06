using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Resources;
using System.Xml;
using MonoGame.Extended;

namespace PhysicsSim
{
	public class Simulator : Game
	{
		// graphics and fonts
		GraphicsDeviceManager graphics;
		public static SpriteBatch spriteBatch;
		SpriteFont textFont;
		
		// a colour palette for the simulator
		public static readonly Dictionary<string, Color> colourPalette = new Dictionary<string, Color>(){
			{"Background", Color.DarkSlateGray},
			{"Grid", Color.Gray},
			{"UI", Color.LightBlue},
			{"Debug UI", Color.LightBlue},
			{"New Planet", Color.DarkRed},
			{"Trails", Color.LightGreen}
		};
		
		// random number generator used to generate a random colour for each planet
		public static Random rng = new Random();

		// window dimensions (these may change during operation)
		public static int currentWindowWidth;
		public static int currentWindowHeight;
		
		// mouse state and position (inc. scrollwheel data)
		public static MouseState currentMouseState;
		public static Vector2 currentMouseVector;
		
		// list of already-created planets and a placeholder planet object for creation
		public static List<Planet> planets = new List<Planet>();
		public static Planet newPlanet = new Planet();
		
		// objects which can be changed programatically
		public static IMode currentMode = new ModeIdle();
		public static IGridHandler currentMouseMode = new FreeMovement();
		
		public Simulator()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		// initialise any non-graphics related content and settings before the simulator starts
		protected override void Initialize()
		{
			Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
			// keep running at full speed even when tabbed out
			InactiveSleepTime = new TimeSpan(0);
			base.Initialize();
			
			// read global config from XML file
			Switches.ReadConfig();
		}

		protected override void LoadContent()
		{
			// create a new SpriteBatch, which can be used to draw textures
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// load Arial Bold
			textFont = Content.Load<SpriteFont>("Arial Bold");
		}

		protected override void UnloadContent()
		{
			
		}

		// logic is run 60 times per second
		protected override void Update(GameTime gameTime)
		{
			// quit the game if the player requests exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
			
			// set new window dimensions
			currentWindowWidth = Window.ClientBounds.Width;
			currentWindowHeight = Window.ClientBounds.Height;

			// update keyboard state and switches
			KeyboardControls.UpdateState();
			Switches.UpdateSwitches();
			
			// update mouse state
			currentMouseState = Mouse.GetState();
			currentMouseMode.UpdateMousePosition();
			
			// perform any actions in the current mode
			currentMode.Update();
			// update the state of the planet in creation
			newPlanet.CreateUpdate();
		}

		// graphics are drawn 60 times per second
		protected override void Draw(GameTime gameTime)
		{
			// set background and begin drawing graphics
			GraphicsDevice.Clear(colourPalette["Background"]);
			spriteBatch.Begin();
			
			// draw the grid (if applicable)
			currentMouseMode.DrawGrid();

			// perform the update method on all the planets in the simulation, as well as the planet being created
			foreach (Planet planet in planets)
			{
				planet.Update();
			}
			newPlanet.Update();
			
			// display the current mode and paused UI
			// 18 is a comfortable value for line spacing
			spriteBatch.DrawString(textFont, "Mode: " + currentMode.Name, new Vector2(10, 10), colourPalette["UI"]);
			if (Switches.pausedMode)
			{
				spriteBatch.DrawString(textFont, "PAUSED", new Vector2(10, 10 + 1*18), colourPalette["UI"]);
			}

			// display some debug information if the debug switch is set
			// 18 is a comfortable value for line spacing
			if (Switches.debugView)
			{
				spriteBatch.DrawString(textFont, "Game running slowly: " + gameTime.IsRunningSlowly.ToString(), new Vector2(10, currentWindowHeight - 24), colourPalette["Debug UI"]);
				spriteBatch.DrawString(textFont, "Number of planets: " + planets.Count.ToString(), new Vector2(10, currentWindowHeight - 24 -1*18), colourPalette["Debug UI"]);
				
				// display the new planet's mass and radius if it is being created
				if (newPlanet.drawLevel > 0)
				{
					// store this debug info in an array
					string[] newPlanetDebugInfo =
					{
						"Mass: " + newPlanet.mass,
						"Radius: " + newPlanet.radius,
						"Position (X component): " + newPlanet.position.X,
						"Position (Y component): " + newPlanet.position.Y,
						"Velocity (X component): " + newPlanet.velocity.X,
						"Velocity (Y component): " + newPlanet.velocity.Y
					};
					int debugInfoHeight = currentWindowHeight - 24 - 3*18;
					
					// display each element of the array at an appropriate height
					foreach (string newPlanetDebugLine in newPlanetDebugInfo)
					{
						spriteBatch.DrawString(textFont, newPlanetDebugLine, new Vector2(10, debugInfoHeight), colourPalette["Debug UI"]);
						debugInfoHeight -= 18;
					}
				}
			}
			
			// display the graphics drawn this frame
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
