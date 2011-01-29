using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    class HumanDeath : GameObject
    {
        SpriteAnimation m_Anim;

        public HumanDeath()
            : base()
        {
            m_Anim = new SpriteAnimation(null, 64, SpriteAnimationType.Single);
        }

        public HumanDeath(Vector2 vPosition)
            : base()
        {
            m_Anim = new SpriteAnimation(null, 0.128f, SpriteAnimationType.Single);
            SpriteSheet spriteSheet = Firecracker.spriteSheets.getSpriteSheet("HumanDeath");
            if (spriteSheet != null)
            {
                for (int i = 1; i < 18; i++)
                {
                    Sprite aSprite = spriteSheet.getSprite(string.Concat("HumanDeath_", i.ToString()));
                    m_Anim.addSprite(aSprite);
                }
            }
            position = vPosition;
        }

        public override void update(GameTime gameTime)
        {
            m_Anim.update(gameTime);
            if (m_Anim.finished()) toBeDeleted = true;
            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            m_Anim.draw(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
            base.draw(spriteBatch);
        }
    }
}
