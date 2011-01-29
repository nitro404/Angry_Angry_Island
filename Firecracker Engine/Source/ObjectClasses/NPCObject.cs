using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class NPCObject : GameObject
    {
        public enum AIWanderType
        {
            AI_Random,
            AI_RandomTowardsNearest,
            AI_Path,
            AI_None,
        }

        private float m_colour;

        protected float m_fAge;
        protected bool m_bIsMoving;
        protected Vector2 m_vTargetLocation;
        protected float m_fSpeed;
        protected float m_fWaitTime;
        protected float m_fIdleTime;

        protected float m_fMaxAge;
        protected AIWanderType m_eWanderType;

        public Sprite sheep,Sshadow;
        public GamePadState gamePadStatus;

        public NPCObject(Vertex position, Sprite sprite) : this(position.toVector(), sprite) { }

        public NPCObject(Vector2 position, Sprite sprite)
            : base()
        {
			this.sprite = sprite;
			this.position = position;

            m_bIsMoving = false;
            m_vTargetLocation = new Vector2();
            m_fSpeed = 50.0f;
            m_fWaitTime = 1.0f;
            m_fIdleTime = 0.0f;
		}

        public static NPCObject parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
			if(input == null || spriteSheets == null) { return null; }

			// get the object's position
			Variable position = Variable.parseFrom(input.ReadLine());
			if(position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the sprite's name
			Variable spriteName = Variable.parseFrom(input.ReadLine());
			if(spriteName == null || !spriteName.id.Equals("Sprite Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the name of the spritesheet in which the sprite is found
			Variable spriteSheetName = Variable.parseFrom(input.ReadLine());
			if(spriteSheetName == null || !spriteSheetName.id.Equals("SpriteSheet Name", StringComparison.OrdinalIgnoreCase)) { return null; }

			// get the object's sprite
			SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName.value);
			if(spriteSheet == null) { return null; }
			Sprite sprite = spriteSheet.getSprite(spriteName.value);
			if(sprite == null) { return null; }

			// parse the sprite's position
			String[] positionData = position.value.Split(',');
			if(positionData.Length != 2) { return null; }

			Vector2 newPosition;
			try {
				newPosition.X = Int32.Parse(positionData[0]);
				newPosition.Y = Int32.Parse(positionData[1]);
			}
			catch(Exception) { return null; }

			// create the object
            NPCObject newObject = new NPCObject(newPosition, sprite);
			newObject.updateInitialValues();

			return newObject;
		}

		public override bool checkCollision(GameObject o) {
			return base.checkCollision(o);
		}

		public override void update(GameTime gameTime) {

            sheepUpdate(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position = new Vector2(position.X - 20.0f*((float)gameTime.ElapsedGameTime.Milliseconds/1000.0f), position.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position = new Vector2(position.X + 20.0f * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f), position.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position = new Vector2(position.X , position.Y - 20.0f * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position = new Vector2(position.X, position.Y + 20.0f * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
            }

			base.update(gameTime);
		}

        public void FleeFromPoint(Vector2 fleePoint)
        {
            m_vTargetLocation = fleePoint - position;
            if (m_vTargetLocation.Length() <= 50.0f && m_vTargetLocation.Length() >= -50.0f)
            {
                m_vTargetLocation.Normalize();
                m_vTargetLocation *= 100.0f;
                m_vTargetLocation += position;
                m_fIdleTime = 0.0f;
                m_bIsMoving = true;
            }
        }

        public void sheepUpdate(GameTime gameTime)
        {
            if (!m_bIsMoving)
            {
                m_fIdleTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (m_fIdleTime >= m_fWaitTime)
                {
                    m_fIdleTime = 0.0f;
                    m_bIsMoving = true;
                    m_vTargetLocation.X = position.X + Firecracker.theRandom.Next(-30, 30);
                    m_vTargetLocation.Y = position.Y + Firecracker.theRandom.Next(-30, 30);
                }
            }
            else
            {
                Vector2 diffVec = m_vTargetLocation - position;
                if (diffVec.Length() <= 1.0f)
                {
                    m_bIsMoving = false;
                    m_fIdleTime = 0.0f;
                }
                else
                {
                    diffVec.Normalize();
                    diffVec *= m_fSpeed;
                    position = position + (diffVec * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f));
                }
            }
        }

		public override void draw(SpriteBatch spriteBatch) {


			base.draw(spriteBatch);
		}
    }
}
