using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firecracker_Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace Firecracker_Engine
{
    class Terrain : GameObject
    {
        public static Terrain Instance;
        public const int WIDTH = 64;
        public const int HEIGHT = 64;
        public const int TILE_WIDTH = 32;
        public const int TILE_HEIGHT = 32;
        public int[,] tiles;
        public bool usingTiles = false;
        //0 = water
        //1 = sand
        //2 = grass
        //could add more later for different elevations.
        Sprite m_grassSprite;
        Sprite m_sandSprite;


        public Terrain()
        {
            Instance = this;
            Texture2D terrain = Firecracker.engineInstance.Content.Load<Texture2D>(@"Sprites/terrain");
            Color[] c = new Color[WIDTH*HEIGHT];
            terrain.GetData(c);
            
            tiles = new int[64,64];
            usingTiles = true;
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    Color color = c[x+y*HEIGHT];
                    if (color.R > 64)
                    {
                        tiles[x,y] = 2;
                    }
                    else if (color.R > 0)
                    {
                        tiles[x, y] = 1;
                    }
                    else
                    {
                        tiles[x, y] = 0;
                    }
                }
            }
            usingTiles = true;
            m_grassSprite = Firecracker.spriteSheets.getSpriteSheet("Grass").getSprite("Grass");
            m_grassSprite.m_SpriteDepth = 0.523f;
            m_sandSprite = Firecracker.spriteSheets.getSpriteSheet("Sand").getSprite("Sand");
            m_sandSprite.m_SpriteDepth = 0.526f;
        }

        public bool isPositionWalkable(Vector2 pos)
        {
            //TODO: isometric conversion
            int x = (int)(pos.X/TILE_WIDTH);
            int y = (int)(pos.Y/TILE_HEIGHT);
            if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT)
            {
                return (tiles[x, y] != 0);
            }
            else return false;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (tiles[x, y] == 1)
                    {
                        m_sandSprite.drawWithOffset(spriteBatch, Vector2.One, 0, new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT), SpriteEffects.None, new Vector2(16, 16));
                    }
                    else if (tiles[x, y] == 2)
                    {
                        m_grassSprite.drawWithOffset(spriteBatch, Vector2.One, 0, new Vector2(x * TILE_WIDTH, y * TILE_HEIGHT), SpriteEffects.None, new Vector2(16,16));
                    }
                }
            }

            base.draw(spriteBatch);
        }

        public static Terrain parseFrom(StreamReader input, SpriteSheetCollection spriteSheets)
        {
            // create the object
            Terrain newObject = new Terrain();
            newObject.updateInitialValues();

            return newObject;

        }
    }
}
