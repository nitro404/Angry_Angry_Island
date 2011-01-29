using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Firecracker_Engine
{
    public static class TextureLoader
    {
        //this class simply loads textures for the UI elements, given an asset name.
        //If the asset has already been loaded, it will just return the already
        //loaded texture.

        public static Dictionary<string, Texture2D> textures = new Dictionary<string,Texture2D>();

        public static Texture2D LoadTexture(string imageID)
        {
            if (textures.ContainsKey(imageID))
            {
                return textures[imageID];
            }
            else
            {
                textures[imageID] = Firecracker.engineInstance.Content.Load<Texture2D>(@"UI\" + imageID);
                return textures[imageID];
            }
        }
    }
}
