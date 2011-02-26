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
    public enum AIWanderType
    {
        AI_Random,
        AI_RandomTowardsNearest,
        AI_Path,
        AI_drawn,
        AI_Scatter,
        AI_Freeze,
        AI_None,
    }

    public class NPCObject : AnimatedGameObject
    {

        protected float m_fAge;
        protected float m_fDeathAt;
        protected bool m_bIsMoving;
        protected Vector2 m_vTargetLocation;
        protected float m_fSpeed;
        protected float m_fWaitTime;
        protected float m_fIdleTime;
        protected bool m_bKillable;
        public float eventTime, currentTime; // add for make the effect last a certain time
        public Vector2 PointOfTerror;
        public Variable AnimNameN;
        public Variable AnimNameS;
        public Variable AnimNameE;
        public Variable AnimNameW;
        public Sprite Ice;
        protected float m_fMaxAge;
        protected AIWanderType m_eWanderType;

        public GamePadState gamePadStatus;

        bool diedNaturally = false;

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
            m_eWanderType = AIWanderType.AI_Random;
            Firecracker.engineInstance.numPeoples++;
            m_fDeathAt = (float)Firecracker.random.NextDouble() * 5 + 5 + (100.0f * (Firecracker.engineInstance.numPeoples / 500.0f));
            m_bKillable = true;
           }

        public NPCObject(Vector2 position, NPCObject objRef)
            : base()
        {
            this.sprite = null;
            this.position = position;

            m_bIsMoving = false;
            m_vTargetLocation = new Vector2();
            m_fSpeed = 50.0f;
            m_fWaitTime = 1.0f;
            m_fIdleTime = 0.0f;

            Firecracker.engineInstance.numPeoples++;
            m_fDeathAt = Firecracker.random.Next(5, 6 + (int)(60.0f * (Firecracker.engineInstance.numPeoples / 500.0f)));
            Ice = Firecracker.spriteSheets.getSpriteSheet("Ice").getSprite("Ice");

            m_bKillable = true;

            this.AnimNameN = objRef.AnimNameN;
            this.AnimNameE = objRef.AnimNameE;
            this.AnimNameS = objRef.AnimNameS;
            this.AnimNameW = objRef.AnimNameW;

            SetIsDirectionBased(true);
            SetDirAnimation(AnimDirection.DIR_N, AnimNameN.value);
            SetDirAnimation(AnimDirection.DIR_E, AnimNameE.value);
            SetDirAnimation(AnimDirection.DIR_S, AnimNameS.value);
            SetDirAnimation(AnimDirection.DIR_W, AnimNameW.value);
        }

        public AIWanderType wanderType
        {
            get { return m_eWanderType; }
            set { m_eWanderType = value; }
        }

        public static NPCObject parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
            if (input == null || spriteSheets == null) { return null; }

            // get the object's position
            Variable position = Variable.parseFrom(input.ReadLine());
            if (position == null || !position.id.Equals("Position", StringComparison.OrdinalIgnoreCase)) { return null; }

            // parse the sprite's position
            String[] positionData = position.value.Split(',');
            if (positionData.Length != 2) { return null; }

            Vector2 newPosition;
            try
            {
                newPosition.X = Int32.Parse(positionData[0]);
                newPosition.Y = Int32.Parse(positionData[1]);
            }
            catch (Exception) { return null; }


            // Get the layer depth of this sprite
            Variable layerDepth = Variable.parseFrom(input.ReadLine());
            if (layerDepth == null || !layerDepth.id.Equals("LayerDepth", StringComparison.OrdinalIgnoreCase)) { return null; }


            // get the name of the spritesheet in which the sprite is found
            Variable spriteSheetName = Variable.parseFrom(input.ReadLine());
            if (spriteSheetName == null || !spriteSheetName.id.Equals("SpriteSheet Name", StringComparison.OrdinalIgnoreCase)) { return null; }


            // create the object
            NPCObject newObject = new NPCObject(newPosition, (Sprite)null);

            // get the sprite's name
            newObject.AnimNameN = Variable.parseFrom(input.ReadLine());
            if (newObject.AnimNameN == null || !newObject.AnimNameN.id.Equals("Anim Name N", StringComparison.OrdinalIgnoreCase)) { return null; }
            newObject.AnimNameE = Variable.parseFrom(input.ReadLine());
            if (newObject.AnimNameE == null || !newObject.AnimNameE.id.Equals("Anim Name E", StringComparison.OrdinalIgnoreCase)) { return null; }
            newObject.AnimNameS = Variable.parseFrom(input.ReadLine());
            if (newObject.AnimNameS == null || !newObject.AnimNameS.id.Equals("Anim Name S", StringComparison.OrdinalIgnoreCase)) { return null; }
            newObject.AnimNameW = Variable.parseFrom(input.ReadLine());
            if (newObject.AnimNameW == null || !newObject.AnimNameW.id.Equals("Anim Name W", StringComparison.OrdinalIgnoreCase)) { return null; }

            Variable isKillable = Variable.parseFrom(input.ReadLine());
            if (isKillable == null || !isKillable.id.Equals("IsKillable", StringComparison.OrdinalIgnoreCase)) { return null; }


            // get the object's sprite
            SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName.value);
            if (spriteSheet == null) { return null; }


            newObject.m_bKillable = bool.Parse(isKillable.value);
            //newObject.sprite.m_SpriteDepth = float.Parse(layerDepth.value);
            newObject.SetIsDirectionBased(true);
            newObject.SetDirAnimation(AnimDirection.DIR_N, newObject.AnimNameN.value);
            newObject.SetDirAnimation(AnimDirection.DIR_E, newObject.AnimNameE.value);
            newObject.SetDirAnimation(AnimDirection.DIR_S, newObject.AnimNameS.value);
            newObject.SetDirAnimation(AnimDirection.DIR_W, newObject.AnimNameW.value);
            newObject.updateInitialValues();

            return newObject;
        }

        public override bool checkCollision(GameObject o)
        {
            float distance = (float)Math.Sqrt(Math.Pow(o.position.X - position.X, 2) + Math.Pow(o.position.Y - position.Y, 2));
            return distance < (o.offset.X * o.scale.X) + (m_offset.X * m_scale.X);

        }

        public override void update(GameTime gameTime)
        {

            personality(gameTime);
                if (m_bKillable)
                    m_fAge += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (m_fAge >= m_fDeathAt)
                {
                    diedNaturally = true;
                    //Explosion exp = new Explosion(position + new Vector2(16.0f, 16.0f));
                    //Firecracker.level.addObject(exp);
                    //Explosion exp2 = new Explosion(position - new Vector2(16.0f, 16.0f));
                    //Firecracker.level.addObject(exp2);

                    if (Firecracker.engineInstance.numPeoples <= 500)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 spawnPosition = position;
                            for (int j = 0; j < 5; j++)
                            {
                                //try 5 times to find a legal spawn position
                                spawnPosition = position + new Vector2((float)Firecracker.random.NextDouble() * 32 - 16, (float)Firecracker.random.NextDouble() * 32 - 16);
                                if (Terrain.Instance == null || Terrain.Instance.isPositionWalkable(spawnPosition))
                                {
                                    break;
                                }
                                spawnPosition = position;
                            }

                            NPCObject newObject = new NPCObject(spawnPosition, this);
                            Firecracker.level.addObject(newObject);
                        }
         
                    HumanDeath newDeath = new HumanDeath(position);
                    Firecracker.level.addObject(newDeath);

                    toBeDeleted = true;
                }
            }

            // until we can see the cursor, don't worry about this.
            //if (Firecracker.engineInstance.m_MouseManager.IsMouseLeftPressed())
            //{
            //    FleeFromPoint(Firecracker.engineInstance.m_MouseManager.GetMousePos());
            //}

            base.update(gameTime);
        }

        public void FleeFromPoint(Vector2 fleePoint) //everyone flee from this point
        {
            m_vTargetLocation = Firecracker.level.getGamePosition(position) - Firecracker.level.getGamePosition(fleePoint);
            if (m_vTargetLocation.Length() <= 180.0f && m_vTargetLocation.Length() >= -180.0f)
            {
                m_vTargetLocation.Normalize();
                m_vTargetLocation *= 100.0f;
            }
        }
        public void SwarmPoint(Vector2 fleePoint)//everyone come to this point
        {
            m_vTargetLocation = Firecracker.level.getGamePosition(fleePoint) - Firecracker.level.getGamePosition(position);
            if (m_vTargetLocation.Length() <= 180.0f && m_vTargetLocation.Length() >= -180.0f)
            {
                m_vTargetLocation.Normalize();
                m_vTargetLocation *= 70.0f;
            }
        }
        public void personality(GameTime gameTime) //this dictats how people act
        {

            if (m_eWanderType == AIWanderType.AI_Scatter)
            {
                m_vTargetLocation += position;
                m_fIdleTime = 0.0f;
                m_bIsMoving = true;
            }
            else if (m_eWanderType == AIWanderType.AI_Random)
            {
                BasicMove(gameTime);
            }
            else if (m_eWanderType == AIWanderType.AI_drawn)
            {
                m_vTargetLocation += position;
                m_fIdleTime = 0.0f;
                m_bIsMoving = true;
            }
            else if (m_eWanderType == AIWanderType.AI_Freeze)
            {
                m_bIsMoving = true;
                this.m_fAge = 0; // dont add to the age therefore they dont die
            }
            else if (m_eWanderType == AIWanderType.AI_None)
            {

            }
            //if there is an event case it time is set by the ack and it keeps going until the event is over 
            if (eventTime < currentTime)
            {
                m_eWanderType = AIWanderType.AI_Random;
            }
            currentTime += 0.05f;
        }
        /*
        public void Repel(Vector2 value) //from landMines
        {
            float distance = (value - Position).Length();
            Vector2 direction = (value - Position) * -1;
            direction.Normalize();
            float difference = (value - goal).Length();
            Vector2 newLoc = (direction * (200.0f - distance) * 4.0f) + Position;
            if (difference > 10)
            {
                goal = new Vector2(ai.Next((int)(-distance / 2.0f), (int)(distance / 2.0f)) + newLoc.X, ai.Next((int)(-distance / 2.0f), (int)(distance / 2.0f)) + newLoc.Y);
                interest = 100;
            }
        }
         */



        public void BasicMove(GameTime gameTime)
        {
            if (!m_bIsMoving)
            {
                m_fIdleTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                if (m_fIdleTime >= m_fWaitTime)
                {
                    m_fIdleTime = 0.0f;
                    m_bIsMoving = true;

                    Vector2 targetpos = position;
                    for (int j = 0; j < 5; j++)
                    {
                        //try 5 times to find a legal position
                        targetpos = position + new Vector2((float)Firecracker.random.NextDouble() * 60 - 30, (float)Firecracker.random.NextDouble() * 60 - 30);
                        if (Terrain.Instance == null || Terrain.Instance.isPositionWalkable(targetpos))
                        {
                            break;
                        }
                        targetpos = position;
                    }
                    m_vTargetLocation = targetpos;
                    float vecAngle = Helpers.GetAngle(targetpos - position);
                    vecAngle += 45;
                    if (vecAngle < 0) vecAngle += 360;
                    if (vecAngle > 360) vecAngle -= 360;
                    AnimDirection theDir = (AnimDirection)(((int)vecAngle) / 90);
                    // Le Cheat Hack!
                    if (theDir == AnimDirection.DIR_N)
                        theDir = AnimDirection.DIR_S;
                    else if (theDir == AnimDirection.DIR_S)
                        theDir = AnimDirection.DIR_N;
                    playAnimation(theDir);
                }
            }
            else
            {
                Vector2 diffVec = m_vTargetLocation - position;
                if (diffVec.Length() <= 1.0f)
                {
                    m_bIsMoving = false;
                    m_fIdleTime = 0.0f;
                    PauseAnimation();
                }
                else
                {
                    diffVec.Normalize();
                    diffVec *= m_fSpeed;
                    Vector2 newPosition = position + Firecracker.level.getScreenPosition((diffVec * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f)));
                    if (Terrain.Instance == null || Terrain.Instance.isPositionWalkable(newPosition))
                    {
                        position = newPosition;
                    }
                    else
                    {
                        m_vTargetLocation = position;
                    }
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
			if(m_eWanderType == AIWanderType.AI_Freeze) {
				base.drawCentered(spriteBatch, Color.CornflowerBlue);
				Ice.drawCentered(spriteBatch, m_scale, m_rotation, position, SpriteEffects.None);
			}
			else {
				base.drawCentered(spriteBatch);
			}
        }

        public override void OnDestroyed()
        {
            Firecracker.engineInstance.numPeoples--;
            if (!diedNaturally)
            {
                Player.Instance.Credits += 3;
            }
            base.OnDestroyed();
        }
    }
}

