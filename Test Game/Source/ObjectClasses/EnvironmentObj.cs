using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
//remove later
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//

namespace Angry_Angry_Island
{
    class CEnvironmentObj :CBaseObject
    {

#pragma warning disable 108
        public const string ClassName = "Environment";
#pragma warning restore 108
        //remove later
        public Sprite s;
        public Sprite forest;
        //public Sprite sheep,Sshadow;
        //public int xPosition,yPosition; 
        //
        protected int m_Sold;
        protected float m_fMaxAge;
        protected int m_danger;
        protected String file;


        public CEnvironmentObj()
            : base()
        {
             s = new Sprite("Sprites/first_island_test_rock", AngryAngryIsland.gameInstance.Content);
			 forest = new Sprite("Sprites/first_island_test_trees", AngryAngryIsland.gameInstance.Content);
             //sheep = new Sprite( new Sprite("Sprites/sheep_sheet01", TestGame.GameInstance.Content),new Rectangle(0,102, 34,34));
             //Sshadow = new Sprite("Sprites/sheep_shadow01", TestGame.GameInstance.Content); 
             //xPosition = 0;
             //yPosition = 0;
        }
        public override bool IsA(string ObjectType)
        {
            if (ObjectType.Equals(ObjectType))
            {
                return true;
            }
            return base.IsA(ObjectType);
        }
        public override void Tick(GameTime gameTime)
        {
            //remove later
            base.Tick(gameTime);
        }
        
        public override void LoadPropertiesList(ObjectDefinition objDef)
        {
           base.LoadPropertiesList(objDef);
            //doesnt load anymore
            if (objDef.ClassProperties.ContainsKey("MaxAge"))
            {
                m_fMaxAge = float.Parse(objDef.ClassProperties["MaxAge"]);
            }
            if (objDef.ClassProperties.ContainsKey("Sold"))
            {
                m_Sold = int.Parse(objDef.ClassProperties["Sold"]);
            }
            if (objDef.ClassProperties.ContainsKey("Danger"))
            {
                m_danger = int.Parse(objDef.ClassProperties["Danger"]);
            }
            if (objDef.ClassProperties.ContainsKey("Sprite"))
            {
                file = objDef.ClassProperties["Sprite"];

            }
        }

     public override void LoadResources()
        {
         
            base.LoadResources();
        }
     public override void Render()
     {
		 AngryAngryIsland.gameInstance.spriteBatch.Begin();
		 s.drawCentered(AngryAngryIsland.gameInstance.spriteBatch, Vector2.One, 0.0f, new Vector2(s.xOffset, s.yOffset), SpriteEffects.None);
		 forest.drawCentered(AngryAngryIsland.gameInstance.spriteBatch, Vector2.One, 0.0f, new Vector2(s.xOffset, s.yOffset), SpriteEffects.None);
         //Sshadow.draw(TestGame.GameInstance.spriteBatch, new Vector2(1.5f), 0.0f, new Vector2(xPosition + s.xOffset, yPosition + s.yOffset), SpriteEffects.None);
         //sheep.draw(TestGame.GameInstance.spriteBatch, Vector2.One, 0.0f, new Vector2(xPosition + s.xOffset +2,yPosition +s.yOffset +2), SpriteEffects.None);
         //TestGame.GameInstance.spriteBatch.Draw();
		 AngryAngryIsland.gameInstance.spriteBatch.End();

         base.Render();
     }


    }
}

