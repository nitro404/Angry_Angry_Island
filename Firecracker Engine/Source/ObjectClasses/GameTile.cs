using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine {

	public class GameTile : GameObject {

        protected Level m_GameLevel = null;

		//public GameTile(Vertex position, Sprite sprite) : this(position.toVector(), sprite) { }

		public GameTile(Vector2 position, Sprite sprite) : base() {
			m_position = position;
			this.sprite = sprite;
		}

        public override void OnBeginGameplay()
        {
            base.OnBeginGameplay();

            CBaseObject baseObj = GlobalFirecrackerRef.Instance.FindObjectByName("Level");

            if (baseObj != null)
            {
                if (baseObj.IsA("Level"))
                {
                    m_GameLevel = (Level)baseObj;
                }
            }

        }

		public override void Render(SpriteBatch spriteBatch) {
			if (m_sprite == null) { return; }

            if (m_GameLevel != null)
			    m_sprite.draw(spriteBatch, m_scale, m_rotation, m_GameLevel.getGamePosition(m_position), SpriteEffects.None);
		}

	}

}
