using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public class AnimatedGameObject : GameObject {

		protected List<SpriteAnimation> m_animations;
		protected int m_currentAnimation;

		public AnimatedGameObject() : base() {
			m_animations = new List<SpriteAnimation>();
			m_currentAnimation = -1;
		}

		public bool playAnimation(int animationNumber) {
			if(animationNumber < 0 || animationNumber >= m_animations.Count()) {
				return false;
			}

			m_currentAnimation = animationNumber;
			m_animations[m_currentAnimation].reset();
			return true;
		}

		public override void update(GameTime gameTime) {
			base.update(gameTime);
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
