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

		GameSettings settings;
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
        InputManager theInput;  //create new instance of input manager

		bool fullScreenKeyPressed = false;

		public Firecracker() {
			settings = new GameSettings();
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
            //Initialize input settings
            theInput = new InputManager();

			// load the game settings from file
			settings.loadFrom(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			// set the screen resolution
			graphics.PreferredBackBufferWidth = settings.screenWidth;
			graphics.PreferredBackBufferHeight = settings.screenHeight;
			graphics.ApplyChanges();

			// set the screen attributes / full screen mode
			Window.AllowUserResizing = false;
            if (settings.fullScreen)
            {
                graphics.ToggleFullScreen();
            }
            //Program occasionally threw access violation when starting in full screen mode. Possibly a Dual Monitor Issue.
            //If you encounter this problem, comment out the previous if statement, and enable the following one.
            //if (settings.fullScreen)
            //    settings.fullScreen = false;

			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

		}

		protected override void UnloadContent() {
			
		}

		protected override void Update(GameTime gameTime) {

            theInput.Update(5);
			//KeyboardState keyboard = Keyboard.GetState();
			//GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

			bool alternateInput = false;

			if(IsActive) {
				//if(keyboard.IsKeyDown(Keys.Escape) || gamePad.Buttons.Back == ButtonState.Pressed) {
                if(theInput.IsKeyDown(Keys.Escape) || theInput.IsButtonPressed(0, Buttons.Back)){
					Exit();
				}

                //if((keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) &&
                //    keyboard.IsKeyDown(Keys.Enter)) {
                if((theInput.IsKeyDown(Keys.LeftAlt) || theInput.IsKeyDown(Keys.RightAlt)) &&
                    theInput.IsKeyDown(Keys.Enter)){
					if(!fullScreenKeyPressed) {
						graphics.ToggleFullScreen();
						settings.fullScreen = graphics.IsFullScreen;
						alternateInput = true;
						fullScreenKeyPressed = true;
					}
				}
				else { fullScreenKeyPressed = false; }

				if(!alternateInput) {

				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			

			base.Draw(gameTime);
		}

		protected override void OnExiting(object sender, EventArgs args) {
			// update the game settings file with changes
			settings.saveTo(Content.RootDirectory + "/" + GameSettings.defaultFileName);

			base.OnExiting(sender, args);
		}

	}

}
