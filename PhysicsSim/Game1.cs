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
		// TODO: make this a dictionary so that the controls have names
		static Keys[] controls = { Keys.A, Keys.S, Keys.D, Keys.F };

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
		public int xCoord;
		public int yCoord;
		public int radius;
	}
	
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Planet> planets = new List<Planet>();

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
			// TODO: Add your initialization logic here

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

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
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

			KeyboardControls.UpdateState();
			base.Update(gameTime);

			if (KeyboardControls.KeyInfo(0) == "just_pressed")
			{
				Planet newPlanet = new Planet
				{
					xCoord = Mouse.GetState().X,
					yCoord = Mouse.GetState().Y,
					radius = 100 
				};
				planets.Add(newPlanet);
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Drawing code
			spriteBatch.Begin();

			if (KeyboardControls.KeyInfo(1) == "just_pressed")
			{
				spriteBatch.DrawCircle(Mouse.GetState().X, Mouse.GetState().Y, 100, 100, Color.Aquamarine, 100);
			}

			if (KeyboardControls.KeyInfo(2) == "held_pressed")
			{
				foreach (Planet planet in planets)
				{
					spriteBatch.DrawCircle(planet.xCoord, planet.yCoord, 100, 100, Color.Red, 100);
				}
			}
			
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
