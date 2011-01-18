using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public class SimpleMenuItem : MenuItem {

		public SimpleMenuItem(string text, float x, float y, SpriteFont font, Color selectedColour, Color unselectedColour, Color arrowColour)
			: base(text, x, y, font, selectedColour, unselectedColour, arrowColour) { }

		public override void left() { }
		public override void right() { }

	}

}
