#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace Firecracker_Engine
{

    //this is the parent of all other UI Objects.
    //every UI element on the screen is either
    //a UIObject or a child of UIObject.

    //the base UIObject contains functionality
    //to display an image, text, or just be an
    //invisible structural element.
    public class UIObject
    {
        
        public bool hidden = false;

        public enum ContentType
        {
            Image,
            Text,
            Structural
        }

        //horizontal alignment within this
        //object's containing area.
        //(Screen size if it's a top level element,
        // or parent's area otherwise)
        public enum HAlign
        {
            Left,
            Center,
            Right,
            Fill
        }

        //vertical alignment within this object's
        //containing area.
        public enum VAlign
        {
            Top,
            Center,
            Bottom,
            Fill
        }

        //position is relative to the space containing this element
        // (the whole screen if it is top-level, or its parent size
        // otherwise)
        //examples: an element with 10 pixels of padding on its left
        //          would have position (10, Y) and alignment left.
        //          an element with 10 pixels of padding on its right
        //          would have position (-10, Y) and alignment right.
        //          a centered element would have position (0, Y), and
        //          alignment Center.
        //          vertical position and alignment works the same way.

        public Vector2 pos;
        public Vector2 size;
        protected bool TriggerMouseEvent;

        //InnerElements contains child UIObjects, which will also be rendered
        //and updated (assuming this element isn't hidden).
        public List<UIObject> InnerElements;
        
        //param provides the text to display for text
        //content types, or the asset name of the texture for
        //image content types.
        public string param;
        public ContentType contentType;
        public HAlign hAlign;
        public VAlign vAlign;
        public UIObject parent;
        public Vector2 topLeft;

        //scaled size is set during draw,
        //when we have the graphics device. It does
        //not need to be specified.
        public Vector2 scaledSize;

        //color to set the text to or multiply the texture with
        public Color color = Color.White;

        //font for text objects
        public SpriteFont font = Style.font;

        //opacity carries on to child elements
        public float opacity = 1;

        public float rotation = 0;
        public Vector2 rotationorigin = Vector2.Zero;

        //NOTE: I know a lot of these variables shouldn't be public. I got lazy.

        //contructor for UIObjects without child elements
        public UIObject(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, bool TriggerMouseEvent, ContentType contentType, string param)
        {
            this.pos = pos;
            this.size = size;
            this.TriggerMouseEvent = TriggerMouseEvent;
            this.contentType = contentType;
            this.param = param;
            this.InnerElements = new List<UIObject>();
            this.hAlign = hAlign;
            this.vAlign = vAlign;
            topLeft = pos;
            scaledSize = pos;
        }

        //constructor for UIObjects with child elements
        public UIObject(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, bool TriggerMouseEvent, ContentType contentType, string param, List<UIObject> InnerElements)
        {
            this.pos = pos;
            this.size = size;
            this.TriggerMouseEvent = TriggerMouseEvent;
            this.contentType = contentType;
            this.param = param;
            this.InnerElements = InnerElements;
            this.hAlign = hAlign;
            this.vAlign = vAlign;
            topLeft = pos;
            scaledSize = pos;
            
            foreach (UIObject element in InnerElements)
            {
                element.parent = this;
            }
        }

        //figures out where the element should be drawn (scaling, alignent),
        //draws the element, and then updates these areas so that the mouse 
        //can be tested correctly later. Also calls draw on children.
        public virtual void Draw( SpriteBatch sb, Vector2 origin, Vector2 containerSize, float scale, float cumulativeOpacity)
        {
            if (!hidden)
            {
                //calculate scaled size and position
                //this is stored so it can be used when
                //testing the mouse.
                scaledSize = size * scale;
                topLeft = pos * scale + origin;
                if (hAlign == HAlign.Center)
                {
                    topLeft.X += (containerSize.X - scaledSize.X) / 2;
                }
                else if (hAlign == HAlign.Right)
                {
                    topLeft.X += containerSize.X - scaledSize.X;
                }
                else if (hAlign == HAlign.Fill)
                {
                    scaledSize.X = containerSize.X - (topLeft.X - origin.X) - scaledSize.X;
                }

                if (vAlign == VAlign.Center)
                {
                    topLeft.Y += (containerSize.Y - scaledSize.Y) / 2;
                }
                else if (vAlign == VAlign.Bottom)
                {
                    topLeft.Y += containerSize.Y - scaledSize.Y;
                }
                else if (vAlign == VAlign.Fill)
                {
                    scaledSize.Y = containerSize.Y - (topLeft.Y - origin.Y) - scaledSize.Y;
                }

                //multiply parent's opacity with our own. this will
                //carry down to children.
                cumulativeOpacity *= opacity;

                //draw
                if (contentType != ContentType.Structural)
                {
                    Color finalcolor = color;
                    finalcolor.A = (byte)(color.A*cumulativeOpacity);
                    if (contentType == ContentType.Text)
                    {
                        Color colorToUse = color;
                        sb.DrawString(font, param, topLeft, finalcolor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    }
                    else if (contentType == ContentType.Image)
                    {
                        //with each draw, we look up the texture based on the string ID.
                        //this seems inefficient, but is actually negligeable and allows
                        //for subclasses to change the param string to switch textures.
                        if (rotation == 0)
                        {
                            sb.Draw(TextureLoader.LoadTexture(param), new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)Math.Ceiling(scaledSize.X), (int)Math.Ceiling(scaledSize.Y)), finalcolor);
                        }
                        else
                        {
                            sb.Draw(TextureLoader.LoadTexture(param), new Rectangle((int)(topLeft.X+scaledSize.X/2), (int)(topLeft.Y+scaledSize.Y/2), (int)Math.Ceiling(scaledSize.X), (int)Math.Ceiling(scaledSize.Y)), null, finalcolor, rotation, size / 2, SpriteEffects.None, 0);
                        }
                    }
                }

                //call draw on all children
                for (int i = 0; i < InnerElements.Count; i++ )
                {
                    InnerElements[i].Draw(sb, topLeft, scaledSize, scale, cumulativeOpacity);
                }
            }
        }

        public virtual void Init()
        {
            foreach (UIObject element in InnerElements)
            {
                element.Init();
            }
        }

        //calculates if the mouse is over the object, and triggers mouse events.
        public void TestMouse(MouseState mouseState, MouseState lastMouseState)
        {
            if (!hidden)
            {
                //trigger mouse event if appropriate.
                if (TriggerMouseEvent)
                {
                    if (mouseState.X >= topLeft.X && mouseState.X <= topLeft.X + scaledSize.X
                        && mouseState.Y >= topLeft.Y && mouseState.Y <= topLeft.Y + scaledSize.Y)
                    {
                        OnMouseOver(mouseState, lastMouseState, true);
                    }
                    else
                    {
                        OnMouseOver(mouseState, lastMouseState, false);
                    }
                }

                //recurse on children.
                foreach (UIObject element in InnerElements)
                {
                    element.TestMouse(mouseState, lastMouseState);
                }
            }
        }
        
        //this should run every frame. Things like changing text or textures of this element
        //or inner elements can be done here.
        public virtual void Update(float deltaT)
        {

        }

        //this should run every frame, if this object triggers mouse events.
        //even if the mouse is not over the object, this will still be called,
        //just with isMouseOver set to false.
        public virtual void OnMouseOver(MouseState mouseState, MouseState lastMouseState, bool isMouseOver)
        {
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
    }
}
