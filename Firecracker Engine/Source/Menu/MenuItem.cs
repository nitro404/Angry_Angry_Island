using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	abstract class MenuItem {

		protected string m_text;
		protected SpriteFont m_font;
		protected Vector2 m_position;
		protected Color m_selectedColour;
		protected Color m_unselectedColour;
		protected Color m_arrowColour;

		public MenuItem(string text, float x, float y, SpriteFont font, Color selectedColour, Color unselectedColour, Color arrowColour) {
			m_text = text;
			m_font = font;
			m_selectedColour = selectedColour;
			m_unselectedColour = unselectedColour;
			m_arrowColour = arrowColour;
			m_position = new Vector2(x, y);
		}

		public abstract void left();

		public abstract void right();

		public virtual void update(GameTime gameTime) { }

		public virtual void draw(SpriteBatch spriteBatch, bool selected) {
			if(selected) { spriteBatch.DrawString(m_font, ">", m_position, m_arrowColour); }
			spriteBatch.DrawString(m_font, m_text, m_position + new Vector2(15, 0), (selected) ? m_selectedColour : m_unselectedColour);
		}

	}

}
