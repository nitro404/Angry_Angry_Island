using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine {

	public class SpriteAnimationCollection {

		private List<SpriteAnimation> m_animations;

		public SpriteAnimationCollection() {
			m_animations = new List<SpriteAnimation>();
		}

		public SpriteAnimationCollection(List<SpriteAnimation> animations) {
			m_animations = new List<SpriteAnimation>();

			addAnimations(animations);
		}

		public SpriteAnimationCollection(SpriteAnimation[] animations) {
			m_animations = new List<SpriteAnimation>();

			addAnimations(animations);
		}


		public void addAnimations(List<SpriteAnimation> animations) {
			if(animations == null) { return; }

			for(int i=0;i<animations.Count();i++) {
				addAnimation(m_animations[i]);
			}
		}

		public void addAnimations(SpriteAnimation[] animations) {
			if(animations == null) { return; }

			for(int i=0;i<animations.Count();i++) {
				addAnimation(m_animations[i]);
			}
		}

		public bool addAnimation(SpriteAnimation animation) {
			if(animation == null) { return false; }

			if(!m_animations.Contains(animation)) {
				m_animations.Add(animation);
				return true;
			}
			return false;
		}

		public SpriteAnimation getAnimation(int index) {
			if(index < 0 || index >= m_animations.Count()) { return null; }

			return m_animations[index];
		}

		public SpriteAnimation getAnimation(String animationName) {
			if(animationName == null) { return null; }

			String name = animationName.Trim();

			for(int i=0;i<m_animations.Count();i++) {
				if(m_animations[i].name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
					return m_animations[i];
				}
			}
			return null;
		}

		public int getAnimationIndex(String animationName) {
			if(animationName == null) { return -1; }

			String name = animationName.Trim();

			for(int i=0;i<m_animations.Count();i++) {
				if(m_animations[i].name.Equals(name, StringComparison.OrdinalIgnoreCase)) {
					return i;
				}
			}
			return -1;
		}


	}

}
