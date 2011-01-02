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

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public Firecracker() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

		}

		protected override void UnloadContent() {
			
		}

		protected override void Update(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();
			GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

			if(keyboard.IsKeyDown(Keys.Escape) || gamePad.Buttons.Back == ButtonState.Pressed) {
				Exit();
			}

			

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			

			base.Draw(gameTime);
		}

	}

}
