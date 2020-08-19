using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PhysicsSim
{
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
