﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    public class Forest
    {
        private List<GameObject> m_lForestList;
        List<Sprite> m_lTreeSprites;

        private static int TOTAL_STARTING_TREES = 400;

        public Forest()
        {
            m_lForestList = new List<GameObject>();
            m_lTreeSprites = new List<Sprite>();
        }

        public void Initialize(SpriteSheetCollection spriteSheets)
        {
            SpriteSheet spriteSheet = spriteSheets.getSpriteSheet("Tree");
            if (spriteSheet == null) { return; }
            for (int i = 1; i < 9; i+= 1)
            {
                Sprite sprite = spriteSheet.getSprite(string.Concat("Tree ", i.ToString()));
                if (sprite == null) { return; }
                //chris edited this
                sprite.m_SpriteDepth = 0.49f;
                m_lTreeSprites.Add(sprite);
            }

            for (int i = 0; i < TOTAL_STARTING_TREES; i++)
            {
                Vector2 pos = new Vector2(Firecracker.random.Next(0, Firecracker.level.dimensions.X * Firecracker.level.gridSize), Firecracker.random.Next(0, Firecracker.level.dimensions.Y * Firecracker.level.gridSize));
                if (Terrain.Instance == null || Terrain.Instance.isPositionWalkable(pos))
                {
                    int RandomIndex = Firecracker.random.Next(0, 4);
                    StaticObject newTree = new StaticObject(pos, m_lTreeSprites[RandomIndex]);
                    if (newTree != null)
                    {
                        Firecracker.level.addObject(newTree);
                        m_lForestList.Add(newTree);
                    }
                }
            }

        }

        public void Tick(GameTime gameTime)
        {

        }

    }
}
