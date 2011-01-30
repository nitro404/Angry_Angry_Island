using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class BigExplosion : GameObject
    {
        SpriteAnimation m_Anim;

        public BigExplosion()
            : base()
        {
            m_Anim = Firecracker.animations.getAnimation("Big Explosion");
            position = Vector2.Zero;
        }

        public BigExplosion(Vector2 vPosition)
            : base()
        {
            m_Anim = Firecracker.animations.getAnimation("Big Explosion");
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
            m_Anim.drawWithOffset(spriteBatch, Vector2.One, 0, position, SpriteEffects.None, new Vector2(110, 214));
            base.draw(spriteBatch);
        }
    }
}
