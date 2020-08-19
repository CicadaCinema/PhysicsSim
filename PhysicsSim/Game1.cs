using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace PhysicsSim
{
	// facilitated keyboard controls
	public class KeyboardControls
	{
		// TODO: add alternative configuration for keyboards with no numberpad
		static Keys[] controls = { Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3 };

		static bool[] currentState = new bool[controls.Length];
		static bool[] previousState = new bool[controls.Length];

		public static void UpdateState()
		{
			Array.Copy(currentState, previousState, currentState.Length);
			for (int i = 0; i < controls.Length; i++)
			{
				currentState[i] = Keyboard.GetState().IsKeyDown(controls[i]);
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
	
	// class responsible for spherical objects
	public class Planet
	{
		public int drawLevel = 0;
		Vector2 position = new Vector2();
		public int radius;
		Vector2 velocity = new Vector2(2, 2);

		public void CreateUpdate()
		{
			if (drawLevel == 1)
			{
				position = new Vector2(Game1.currentMouseState.X, Game1.currentMouseState.Y);
				radius = Convert.ToInt32((Math.Tanh(Game1.currentMouseState.ScrollWheelValue / 1000.0)+1) * 100);
			} else if (drawLevel == 2)
			{
				Game1.planets.Add(Game1.newPlanet);
				Game1.newPlanet = new Planet();
			}
		}

		public void Update()
		{
			// if the planet is on the final drawLevel and the game is unpaused, update its velocity
			if (drawLevel == 2 && !Game1.pausedMode)
			{
				position = Vector2.Add(position, velocity);
			}
			
			// unless the planet hasn't entered creation mode, draw it at its position
			if (drawLevel > 0)
			{
				Game1.spriteBatch.DrawCircle(position.X, position.Y, radius, 100, Color.Red, radius);
			}
		}
	}

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

	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		public static SpriteBatch spriteBatch;
		SpriteFont textFont;
		
		public static MouseState currentMouseState;
		
		public static List<Planet> planets = new List<Planet>();
		public static Planet newPlanet = new Planet();
		public static IMode currentMode = new ModeIdle();

		public static bool pausedMode = false;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Add your initialization logic here
			this.IsMouseVisible = true;
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// Load Arial font for use throughout the UI
			textFont = Content.Load<SpriteFont>("Arial Bold");

			// use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// Game update logic
			base.Update(gameTime);
			KeyboardControls.UpdateState();
			currentMouseState = Mouse.GetState();

			if (KeyboardControls.KeyInfo(0) == "just_pressed")
			{
				pausedMode = !pausedMode;
			}
			
			currentMode.Update();
			newPlanet.CreateUpdate();
		}

		// Drawing code
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();

			foreach (Planet planet in planets)
			{
				planet.Update();
			}
			newPlanet.Update();
			
			spriteBatch.DrawString(textFont, "Mode: " + currentMode.Name, new Vector2(10, 10), Color.White);
			if (pausedMode)
			{
				spriteBatch.DrawString(textFont, "PAUSED", new Vector2(10, 28), Color.White);
			}
			
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
