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
        Texture2D cursorTex;


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
            //Firecracker.engineInstance.elapsedTime[0] = 0; Firecracker.engineInstance.elapsedTime[1] = 0;

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
            cursorTex = Content.Load<Texture2D>(@"Sprites\god_hand");
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
            Firecracker.engineInstance.elapsedTime += gameTime.ElapsedGameTime.TotalSeconds; //gameTime.TotalGameTime.Seconds;
            //Firecracker.engineInstance.elapsedTime[0] += gameTime.ElapsedGameTime.TotalMinutes;//gameTime.TotalGameTime.Minutes;


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

                if (screenManager.activeScreen == ScreenType.Game)
                {
                    HandleInput((float)gameTime.ElapsedGameTime.TotalSeconds);
                }
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
                if (levelLoaded())
                {
                    level.draw(spriteBatch);
                }
                spriteBatch.End();

				if(UIScreenManager.Instance.currentScreen != null) {
					UIScreenManager.Instance.Draw(GraphicsDevice, spriteBatch);
				}
                //draw cursor
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
                spriteBatch.Draw(cursorTex, m_MouseManager.GetMousePos() - new Vector2(5, 2), Color.White);
                spriteBatch.End();
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

        public void HandleInput(float fTime)
        {
            //mouse scrolling
            const int SCROLL_BOUNDARY = 20;
            const float SCROLL_SPEED = 200; //in pixels
            Vector2 mousePos = m_MouseManager.GetMousePos();
            Vector2 screenSize = new Vector2(settings.screenWidth, settings.screenHeight);
            if (mousePos.X < SCROLL_BOUNDARY ||
                mousePos.X >= screenSize.X - SCROLL_BOUNDARY ||
                mousePos.Y < SCROLL_BOUNDARY ||
                mousePos.Y >= screenSize.Y - SCROLL_BOUNDARY)
            {
                Vector2 scrollAmount = mousePos - screenSize / 2.0f;
                scrollAmount.Normalize();
                scrollAmount *= SCROLL_SPEED * fTime;
                theCamera.SetCameraPos(theCamera.GetCameraPos().X + scrollAmount.X, theCamera.GetCameraPos().Y + scrollAmount.Y);
            }

            //keyboard scrolling
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) //thekeyboard.IsKeyDown(Keys.Left))
            {
                theCamera.MoveCameraLeft();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right)) //(thekeyboard.IsKeyDown(Keys.Right))
            {
                theCamera.MoveCameraRight();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up)) //(thekeyboard.IsKeyDown(Keys.Up))
            {
                theCamera.MoveCameraUp();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down)) //(thekeyboard.IsKeyDown(Keys.Down))
            {
                theCamera.MoveCameraDown();
            }
        }
	}
}