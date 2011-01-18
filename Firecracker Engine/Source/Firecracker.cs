using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Firecracker_Engine {
	
	public class Firecracker : Microsoft.Xna.Framework.Game {

		public static GameSettings settings;
		public static ScreenManager screenManager;
		public static CommandInterpreter interpreter;
		public static ControlSystem controlSystem;
		public static Menu menu;
		public static GameConsole console;
		protected GraphicsDeviceManager graphics;
		protected SpriteBatch spriteBatch;
		public static SpriteSheetCollection spriteSheets;
		protected RenderTarget2D buffer;
		protected Effect blur;

		public Firecracker() {
			settings = new GameSettings();
			screenManager = new ScreenManager();
			graphics = new GraphicsDeviceManager(this);
			interpreter = new CommandInterpreter();
			controlSystem = new ControlSystem();
			menu = new Menu();
			console = new GameConsole();
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			// load the game settings from file
			settings.loadFrom(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			// set the screen resolution
			graphics.PreferredBackBufferWidth = settings.screenWidth;
			graphics.PreferredBackBufferHeight = settings.screenHeight;
			graphics.ApplyChanges();

			// set the screen attributes / full screen mode
			Window.AllowUserResizing = false;
			if(settings.fullScreen) {
				// NOTE: may cause access violations in dual screen situations
				graphics.ToggleFullScreen();
			}

			// initialize the draw buffer
			buffer = new RenderTarget2D(graphics.GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 1, GraphicsDevice.DisplayMode.Format);

			screenManager.initialize(this);
			interpreter.initialize(this);
			controlSystem.initialize();
			menu.initialize();
			console.initialize();

			base.Initialize();
		}

		protected override void LoadContent() {
			// load shaders
			blur = Content.Load<Effect>("Shaders\\Blur");

			spriteBatch = new SpriteBatch(GraphicsDevice);

			// parse and load all sprite sheets
			spriteSheets = SpriteSheetCollection.parseFrom(Content.RootDirectory + "/" + settings.spriteSheetFileName, Content);

			// load game content
			menu.loadContent(Content);

			console.loadContent(Content);
		}

		public void toggleFullScreen() {
			graphics.ToggleFullScreen();
			settings.fullScreen = graphics.IsFullScreen;
		}

		public bool loadLevel(string levelName) {
			if(levelName == null) { return false; }

			// TODO: implement me!

			return false;
		}

		public bool levelLoaded() {
			//return level != null;

			// TODO: implement me!

			return false;
		}

		public void handleInput(GameTime gameTime) {
			// handle game-related input, and update the game
			controlSystem.handleInput(gameTime);
		}

		protected override void Update(GameTime gameTime) {
			if(IsActive) {
				screenManager.handleInput(gameTime);
			}

			screenManager.update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			graphics.GraphicsDevice.SetRenderTarget(0, buffer);
			GraphicsDevice.Clear(Color.Black);

			// disable the render target for post processing
			graphics.GraphicsDevice.SetRenderTarget(0, null);
			
			// blur game screen if menu is open
			if(menu.active) {
				blur.Begin();
				spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.SaveState);
				foreach(EffectTechnique t in blur.Techniques) {
					foreach(EffectPass p in t.Passes) {
						p.Begin();
						spriteBatch.Draw(buffer.GetTexture(), Vector2.Zero, Color.White);
						p.End();
					}
				}
				spriteBatch.End();
				blur.End();
			}
			else {
				spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.SaveState);
				spriteBatch.Draw(buffer.GetTexture(), Vector2.Zero, Color.White);
				spriteBatch.End();
			}
			
			screenManager.draw(spriteBatch, graphics.GraphicsDevice);

			base.Draw(gameTime);
		}

		protected override void OnExiting(object sender, EventArgs args) {
			// update the game settings file with changes
			settings.saveTo(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			base.OnExiting(sender, args);
		}

	}

}
