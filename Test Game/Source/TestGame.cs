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
using Firecracker_Engine;

namespace Test_Game {

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class TestGame : Firecracker {

        public static TestGame gameInstance;
		protected RenderTarget2D buffer;
		protected Effect blur;

		public TestGame() : base() {
			gameInstance = this;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			base.Initialize();

			ObjectDefinitions.Initialize();

			// initialize the draw buffer
			buffer = new RenderTarget2D(graphics.GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 1, GraphicsDevice.DisplayMode.Format);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			base.LoadContent();

            UIInitializer.InitializeUI();

			// load shaders
			blur = Content.Load<Effect>("Shaders\\Blur");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			base.UnloadContent();
		}

		public override void updateGame(GameTime gameTime) {
			base.updateGame(gameTime);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			base.Update(gameTime);

			if(IsActive) {
				screenManager.handleInput(gameTime);
			}

			screenManager.update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			base.Draw(gameTime);

			graphics.GraphicsDevice.SetRenderTarget(0, buffer);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			if(levelLoaded()) {
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
				if(levelLoaded()) {
					level.draw(spriteBatch);
				}
				spriteBatch.End();

				if(UIScreenManager.Instance.currentScreen != null) {
					UIScreenManager.Instance.Draw(GraphicsDevice, spriteBatch);
				}
			}

			// render all game objects
			foreach(CBaseObject objRef in m_lObjectList) {
				objRef.Render();
			}

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
		}
	}
}