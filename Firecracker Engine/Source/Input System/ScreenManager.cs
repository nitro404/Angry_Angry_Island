using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine {

	// all of the screens which are managed by the ScreenManager
	public enum ScreenType { Game, Menu, Console }

	// the screen visibility change type
	public enum ScreenVisibilityChange { None, Show, Hide, Toggle }

	public class ScreenManager {

		// local variables
		private ScreenType m_activeScreen = ScreenType.Menu;
		bool fullScreenKeyPressed = false;

		public ScreenManager() { }

		public ScreenType activeScreen {
			get { return m_activeScreen; }
		}

		// toggle the specified screen
		public void toggle(ScreenType screen) {
			if(screen == ScreenType.Menu) {
				if(Firecracker.engineInstance.levelLoaded() || !Firecracker.menu.active)
					Firecracker.menu.toggle();
			}
			if(screen == ScreenType.Console) {
				Firecracker.console.toggle();
			}

			// since screens can be layered, determine which screen now takes priority
			updateActiveScreen();
		}

		// manually change the visibility of a specific screen
		public void set(ScreenType screen, ScreenVisibilityChange visibility) {
				 if(visibility == ScreenVisibilityChange.Toggle) { toggle(screen); }
			else if(visibility == ScreenVisibilityChange.Show) { show(screen); }
			else if(visibility == ScreenVisibilityChange.Hide) { hide(screen); }
		}

		// force enable a specified screen
		public void show(ScreenType screen) {
			if(screen == ScreenType.Game) {
				Firecracker.menu.close();
				Firecracker.console.close();
			}
			else if(screen == ScreenType.Menu) {
				Firecracker.menu.open();
			}
			else if(screen == ScreenType.Console) {
				Firecracker.console.open();
			}

			// set the active screen to the specified (and disable any others)
			m_activeScreen = screen;
		}

		// force disable a specified screen
		public void hide(ScreenType screen) {
			if(screen == ScreenType.Menu) {
				Firecracker.menu.close();
			}
			else if(screen == ScreenType.Console) {
				Firecracker.console.close();
			}

			updateActiveScreen();
		}

		// determine which screen now takes priority
		private void updateActiveScreen() {
			m_activeScreen = ScreenType.Game;
			if(Firecracker.menu.active) { m_activeScreen = ScreenType.Menu; }
			if(Firecracker.console.active) { m_activeScreen = ScreenType.Console; }
		}

		// allow the active screen to receive input from the user
		public void handleInput(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();

			bool alternateInput = false;

			if((keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) &&
				keyboard.IsKeyDown(Keys.Enter)) {
				if(!fullScreenKeyPressed) {
					Firecracker.engineInstance.toggleFullScreen();
					alternateInput = true;
					fullScreenKeyPressed = true;
				}
			}
			else { fullScreenKeyPressed = false; }

			if(alternateInput) { return; }

			if(m_activeScreen == ScreenType.Game) { Firecracker.engineInstance.handleInput(gameTime); }

			if(m_activeScreen != ScreenType.Console) {
				Firecracker.menu.handleInput(gameTime);
			}

			Firecracker.console.handleInput(gameTime);
		}

		// update the active screen
		public void update(GameTime gameTime) {
			if(Firecracker.menu.active) { Firecracker.menu.update(gameTime); }
			if(Firecracker.console.active) { Firecracker.console.update(gameTime); }
			if(!Firecracker.menu.active && !Firecracker.console.active) {
				Firecracker.engineInstance.updateGame(gameTime);
                PopulationManager.Instance.Update(gameTime);
				//mouse showing code is temporary.. should just draw a sprite in UIScreenManager.Draw or Screen.Draw
				Firecracker.engineInstance.IsMouseVisible = false;
				UIScreenManager.Instance.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
			}
			else {
				Firecracker.engineInstance.IsMouseVisible = false;
			}
		}

		// draw the active screen
		public void draw(SpriteBatch spriteBatch, GraphicsDevice graphics) {
			if(Firecracker.menu.active || Firecracker.console.active) {
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
			}

			if(Firecracker.menu.active) { Firecracker.menu.draw(spriteBatch); }
			if(Firecracker.console.active) { Firecracker.console.draw(spriteBatch); }

			if(Firecracker.menu.active || Firecracker.console.active) {
				spriteBatch.End();
			}
		}

	}

}
