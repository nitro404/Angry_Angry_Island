using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Firecracker_Engine {

	public enum SpriteAnimationType { Invalid = -1, Single, Loop }

	public class SpriteAnimation {

		private String m_name;
		private List<Sprite> m_sprites;
		private SpriteAnimationType m_type;
		private float m_duration;
		private float m_interval;
		private float m_sequence;
		private int m_index;
		private bool m_finished;

		public static String[] spriteAnimationTypeStrings = { "Invalid", "Single", "Loop" };

		private SpriteAnimation() { }

		public SpriteAnimation(String name, float duration, SpriteAnimationType type) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_duration = (duration <= 0) ? 1 : duration;
			m_interval = 0;
			m_sequence = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;
		}

		public SpriteAnimation(String name, float duration, SpriteAnimationType type, Sprite[] sprites) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_duration = (duration <= 0) ? 1 : duration;
			m_interval = 0;
			m_sequence = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(sprites);
		}

		public SpriteAnimation(String name, float duration, SpriteAnimationType type, List<Sprite> sprites) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_duration = (duration <= 0) ? 1 : duration;
			m_interval = 0;
			m_sequence = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(sprites);
		}

		public SpriteAnimation(String name, float duration, SpriteAnimationType type, SpriteSheet spriteSheet) {
			m_name = (name == null) ? "" : name;
			m_sprites = new List<Sprite>();
			m_duration = (duration <= 0) ? 1 : duration;
			m_interval = 0;
			m_sequence = 0;
			m_type = type;
			m_index = 0;
			m_finished = false;

			addSprites(spriteSheet);
		}

		public SpriteAnimation getInstance() {
			return new SpriteAnimation(m_name, m_duration, m_type, m_sprites);
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

		public Sprite getSprite(int index) {
			if(index < 0 || index >= m_sprites.Count()) { return null; }

			return m_sprites[index];
		}

		// add a frame to the animation
		public void addSprite(Sprite sprite) {
			if(sprite == null) { return; }
			m_sprites.Add(sprite);
			m_interval = m_duration / m_sprites.Count();
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
		
		// if the animation is not set to loop, has it finished animating?
		public bool finished() {
			return m_finished;
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

		public void draw(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.draw(spriteBatch, scale, rotation, position, effect, Color.White);
			}
		}

		public void drawCentered(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.drawCentered(spriteBatch, scale, rotation, position, effect, Color.White);
			}
		}

		public void drawWithOffset(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect, Vector2 offset) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.drawWithOffset(spriteBatch, scale, rotation, position, effect, Color.White, offset);
			}
		}

		// draw the currently active animation frame sprite
		public void draw(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect, Color colour) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.draw(spriteBatch, scale, rotation, position, effect, colour);
			}
		}

		// draw the currently active animation frame sprite centered
		public void drawCentered(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect, Color colour) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.drawCentered(spriteBatch, scale, rotation, position, effect, colour);
			}
		}

		public void drawWithOffset(SpriteBatch spriteBatch, Vector2 scale, float rotation, Vector2 position, SpriteEffects effect, Color colour, Vector2 offset) {
			if(m_sprites.Count() == 0) { return; }

			if(!finished()) {
				sprite.drawWithOffset(spriteBatch, scale, rotation, position, effect, colour, offset);
			}
		}

		public static SpriteAnimationType parseType(String data) {
			if(data == null) { return SpriteAnimationType.Invalid; }
			string temp = data.Trim();

			for(int i=0;i<spriteAnimationTypeStrings.Length;i++) {
				if(temp.Equals(spriteAnimationTypeStrings[i], StringComparison.OrdinalIgnoreCase)) {
					return (SpriteAnimationType) (i - 1);
				}
			}

			return SpriteAnimationType.Invalid;
		}

		public static SpriteAnimation parseFrom(StreamReader input, SpriteSheetCollection spriteSheets) {
			if(input == null || spriteSheets == null) { return null; }

			SpriteAnimation animation;
			VariableSystem animationProperties = new VariableSystem();

			// store all of the animation properties
			String data;
			Variable property;
			do {
				data = input.ReadLine();
				if(data == null) { return null; }

				data = data.Trim();
				if(data.Length == 0) { continue; }

				property = Variable.parseFrom(data);
				if(property == null) { return null; }

				animationProperties.add(property);
			} while(animationProperties.size() < 6);

			// parse the animation name
			String animationName = animationProperties.getValue("Animation Name");
			if(animationName == null) { return null; }

			// parse the animation type
			SpriteAnimationType animationType = parseType(animationProperties.getValue("Animation Type"));
			if(animationType == SpriteAnimationType.Invalid) { return null; }

			// parse the duration of the animation
			float duration;
			try {
				duration = float.Parse(animationProperties.getValue("Animation Duration"));
			}
			catch(Exception) { return null; }
			if(duration <= 0) { return null; }

			// parse the number of frames in the animation
			int numberOfFrames;
			try {
				numberOfFrames = int.Parse(animationProperties.getValue("Number of Frames"));
			}
			catch(Exception) { return null; }
			if(numberOfFrames <= 0) { return null; }

			// get the sprite sheet name
			String spriteSheetName = animationProperties.getValue("SpriteSheet Name");
			if(spriteSheetName == null) { return null; }

			// get the sprite name
			String spriteName = animationProperties.getValue("Sprite Name");
			if(spriteName == null) { return null; }

			animation = new SpriteAnimation(animationName, duration, animationType);

			// get the sprite sheet which contains the animation
			SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName);
			if(spriteSheet == null) { return null; }

			// obtain and add the sprites to the sprite animation
			Sprite sprite;
			for(int i=1;i<numberOfFrames;i++) {
				sprite = spriteSheet.getSprite(spriteName + " " + i);
				if(sprite == null) { return null; }

				animation.addSprite(sprite);
			}

			return animation;
		}
	}

}
