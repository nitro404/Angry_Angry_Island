using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Firecracker_Engine
{

    public enum RotationDirection { Clockwise, CounterClockwise, None }

    public enum ScaleDirection { Smaller, Larger, None }

    public class GameObject : CBaseObject
    {

        protected Sprite m_sprite;

        protected Vector2 m_position;
        protected Vector2 m_offset;
        protected Vector2 m_velocity;
        protected float m_maximumVelocity;
        protected float m_acceleration;
        protected float m_maximumAcceleration;

        protected float m_rotation;
        protected float m_rotationSpeed;
        protected float m_maximumRotationSpeed;
        protected RotationDirection m_rotationDirection;

        protected Vector2 m_size;
        protected Vector2 m_scale;
        protected float m_scaleSpeed;

        protected ScaleDirection m_scaleDirection;

        public GameObject()
        {
            m_sprite = null;

            m_position = new Vector2(0, 0);
            m_offset = new Vector2(0, 0);
            m_velocity = new Vector2(0, 0);
            m_maximumVelocity = 1;
            m_acceleration = 0;
            m_maximumAcceleration = 1;

            m_rotation = 0;
            m_rotationSpeed = 0;
            m_maximumRotationSpeed = 1;
            m_rotationDirection = RotationDirection.None;

            m_size = new Vector2(0, 0);
            m_scale = new Vector2(1, 1);
            m_scaleSpeed = 0;
            m_scaleDirection = ScaleDirection.None;

        }

        public Sprite sprite
        {
            get { return m_sprite; }
            set { m_sprite = value; }
        }

        public string name
        {
            get { return (m_sprite == null) ? null : m_sprite.name; }
        }

        public int x
        {
            get { return (int)(m_position.X); }
        }

        public int y
        {
            get { return (int)(m_position.Y); }
        }

        public Vector2 position
        {
            get { return m_position; }
            set { if (value != null) { m_position = new Vector2(value.X, value.Y); } }
        }

        public Vector2 offset
        {
            get { return m_offset; }
        }

        public Vector2 velocity
        {
            get { return m_velocity; }
            set { if (value != null) { m_velocity = new Vector2(value.X, value.Y); } }
        }

        public float maximumVelocity
        {
            get { return m_maximumVelocity; }
            set { m_maximumVelocity = value; }
        }

        public float acceleration
        {
            get { return m_acceleration; }
            set { m_acceleration = value; }
        }

        public float maximumAcceleration
        {
            get { return m_maximumAcceleration; }
            set { m_maximumAcceleration = value; }
        }

        public float speed
        {
            get { return (float)Math.Sqrt(Math.Pow(m_velocity.X, 2) + Math.Pow(m_velocity.Y, 2)); }
        }

        public float rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public float rotationSpeed
        {
            get { return m_rotationSpeed; }
            set { m_rotationSpeed = value; }
        }

        public float maximumRotationSpeed
        {
            get { return m_maximumRotationSpeed; }
            set { m_maximumRotationSpeed = value; }
        }

        public RotationDirection rotationDirection
        {
            get { return m_rotationDirection; }
            set { m_rotationDirection = value; }
        }

        public Vector2 size
        {
            get { return m_size; }
            set { if (value.X >= 0 && value.Y >= 0) { m_size = value; } }
        }

        public Vector2 scale
        {
            get { return m_scale; }
            set { if (value != null) { m_scale = new Vector2(value.X, value.Y); } }
        }

        public float scaleSpeed
        {
            get { return m_scaleSpeed; }
            set { m_scaleSpeed = value; }
        }

        public ScaleDirection scaleDirection
        {
            get { return m_scaleDirection; }
            set { m_scaleDirection = value; }
        }

        // reset the object to its original state
        public virtual void RestoreInitialValues(ObjectDefinition objDef)
        {
            // TODO: Fix this
            /*m_position = new Vector2(m_initialPosition.X, m_initialPosition.Y);
            m_velocity = new Vector2(m_initialVelocity.X, m_initialVelocity.Y);
            m_acceleration = m_initialAcceleration;
            m_rotation = m_initialRotation;
            m_rotationSpeed = m_initialRotationSpeed;
            m_rotationDirection = m_initialRotationDirection;
            m_scale = new Vector2(m_initialScale.X, m_initialScale.Y);
            m_scaleSpeed = m_initialScaleSpeed;
            m_scaleDirection = m_initialScaleDirection;
            */
        }

        public override void Tick(GameTime gameTime) { }

        public virtual void handleInput(GameTime gameTime) { }

        public override void Render()
        {
            base.Render();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (m_sprite == null) { return; }

            m_sprite.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
        }

        public virtual bool checkCollision(GameObject o) { return false; }
    }

}
