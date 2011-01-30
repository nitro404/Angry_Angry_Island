using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

    public class Settlement : GameObject {

        private Sprite[] sprites;
		private SpriteAnimation[] destructionSprites;
		public int health = 2;

		public Settlement() : this(Vector2.Zero) { }

        public Settlement(Vector2 vPosition) : base() {
            position = vPosition;
            sprites = new Sprite[]{Firecracker.spriteSheets.getSpriteSheet("Hut").getSprite("Hut"),
                       Firecracker.spriteSheets.getSpriteSheet("Hamlet").getSprite("Hamlet"),
                       Firecracker.spriteSheets.getSpriteSheet("Settlement").getSprite("Settlement"),
                       Firecracker.spriteSheets.getSpriteSheet("City").getSprite("City")};
			destructionSprites = new SpriteAnimation[] { Firecracker.animations.getAnimation("Hut Destruction"),
														 Firecracker.animations.getAnimation("Village Destruction"),
														 Firecracker.animations.getAnimation("Old City Destruction"),
														 Firecracker.animations.getAnimation("City Destruction") };
            foreach(Sprite building in sprites) {
                building.m_SpriteDepth = 0.51f;
				foreach(SpriteAnimation a in destructionSprites) {
					a.SetAnimDepthLayer(0.51f);
				}
            }
        }

		public void attack() {
			health--;
			if(health < 0) health=0;
			
		}

        public override void update(GameTime gameTime) {
            base.update(gameTime);
			if(health == 0) {
				destructionSprites[(int) PopulationManager.Instance.age].update(gameTime);
				if(destructionSprites[(int) PopulationManager.Instance.age].finished()) {
					toBeDeleted = true;
					PopulationManager.Instance.age = PopulationManager.Age.Primitive;
					PopulationManager.Instance.TimeSpentAboveThreshold = 0;
				}
			}
        }

        public override void draw(SpriteBatch spriteBatch) {
			if(health > 0) {
				sprites[(int) PopulationManager.Instance.age].drawCentered(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
			}
			else {
				destructionSprites[(int) PopulationManager.Instance.age].drawCentered(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
			}
            base.draw(spriteBatch);
        }

        public static Settlement parseFrom(StreamReader input, SpriteSheetCollection spriteSheets) {
            if (input == null || spriteSheets == null) { return null; }

            // get the object's position
            Variable position = Variable.parseFrom(input.ReadLine());
            if (position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }

            // parse the sprite's position
            String[] positionData = position.value.Split(',');
            if (positionData.Length != 2) { return null; }

            Vector2 newPosition;
            try
            {
                newPosition.X = Int32.Parse(positionData[0]);
                newPosition.Y = Int32.Parse(positionData[1]);
            }
            catch (Exception) { return null; }

            // create the object
            Settlement newObject = new Settlement(newPosition);
            newObject.updateInitialValues();

            return newObject;
        }
    }
}
