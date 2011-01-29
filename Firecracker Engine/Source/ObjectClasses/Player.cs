using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using System.IO;

namespace Firecracker_Engine
{
    public class Player : GameObject
    {

        private const float CREDIT_TRICKLE_RATE = 0.25f;
        private float m_credits;
        //private Ability m_selectedAbility;
        //private List<Ability> m_abilities;
        public static Player Instance;

        public Player()
            : base()
        {
		}
        public Player(Sprite theSprite)
            : base()
        {
            m_sprite = theSprite;
        }

        public void DoSelectedAbility(Vector2 position) //use an ability where the user clicked
        {
            /*if (m_selectedAbility.Cost < m_credits)
            {
                m_credits -= m_selectedAbility.Cost;
                m_selectedAbility.DoEffects(position);
            }*/
            Lightning newLightning = new Lightning(position);
            Firecracker.level.addObject(newLightning);
        }

        public override void update(GameTime gameTime)
        {

            position = Firecracker.engineInstance.m_MouseManager.GetMousePos();

            if (Firecracker.engineInstance.m_MouseManager.IsMouseLeftPressed())
            {
                DoSelectedAbility(Firecracker.engineInstance.m_MouseManager.GetMousePos());
            }

            m_credits += (float)((float)gameTime.ElapsedGameTime.Milliseconds/1000.0f) * CREDIT_TRICKLE_RATE;
            base.update(gameTime);
        }

        public static Player parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
            if (input == null || spriteSheets == null) { return null; }

            // Get the layer depth of this sprite
            Variable layerDepth = Variable.parseFrom(input.ReadLine());
            if (layerDepth == null || !layerDepth.id.Equals("LayerDepth", StringComparison.OrdinalIgnoreCase)) { return null; }

            // get the sprite's name
            Variable spriteName = Variable.parseFrom(input.ReadLine());
            if (spriteName == null || !spriteName.id.Equals("Sprite Name", StringComparison.OrdinalIgnoreCase)) { return null; }

            // get the name of the spritesheet in which the sprite is found
            Variable spriteSheetName = Variable.parseFrom(input.ReadLine());
            if (spriteSheetName == null || !spriteSheetName.id.Equals("SpriteSheet Name", StringComparison.OrdinalIgnoreCase)) { return null; }

            // get the object's sprite
            SpriteSheet spriteSheet = spriteSheets.getSpriteSheet(spriteSheetName.value);
            if (spriteSheet == null) { return null; }
            Sprite sprite = spriteSheet.getSprite(spriteName.value);
            if (sprite == null) { return null; }

            // create the object
            Player newObject = new Player(sprite);
            newObject.sprite.m_SpriteDepth = float.Parse(layerDepth.value);
            newObject.updateInitialValues();

            return newObject;

        }

        public float Credits
        {
            get { return m_credits; }
            set { m_credits = value; }
        }

        public override void OnDestroyed()
        {

            base.OnDestroyed();
        }
    }
}
