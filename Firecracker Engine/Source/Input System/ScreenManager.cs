using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine {

	// all of the screens which are managed by the ScreenManager
	enum ScreenType { Game, Menu, Console }

	// the screen visibility change type
	enum ScreenVisibilityChange { None, Show, Hide, Toggle }

	class ScreenManager {

		// local variables
		private ScreenType m_activeScreen = ScreenType.Menu;

		bool fullScreenKeyPressed = false;

		// "global" variables
		private Firecracker m_game;
		private GameSettings m_settings;
		private CommandInterpreter m_interpreter;
		private ControlSystem m_controlSystem;
		private Menu m_menu;
		private GameConsole m_console;

		public ScreenManager() { }

		public void initialize(Firecracker game, GameSettings settings, CommandInterpreter interpreter, ControlSystem controlSystem, Menu menu, GameConsole console) {
			// initialize references to "global" variables
			m_game = game;
			m_settings = settings;
			m_interpreter = interpreter;
			m_controlSystem = controlSystem;
			m_menu = menu;
			m_console = console;
		}

		// toggle the specified screen
		public void toggle(ScreenType screen) {
			if(screen == ScreenType.Menu) {
                if(m_game.levelLoaded() || !m_menu.active)
                    m_menu.toggle();
			}
			if(screen == ScreenType.Console) {
				m_console.toggle();
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
				m_menu.close();
				m_console.close();
			}
			else if(screen == ScreenType.Menu) {
				m_menu.open();
			}
			else if(screen == ScreenType.Console) {
				m_console.open();
			}

			// set the active screen to the specified (and disable any others)
			m_activeScreen = screen;
		}

		// force disable a specified screen
		public void hide(ScreenType screen) {
			if(screen == ScreenType.Menu) {
				m_menu.close();
			}
			else if(screen == ScreenType.Console) {
				m_console.close();
			}

			updateActiveScreen();
		}

		// determine which screen now takes priority
		private void updateActiveScreen() {
			m_activeScreen = ScreenType.Game;
			if(m_menu.active) { m_activeScreen = ScreenType.Menu; }
			if(m_console.active) { m_activeScreen = ScreenType.Console; }
		}

		// allow the active screen to receive input from the user
		public void handleInput(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();

			bool alternateInput = false;

			if((keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt)) &&
				keyboard.IsKeyDown(Keys.Enter)) {
				if(!fullScreenKeyPressed) {
					m_game.toggleFullScreen();
					alternateInput = true;
					fullScreenKeyPressed = true;
				}
			}
			else { fullScreenKeyPressed = false; }

			if(alternateInput) { return; }

			if(m_activeScreen == ScreenType.Game) { m_game.handleInput(gameTime); }

			if(m_activeScreen != ScreenType.Console) {
				m_menu.handleInput(gameTime);
			}

			m_console.handleInput(gameTime);
		}

		// update the active screen
		public void update(GameTime gameTime) {
			if(m_menu.active) { m_menu.update(gameTime); }
			if(m_console.active) { m_console.update(gameTime); }
		}

		// draw the active screen
		public void draw(SpriteBatch spriteBatch, GraphicsDevice graphics) {
			if(m_menu.active || m_console.active) {
				spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.SaveState);
			}

			if(m_menu.active) { m_menu.draw(spriteBatch); }
			if(m_console.active) { m_console.draw(spriteBatch); }

			if(m_menu.active || m_console.active) {
				spriteBatch.End();
			}
		}

	}

}
