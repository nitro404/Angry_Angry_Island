using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public abstract class SubMenu {

		// local variables
		protected List<MenuItem> m_items;
		protected string m_title;
		protected int m_index = 0;
		protected Vector2 m_position;
		protected SpriteFont m_titleFont;
		protected SpriteFont m_itemFont;
		protected Color m_titleColour;
		protected Color m_selectedItemColour;
		protected Color m_unselectedItemColour;
		protected Color m_arrowColour;
		protected Menu m_parentMenu;

		public SubMenu(string title, Menu parentMenu, Vector2 position, Color titleColour, Color selectedItemColour, Color unselectedItemColour, Color arrowColour) {
			m_items = new List<MenuItem>();
			m_title = title;
			m_parentMenu = parentMenu;
			m_position = position;
			m_titleColour = titleColour;
			m_selectedItemColour = selectedItemColour;
			m_unselectedItemColour = unselectedItemColour;
			m_arrowColour = arrowColour;
		}

		// set the sub-menu fonts
		public virtual void setContent(SpriteFont titleFont, SpriteFont itemFont) {
			m_titleFont = titleFont;
			m_itemFont = itemFont;
		}

		// return the number of items in the sub-menu
		public int size() {
			return m_items.Count();
		}

		// return the current selection index of the sub-menu
		public int index {
			get { return m_index; }
			set { if(value >= 0 && value < m_items.Count()) { m_index = value; } }
		}

		// add a menu item to the sub-menu
		public bool addItem(MenuItem item) {
			if(item == null || m_items.Contains(item)) { return false; }
			m_items.Add(item);
			return true;
		}

		// get the current selected menu item from the sub-menu
		public MenuItem getItem(int index) {
			if(index < 0 || index >= m_items.Count()) { return null; }
			return m_items.ElementAt(index);
		}

		// move the menu selection up
		public virtual void up() {
			m_index--;
			if(m_index < 0) { m_index = m_items.Count() - 1; }
		}

		// move the menu selection down
		public virtual void down() {
			m_index++;
			if(m_index >= m_items.Count()) { m_index = 0; }
		}

		// move the selection in the selected menu item left
		public virtual void left() {
			m_items.ElementAt(m_index).left();
		}

		// move the selection in the selected menu item right
		public virtual void right() {
			m_items.ElementAt(m_index).right();
		}

		// select the current menu item (implemented in sub-classes)
		public abstract void select();

		// reset the sub-menu
		public void reset() {
			m_index = 0;
		}

		// update the sub-menu
		public void update(GameTime gameTime) {
			for(int i=0;i<m_items.Count();i++) {
				m_items[i].update(gameTime);
			}
		}

		// draw the contents of the sub-menu
		public void draw(SpriteBatch spriteBatch) {
			spriteBatch.DrawString(m_titleFont, m_title, m_position, m_titleColour);
			for(int i=0;i<m_items.Count();i++) {
				m_items[i].draw(spriteBatch, i == m_index);
			}
		}

	}

}
