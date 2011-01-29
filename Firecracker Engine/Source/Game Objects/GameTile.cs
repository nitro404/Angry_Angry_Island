using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Firecracker_Engine {

	public class GameTile : GameObject {

		public GameTile(Vertex position, Sprite sprite) : this(position.toVector(), sprite) { }

		public GameTile(Vector2 position, Sprite sprite) : base() {
			m_position = position;
			this.sprite = sprite;
			updateInitialValues();
		}

		public static GameTile parseFrom(StreamReader input, SpriteSheetCollection spriteSheets) {
			if(input == null || spriteSheets == null) { return null; }

			// get the object's position
			Variable position = Variable.parseFrom(input.ReadLine());
			if(position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }
            
            // Get the layer depth of this sprite
            Variable layerDepth = Variable.parseFrom(input.ReadLine());
            if (layerDepth == null || !layerDepth.id.Equals("LayerDepth", StringComparison.OrdinalIgnoreCase)) { return null; }

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
			GameTile newObject = new GameTile(newPosition, sprite);
            newObject.sprite.m_SpriteDepth = float.Parse(layerDepth.value);
			newObject.updateInitialValues();

			return newObject;
		}

		public override void draw(SpriteBatch spriteBatch) {
			if (m_sprite == null) { return; }
			m_sprite.draw(spriteBatch, m_scale, m_rotation, Firecracker.level.getGamePosition(m_position), SpriteEffects.None);
		}

	}

}
