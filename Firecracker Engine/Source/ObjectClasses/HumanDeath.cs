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
			m_Anim = Firecracker.animations.getAnimation("HumanDeath");
            position = Vector2.Zero;
            //m_Anim.sprite.m_SpriteDepth = 0.522f;
            m_Anim.SetAnimDepthLayer(0.5f);
        }

        public HumanDeath(Vector2 vPosition)
            : base()
        {
			m_Anim = Firecracker.animations.getAnimation("HumanDeath");
            position = vPosition;
            //m_Anim.sprite.m_SpriteDepth = 0.522f;
            m_Anim.SetAnimDepthLayer(0.5f);
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
