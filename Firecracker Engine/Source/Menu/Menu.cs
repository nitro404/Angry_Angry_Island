using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Firecracker_Engine {

	public enum MenuType { Main, SinglePlayer }

	public class Menu {

		// local variables
		private bool m_active = true;
		private MenuType m_currentMenu = MenuType.Main;
		private List<SubMenu> m_menu;
		private Vector2 m_position;
		private SpriteFont m_titleFont;
		private SpriteFont m_itemFont;
		private Color m_titleColour;
		private Color m_selectedItemColour;
		private Color m_unselectedItemColour;
		private Color m_arrowColour;

		// boolean locking variables
		private bool m_backKeyPressed = false;
		private bool m_selectKeyPressed = false;
		private bool m_upKeyPressed = false;
		private bool m_downKeyPressed = false;
		private bool m_leftKeyPressed = false;
		private bool m_rightKeyPressed = false;

		public Menu() {
			m_menu = new List<SubMenu>();
			m_position = new Vector2(50, 400);
			m_titleColour = new Color(60, 50, 40);
			m_selectedItemColour = new Color(196, 170, 128);
			m_unselectedItemColour = new Color(128, 108, 96);
			m_arrowColour = new Color(255, 255, 255);
		}

		// initialize the menu
		public void initialize() {
			// add all sub-menus to the sub-menu collection
			m_menu.Add(new MainMenu(this, m_position, m_titleColour, m_selectedItemColour, m_unselectedItemColour, m_arrowColour));
			m_menu.Add(new GameSinglePlayerMenu(this, m_position, m_titleColour, m_selectedItemColour, m_unselectedItemColour, m_arrowColour));
		}

		// load menu content
		public void loadContent(ContentManager content) {
			// load menu fonts
			m_titleFont = content.Load<SpriteFont>("Fonts/MenuTitleFont");
			m_itemFont = content.Load<SpriteFont>("Fonts/MenuItemFont");

			// set fonts for all sub-menus
			for(int i=0;i<m_menu.Count();i++) {
				m_menu[i].setContent(m_titleFont, m_itemFont);
			}
		}

		public bool active {
			get { return m_active; }
			set { m_active = value; }
		}

		public void toggle() {
			m_active = !m_active;
			reset();
		}

		public void open() {
			m_active = true;
			reset();
		}

		public void close() {
			m_active = false;
			reset();
		}

		public void handleInput(GameTime gameTime) {
			KeyboardState keyboard = Keyboard.GetState();
			GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

			bool menuWasOpen = m_active;

			// activate the menu if it was not already active (or close it if appropriate)
			if(keyboard.IsKeyDown(Keys.Escape) ||
			   gamePad.IsButtonDown(Buttons.Back)) {
				if(!m_backKeyPressed) {
					back();
					m_backKeyPressed = true;
				}
			}
			else { m_backKeyPressed = false; }

			// if the menu was not already open, stop (already handled the command to open it)
			if(!menuWasOpen) { return; }

			// check for menu select input
			if(keyboard.IsKeyDown(Keys.Space) ||
			   keyboard.IsKeyDown(Keys.Enter) ||
			   gamePad.IsButtonDown(Buttons.A)) {
				if(!m_selectKeyPressed) {
					select();
					m_selectKeyPressed = true;
				}
			}
			else { m_selectKeyPressed = false; }

			// check for move menu selection up input
			if(keyboard.IsKeyDown(Keys.W) ||
			   keyboard.IsKeyDown(Keys.Up) ||
			   keyboard.IsKeyDown(Keys.NumPad8) ||
			   gamePad.ThumbSticks.Left.Y > 0 ||
			   gamePad.ThumbSticks.Right.Y > 0 ||
			   gamePad.IsButtonDown(Buttons.DPadUp)) {
				if(!m_upKeyPressed) {
					up();
					m_upKeyPressed = true;
				}
			}
			else { m_upKeyPressed = false; }

			// check for move menu selection down input
			if(keyboard.IsKeyDown(Keys.S) ||
			   keyboard.IsKeyDown(Keys.Down) ||
			   keyboard.IsKeyDown(Keys.NumPad2) ||
			   gamePad.ThumbSticks.Left.Y < 0 ||
			   gamePad.ThumbSticks.Right.Y < 0 ||
			   gamePad.IsButtonDown(Buttons.DPadDown)) {
				if(!m_downKeyPressed) {
					down();
					m_downKeyPressed = true;
				}
			}
			else { m_downKeyPressed = false; }

			// check for move menu selection left input
			if(keyboard.IsKeyDown(Keys.A) ||
			   keyboard.IsKeyDown(Keys.Left) ||
			   keyboard.IsKeyDown(Keys.NumPad4) ||
			   gamePad.ThumbSticks.Left.X < 0 ||
			   gamePad.ThumbSticks.Right.X < 0 ||
			   gamePad.IsButtonDown(Buttons.DPadLeft)) {
				if(!m_leftKeyPressed) {
					left();
					m_leftKeyPressed = true;
				}
			}
			else { m_leftKeyPressed = false; }

			// check for move menu selection right input
			if(keyboard.IsKeyDown(Keys.D) ||
			   keyboard.IsKeyDown(Keys.Right) ||
			   keyboard.IsKeyDown(Keys.NumPad6) ||
			   gamePad.ThumbSticks.Left.X > 0 ||
			   gamePad.ThumbSticks.Right.X > 0 ||
			   gamePad.IsButtonDown(Buttons.DPadRight)) {
				if(!m_rightKeyPressed) {
					right();
					m_rightKeyPressed = true;
				}
			}
			else { m_rightKeyPressed = false; }
		}

		public void up() {
			m_menu[(int) m_currentMenu].up();
		}

		public void down() {
			m_menu[(int) m_currentMenu].down();
		}
		public void left() {
			m_menu[(int) m_currentMenu].left();
		}

		public void right() {
			m_menu[(int) m_currentMenu].right();
		}

		// navigate to the previous sub-menu (or toggle the menu, if appropriate)
		public void back() {
			if(m_currentMenu == MenuType.Main) {
				Firecracker.interpreter.execute("menu toggle");
			}
			else if(m_currentMenu == MenuType.SinglePlayer) {
				m_menu[(int) m_currentMenu].index = 0;
				m_currentMenu = MenuType.Main;
			}
		}

		// select the current active menu item within the active sub-menu
		public void select() {
			m_menu[(int) m_currentMenu].select();
		}

		// set the current active sub-menu
		public void setSubMenu(MenuType subMenu) {
			m_menu[(int) m_currentMenu].index = 0;
			m_currentMenu = subMenu;
		}

		// reset the menu (and all sub-menus)
		public void reset() {
			for(int i=0;i<m_menu.Count();i++) {
				m_menu[i].reset();
			}
			m_currentMenu = MenuType.Main;
		}

		// update the current active sub-menu
		public void update(GameTime gameTime) {
			m_menu[(int) m_currentMenu].update(gameTime);
		}

		// draw the current active sub-menu
		public void draw(SpriteBatch spriteBatch) {
			m_menu[(int) m_currentMenu].draw(spriteBatch);
		}

	}

}
