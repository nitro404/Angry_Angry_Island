using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Firecracker_Engine
{
    //a screen stores all of the UI objects that
    //will be displayed on the current screen,
    //and handles calling draw and update on them.
    public class Screen
    {
        //all coordinates are given with an assumption
        //of this height. During draw, they are scaled
        //appropriately according to the actual screen height.
        List<UIObject> elements;

        //screens are initialized with a list of
        //contained elements so that all of the 
        //UI can be defined using only constructor
        //parameters.
        public Screen(List<UIObject> elements) { this.elements = elements; }

        public void TestMouse(MouseState mouseState, MouseState lastMouseState)
        {
            foreach (UIObject element in elements)
            {
                element.TestMouse(mouseState, lastMouseState);
            }
        }

        //calculates how much to scale the UI elements and calls
        //draw on all of the elements, giving the entire screen size
        //as the containing area. (Inner UI elements can have smaller containing areas)
        public void Draw(GraphicsDevice g, SpriteBatch sb)
        {
            sb.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            float scale = ((float)g.PresentationParameters.BackBufferHeight) /Style.BaseHeight;
            foreach (UIObject element in elements)
            {
                element.Draw(sb, Vector2.Zero, new Vector2(g.PresentationParameters.BackBufferWidth, g.PresentationParameters.BackBufferHeight), scale, 1);
            }
            sb.End();
        }

        //called each frame. Updates all UI elements.
        public void Update(float deltaT)
        {
            foreach (UIObject element in elements)
            {
                element.Update(deltaT);
            }
        }

        //initializes all elements. 
        public void Init()
        {
            foreach (UIObject element in elements)
            {
                element.Init();
            }
        }

    }
}
