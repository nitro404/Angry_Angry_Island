using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
  public  class Ocean :GameObject
    {
        SpriteAnimation m_water;
        public float depth = 0.53f;

    public Ocean () 
        :base()  
    
    {
        m_water = Firecracker.animations.getAnimation("Ocean");
        for(int i =0 ; i < m_water.size() ;i++)
        {
            m_water.getSprite(i).m_SpriteDepth = depth;
        }
    }
    public override void update(GameTime gameTime)
    {
        m_water.update(gameTime);
    }
    public override void draw(SpriteBatch spriteBatch)
    {
        int waterOffsetX = -110;
        int waterOffsetY = -110;
        for (int x = 0; x < 80; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                m_water.draw(spriteBatch, m_scale, m_rotation, new Vector2(waterOffsetX,waterOffsetY), SpriteEffects.None);
                waterOffsetX += 32;
            }
            waterOffsetY += 16;
            waterOffsetX = -110;
        }
    }
    

   
    }
}
