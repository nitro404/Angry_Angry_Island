using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    class Settlement : GameObject
    {

        Sprite[] sprites;

        public Settlement()
            : base()
        {
            position = Vector2.Zero;
            sprites = new Sprite[]{Firecracker.spriteSheets.getSpriteSheet("Hut").getSprite("Hut"),
                       Firecracker.spriteSheets.getSpriteSheet("Hamlet").getSprite("Hamlet"),
                       Firecracker.spriteSheets.getSpriteSheet("Settlement").getSprite("Settlement"),
                       Firecracker.spriteSheets.getSpriteSheet("City").getSprite("City")};
        }

        public Settlement(Vector2 vPosition)
            : base()
        {
            position = vPosition;
            sprites = new Sprite[]{Firecracker.spriteSheets.getSpriteSheet("Hut").getSprite("Hut"),
                       Firecracker.spriteSheets.getSpriteSheet("Hamlet").getSprite("Hamlet"),
                       Firecracker.spriteSheets.getSpriteSheet("Settlement").getSprite("Settlement"),
                       Firecracker.spriteSheets.getSpriteSheet("City").getSprite("City")};
            foreach (Sprite building in sprites)
            {
                building.m_SpriteDepth = 0.51f;
            }
        }

        public override void update(GameTime gameTime)
        {
            
            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            sprites[(int)PopulationManager.Instance.age].drawCentered(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
            base.draw(spriteBatch);
        }

        public static Settlement parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
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
