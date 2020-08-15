using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace PhysicsSim
{
	public class Planet
	{
		public int xCoord;
		public int yCoord;
	}

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

		public static bool OneShot(int key)
		{
			if (!previousState[key] && currentState[key])
			{
				return true;
			}
			else
			{
				return false;
			}
		}		

	}

	/// <summary>
	/// This is the main type for your game.
	/// </summary>
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

			if (KeyboardControls.OneShot(0))
			{
				Planet newPlanet = new Planet
				{
					xCoord = Mouse.GetState().X,
					yCoord = Mouse.GetState().Y
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

			if (KeyboardControls.OneShot(1))
			{
				spriteBatch.Begin();
				spriteBatch.DrawCircle(Mouse.GetState().X, Mouse.GetState().Y, 100, 100, Color.Aquamarine, 100);
				spriteBatch.End();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				spriteBatch.Begin();
				foreach (Planet planet in planets)
				{
					spriteBatch.DrawCircle(planet.xCoord, planet.yCoord, 100, 100, Color.Red, 100);
				}
				spriteBatch.End();
			}

			base.Draw(gameTime);
		}
	}
}
