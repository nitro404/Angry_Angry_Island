using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public class MainMenu : SubMenu {

		public MainMenu(Menu parentMenu, Vector2 position, Color titleColour, Color selectedItemColour, Color unselectedItemColour, Color arrowColour)
			: base("", parentMenu, position, titleColour, selectedItemColour, unselectedItemColour, arrowColour) {
		}

		public override void setContent(SpriteFont titleFont, SpriteFont itemFont) {
			base.setContent(titleFont, itemFont);
			createMenu();
		}

		// create the main menu elements
		public void createMenu() {
			float x = m_position.X;
			float y = m_position.Y + m_titleFont.LineSpacing;
			addItem(new SimpleMenuItem("Single Player", x, y, m_itemFont, m_selectedItemColour, m_unselectedItemColour, m_arrowColour));
			y += m_itemFont.LineSpacing;
			addItem(new SimpleMenuItem("Quit", x, y, m_itemFont, m_selectedItemColour, m_unselectedItemColour, m_arrowColour));
		}

		public override void up() {
			base.up();
		}

		public override void down() {
			base.down();
		}
		public override void left() {
			base.left();
		}

		public override void right() {
			base.right();
		}

		// handle input based on the current selected menu item
		public override void select() {
			if(m_index == 0) {
				m_parentMenu.setSubMenu(MenuType.SinglePlayer);
			}
			else if(m_index == 1) {
				Firecracker.interpreter.execute("quit");
			}
		}

	}

}
