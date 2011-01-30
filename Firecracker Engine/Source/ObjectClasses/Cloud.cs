using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class Cloud :GameObject
    {
        public Sprite m_cloud;
        public float m_time;
       
        public Cloud(int number) :base()
        {
            m_cloud = Firecracker.spriteSheets.getSpriteSheet("Cloud").getSprite("Cloud "+number);
            m_cloud.m_SpriteDepth = 0.39f;
            spawn();
            
        }
        public void spawn ()
        {
            position = new Vector2(Firecracker.random.Next(0, Firecracker.level.dimensions.X * Firecracker.level.gridSize) + Firecracker.random.Next(-200, 280), Firecracker.random.Next(0, Firecracker.level.dimensions.Y * Firecracker.level.gridSize) + Firecracker.random.Next(-200, 280));
            m_time = 0;
            //scale = new Vector2(Firecracker.random.Next(0, 10) / 10.0f );
         //   m_scale = m_scale / 2;
        }
        public override void update(GameTime gameTime)
        {
            m_time += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            position = new Vector2(position.X +  16.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f, position.Y - 16.0f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (m_time > 20.0f)
            {
                    spawn();
                    
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
                m_cloud.draw(spriteBatch, m_scale, m_rotation, m_position, SpriteEffects.None);
           
        }

    }
}
