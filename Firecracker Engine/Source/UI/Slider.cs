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
    public class Slider : UIObject
    {
        //this class represents a horizontal slider. The slider stores a value between 0 and 1.

        public bool clicked = false;
        public float value = 0.5f;

        public string ID;
        public Slider(Vector2 pos, Vector2 size, HAlign hAlign, VAlign vAlign, string ID)
            : base(pos, size, hAlign, vAlign, true, ContentType.Structural, "")
        {
            this.ID = ID;
            InnerElements.Add(new UIObject(Vector2.Zero, size - new Vector2(Style.SliderHandleSize.X, 0), HAlign.Center, VAlign.Center, false, ContentType.Image, Style.SliderBar));
            InnerElements.Add(new UIObject(Vector2.Zero, Style.SliderHandleSize, HAlign.Center, VAlign.Center, false, ContentType.Image, Style.SliderHandle));
            Init();
        }

        public override void OnMouseOver(MouseState mouseState, MouseState lastMouseState, bool isMouseOver)
        {
            if (isMouseOver)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    clicked = true;
                }
                InnerElements[0].param = Style.SliderBarMouseOver;
                InnerElements[1].param = Style.SliderHandleMouseOver;
            }
            else if (!clicked)
            {
                InnerElements[0].param = Style.SliderBar;
                InnerElements[1].param = Style.SliderHandle;
            }
            if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                clicked = false;
            }

            if (clicked)
            {
                value = (float)Math.Min(1, Math.Max(0, ((float)mouseState.X - InnerElements[0].topLeft.X) / InnerElements[0].scaledSize.X));
                InnerElements[1].pos.X = (value - 0.5f) * InnerElements[0].size.X;
                param = Style.SliderBarMouseOver;
                InnerElements[1].param = Style.SliderHandleMouseOver;
                DoAction();
            }
        }

        public override void Init()
        {
            //initialization of the value of the slider is currently hard-coded.
            //ideally it could be bound to a game setting with min/max values but this would
            //require an overhauld of the settings system.


            //find the value
           

            //put the handle in the right spot based on the value found.
            InnerElements[1].pos.X = (value - 0.5f) * InnerElements[0].size.X;
            base.Init();

            UICallBackInfo info = new UICallBackInfo();
            info.ID = ID;
            info.eventType = UIEventType.SliderInit;
            IDRegistrar.ExecuteCallBack(info);
        }

        public void DoAction()
        {
            //actions are hard-coded for now.
            //it would be better if sliders could simply be bound to a game setting with max/min values.
            UICallBackInfo info = new UICallBackInfo();
            info.ID = ID;
            info.eventType = UIEventType.SliderChangeValue;
            info.sliderValue = value;
            IDRegistrar.ExecuteCallBack(info);
        }
    }
}
