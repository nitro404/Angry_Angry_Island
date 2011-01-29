﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class Explosion : GameObject
    {
        SpriteAnimation m_Anim;

        public Explosion()
            : base()
        {
            m_Anim = new SpriteAnimation("Explosion", 0.128f, SpriteAnimationType.Single);
            position = new Vector2();
        }

        public Explosion(Vector2 vPosition)
            : base()
        {
            m_Anim = new SpriteAnimation("Explosion", 0.128f, SpriteAnimationType.Single);
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