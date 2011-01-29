using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public enum SpriteAnimationType { Single, Loop }

	public class SpriteAnimation {

		private String m_name;
		private List<Sprite> m_sprites;
		private SpriteAnimationType m_type;
		private float m_interval;
		private float m_sequence;
		private float m_end;
		private int m_index;
		private bool m_finished;

		public SpriteAnimation(String name, float interval, SpriteAnimationType type) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_interval = interval;
			m_sequence = 0;
			m_end = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;
		}

		public SpriteAnimation(String name, float interval, SpriteAnimationType type, Sprite[] sprites) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_interval = interval;
			m_sequence = 0;
			m_end = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(sprites);
		}

		public SpriteAnimation(String name, float interval, SpriteAnimationType type, List<Sprite> sprites) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_interval = interval;
			m_sequence = 0;
			m_end = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(sprites);
		}

		public SpriteAnimation(String name, float interval, SpriteAnimationType type, SpriteSheet spriteSheet) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_interval = interval;
			m_sequence = 0;
			m_end = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(spriteSheet);
		}

		public String name {
			get { return m_name; }
			set { if(value != null) { m_name = name; } }
		}

		// get the current sprite animation frame
		public Sprite sprite {
			//get { return (m_sprites.Count() == 0) ? null : m_sprites[(int) (m_sequence / m_interval)]; }
			get { return (m_sprites.Count() == 0) ? null : m_sprites[m_index]; }
		}

		public SpriteAnimationType type {
			get { return m_type; }
		}

		// get the number of frames in the animation
		public int size() {
			return m_sprites.Count();
		}

		// add a frame to the animation
		public void addSprite(Sprite sprite) {
			if(sprite == null) { return; }
			m_sprites.Add(sprite);
			m_end = m_interval * m_sprites.Count();
		}

		// add a collection of frames to the animation from an array
		public void addSprites(Sprite[] sprites) {
			if(sprites == null) { return; }

			for(int i=0;i<sprites.Length;i++) {
				addSprite(sprites[i]);
			}
		}

		// add a collection of frames to the animation from a list
		public void addSprites(List<Sprite> sprites) {
			if(sprites == null) { return; }

			for(int i=0;i<sprites.Count();i++) {
				addSprite(sprites[i]);
			}
		}

		// add a collection of frames to the animation from a sprite sheet
		public void addSprites(SpriteSheet spriteSheet) {
			if(spriteSheet == null) { return; }

			for(int i=0;i<spriteSheet.size();i++) {
				addSprite(spriteSheet.getSprite(i));
			}
		}

		// update (increment) the animation
		public void update(GameTime gameTime) {
			if(gameTime == null || finished()) { return; }

			if(m_type == SpriteAnimationType.Loop) {
				m_sequence += (float) (gameTime.ElapsedGameTime.TotalSeconds);
				if(m_sequence >= m_interval) {
					m_sequence -= m_interval;
					m_index++;
					if(m_index >= m_sprites.Count) {
						m_index = 0;
					}
				}
			}
			else if(m_type == SpriteAnimationType.Single) {
				m_sequence += (float) (gameTime.ElapsedGameTime.TotalSeconds);
				if(m_sequence > m_interval) {
					m_sequence -= m_interval;
					m_index++;
					if(m_index >= m_sprites.Count) {
						m_index = 0;
						m_finished = true;
					}
				}
			}
		}
		
		// restart the animation
		public void reset() {
			m_sequence = 0;
		}

		// draw the currently active animation frame sprite
		public void draw(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.draw(spriteBatch, scale, rotation, position - Firecracker.engineInstance.theCamera.GetCameraPos(), effect);
			}
		}

		// draw the currently active animation frame sprite centered
		public void drawCentered(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.drawCentered(spriteBatch, scale, rotation, position - Firecracker.engineInstance.theCamera.GetCameraPos(), effect);
			}
		}

		// if the animation is not set to loop, has it finished animating?
		public bool finished() {
			//return m_type == SpriteAnimationType.Single && m_sequence >= m_end;
			return m_finished;
		}

	}

}
