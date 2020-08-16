﻿using Microsoft.Xna.Framework;
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
		public int xCoord;
		public int yCoord;
		public int radius;

		public void Draw()
		{
			Game1.spriteBatch.DrawCircle(xCoord, yCoord, radius, 100, Color.Red, radius);
		}
	}
	
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		// making this pubilic and static is probably a bad idea?
		public static SpriteBatch spriteBatch;
		SpriteFont textFont;
		
		List<Planet> planets = new List<Planet>();
		string mode = "idle";

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

		// Drawing code
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();

			foreach (Planet planet in planets)
			{
				planet.Draw();
			}
			spriteBatch.DrawString(textFont, "Mode: " + mode, new Vector2(10, 10), Color.Black);
			
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
