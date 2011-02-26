using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class HumanZap : GameObject
    {
        SpriteAnimation m_Anim;

        public HumanZap()
            : base()
        {
			m_Anim = Firecracker.animations.getAnimation("HumanZap");
            position = Vector2.Zero;
        }

        public HumanZap(Vector2 vPosition)
            : base()
        {
			m_Anim = Firecracker.animations.getAnimation("HumanZap");
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
            m_Anim.drawCentered(spriteBatch, Vector2.One, 0, position, SpriteEffects.None);
        }
    }
}
