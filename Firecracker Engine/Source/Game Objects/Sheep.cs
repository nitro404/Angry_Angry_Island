using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public class Sheep : AnimatedGameObject {

		private float m_colour;

		public Sheep(Vertex position, Sprite sprite) : this(position.toVector(), sprite) { }

		public Sheep(Vector2 position, Sprite sprite) : base() {
			this.sprite = sprite;
			this.position = position;
		}

		public Sheep parseFrom(StreamReader input, SpriteSheetCollection spriteSheets) {
			if(input == null || spriteSheets == null) { return null; }

			VariableSystem variables = new VariableSystem();
			Variable v;
			for(int i=0;i<4;i++) {
				v = Variable.parseFrom(input.ReadLine();
				if(v == null) { return null; }
				variables.add(v);
			}

			variables.getValue("Position");

			// get the object's position
			Variable position = Variable.parseFrom(input.ReadLine());
			if(position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the sprite's name
			Variable spriteName = Variable.parseFrom(input.ReadLine());
			if(spriteName == null || !spriteName.id.Equals("Sprite Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the name of the spritesheet in which the sprite is found
			Variable spriteSheetName = Variable.parseFrom(input.ReadLine());
			if(spriteSheetName == null || !spriteSheetName.id.Equals("SpriteSheet Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the sheep's colour
			Variable sheepColour = Variable.parseFrom(input.ReadLine());
			if(spriteName == null || !spriteName.id.Equals("Sprite Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the object's sprite
			SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName.value);
			if(spriteSheet == null) { return null; }
			Sprite sprite = spriteSheet.getSprite(spriteName.value);
			if(sprite == null) { return null; }

			// parse the sprite's position
			String[] positionData = position.value.Split(',');
			if(positionData.Length != 2) { return null; }

			Vector2 newPosition;
			try {
				newPosition.X = Int32.Parse(positionData[0]);
				newPosition.Y = Int32.Parse(positionData[1]);
			}
			catch(Exception) { return null; }

			// create the object
			Sheep newObject = new Sheep(newPosition, sprite);
			newObject.updateInitialValues();

			return newObject;
		}

		public override bool checkCollision(GameObject o) {
			return base.checkCollision(o);
		}

		public override void update(GameTime gameTime) {
			base.update(gameTime);
		}

		public override void draw(SpriteBatch spriteBatch) {
			base.draw(spriteBatch);
		}

	}

}
