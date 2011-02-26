using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    public class SpiderFire : GameObject
    {
        private struct FireNode
        {
            public GameObject m_FireObject;
            public float m_fLifeLeft;
            public float m_fSpeed;
            public Vector2 m_vTargetLocation;
            public Vector2 m_vCurrentLocation;

            public int m_iNodesRemaining;
        }

        List<FireNode> m_lFireObjects;

        public SpiderFire(Vector2 thePos)
            : base()
        {
            m_lFireObjects = new List<FireNode>();
            m_position = thePos;

            SpriteSheet spriteSheet = Firecracker.spriteSheets.getSpriteSheet("ExplosionObject");
            if (spriteSheet != null)
            {
                Sprite sprite = spriteSheet.getSprite("Explosion 3");
                if (sprite != null)
                {
                    m_sprite = sprite;
                    for (int i = 0; i < 4; i++)
                    {
                        CreateNewNode(position,5);
                    }
                }
            }
        }
        private void CreateNewNode(Vector2 thePosition, int NewNodes)
        {
            if (NewNodes == 0) return;
            FireNode newNode = new FireNode();
            newNode.m_FireObject = new StaticObject(thePosition, m_sprite);
            newNode.m_fLifeLeft = (float)Firecracker.random.NextDouble();
            newNode.m_fSpeed = (float)Firecracker.random.NextDouble() * 100.0f;
            newNode.m_vTargetLocation.X = thePosition.X + Firecracker.theRandom.Next(-450, 450);
            newNode.m_vTargetLocation.Y = thePosition.Y + Firecracker.theRandom.Next(-450, 450);
            newNode.m_vCurrentLocation = thePosition;
            newNode.m_iNodesRemaining = NewNodes;
            m_lFireObjects.Add(newNode);
        }

        public override void update(GameTime gameTime)
        {
            for (int i = 0; i < m_lFireObjects.Count(); i++)
            {
                FireNode theNode = m_lFireObjects[i];
                Vector2 newPos = theNode.m_vTargetLocation - m_lFireObjects[i].m_vCurrentLocation;
                newPos.Normalize();
                newPos *= (float)(m_lFireObjects[i].m_fSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                theNode.m_vCurrentLocation = m_lFireObjects[i].m_vCurrentLocation + newPos;
                theNode.m_fLifeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                theNode.m_FireObject.position = theNode.m_vCurrentLocation;

                m_lFireObjects[i] = theNode;

                if (m_lFireObjects[i].m_fLifeLeft <= 0)
                {
                    CreateNewNode(m_lFireObjects[i].m_vCurrentLocation, m_lFireObjects[i].m_iNodesRemaining - 1);
                    CreateNewNode(m_lFireObjects[i].m_vCurrentLocation, m_lFireObjects[i].m_iNodesRemaining - 1);
                    m_lFireObjects.RemoveAt(i);
                    i--;
                    continue;
                }

        
            // check the collisions.
                for (int j = 0; j < Firecracker.level.numberOfObjects(); j++)
                {
                    GameObject theObj = Firecracker.level.objectAt(j);
                    if (theObj.GetType() == typeof(NPCObject))
                    {
                        if ((theObj.position - m_lFireObjects[i].m_vCurrentLocation).Length() < 10)
                        {
                            // kill this guy.
                            theObj.toBeDeleted = true;
                            Explosion newDeath = new Explosion(theObj.position);
                            HumanDeath ashDeath = new HumanDeath(theObj.position);
                            Firecracker.level.addObject(newDeath);
                            Firecracker.level.addObject(ashDeath);
                        }
                    }
                }
            }
            base.update(gameTime);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_lFireObjects.Count(); i++)
            {
                m_lFireObjects[i].m_FireObject.drawCentered(spriteBatch);
            }
            // we don't want to render the underlying object.
            //base.draw(spriteBatch);
        }
    }
}
