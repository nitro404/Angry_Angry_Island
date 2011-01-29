using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine {

	class StaticObject : GameObject {

		public StaticObject(Vertex position, Sprite sprite) : this(position.toVector(), sprite) { }

		public StaticObject(Vector2 position, Sprite sprite) : base() {
			this.sprite = sprite;
			this.position = position;
		}

		public static StaticObject parseFrom(StreamReader input, SpriteSheetCollection spriteSheets) {
			if(input == null || spriteSheets == null) { return null; }

			// get the object's position
			Variable position = Variable.parseFrom(input.ReadLine());
			if(position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the sprite's name
			Variable spriteName = Variable.parseFrom(input.ReadLine());
			if(spriteName == null || !spriteName.id.Equals("Sprite Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the name of the spritesheet in which the sprite is found
			Variable spriteSheetName = Variable.parseFrom(input.ReadLine());
			if(spriteSheetName == null || !spriteSheetName.id.Equals("SpriteSheet Name", StringComparison.OrdinalIgnoreCase)) { return null; }

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
			StaticObject newObject = new StaticObject(newPosition, sprite);
			newObject.updateInitialValues();

			return newObject;
		}

		public override bool checkCollision(GameObject o) {
			// TODO: implement object > object collision checking
			return false;
		}

	}

}
