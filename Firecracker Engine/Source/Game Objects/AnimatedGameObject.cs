using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

    public enum AnimDirection
    {
        DIR_N = 0,
        DIR_S,
        DIR_E,
        DIR_W,
        DIR_MAX
    }
	public class AnimatedGameObject : GameObject {

		protected List<SpriteAnimation> m_animations;
		protected int m_currentAnimation;

        protected AnimDirection m_eDir;
        protected bool m_bIsDirectionBased;

        protected bool m_bIsPlaying;

		public AnimatedGameObject() : base() {
			m_animations = new List<SpriteAnimation>();
			m_currentAnimation = -1;
		}
        public void SetAnimDir(AnimDirection theDir)
        {
            m_eDir = theDir;
            m_currentAnimation = (int)m_eDir;
        }

        public void SetIsDirectionBased(bool isDirBased)
        {
            m_bIsDirectionBased = isDirBased;
            if (isDirBased)
            {
                // make sure there are 4 anims.
                if (m_animations.Count < 4)
                 m_animations.AddRange(new SpriteAnimation[4-m_animations.Count]);
            }
        }

        public void SetDirAnimation(AnimDirection theDir, string sAnimName)
        {
            SpriteAnimation newAnim = Firecracker.animations.getAnimation(sAnimName);
            m_animations[(int)theDir] = newAnim;
        }

        public bool playAnimation(AnimDirection animDir)
        {
            m_eDir = animDir;
            return playAnimation((int)animDir);
        }

		public bool playAnimation(int animationNumber) {
			if(animationNumber < 0 || animationNumber >= m_animations.Count()) {
				return false;
			}

			m_currentAnimation = animationNumber;
			m_animations[m_currentAnimation].reset();
            m_bIsPlaying = true;
			return true;
		}

        public void PauseAnimation()
        {
            m_bIsPlaying = false;
        }

		public override void update(GameTime gameTime) {
			base.update(gameTime);
            if (m_bIsPlaying)
			    m_animations[m_currentAnimation].update(gameTime);
		}

		public override void draw(SpriteBatch spriteBatch) {
			if (m_currentAnimation < 0) {
				base.draw(spriteBatch);
			}
			else {
				m_animations[m_currentAnimation].draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
			}
		}

	}

}
