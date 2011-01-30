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

namespace Test_Game
{
    public class InfoBar : UIObject
    {
        const int BAR_HEIGHT = 40;
        const int BAR_WIDTH = 450;
        const int BAR_PADDING = 15;
        private Label infoLabel;

        public InfoBar()
            : base(Vector2.Zero, new Vector2(BAR_WIDTH, BAR_HEIGHT), HAlign.Left, VAlign.Top, false, ContentType.Image, "transparentgrey")
        {
            InnerElements = new List<UIObject>();
            infoLabel = new Label(new Vector2(BAR_PADDING, 0), HAlign.Left, VAlign.Center, "Age: Dark Age       Llamas Licked: " + Firecracker.engineInstance.numPeoples.ToString()); //+ Firecracker.engineInstance.theCamera.GetCameraPos());
            InnerElements.Add(infoLabel);
        }

        public override void Update(float deltaT)
        {
            infoLabel.SetText("Time: " + 
                Firecracker.engineInstance.makeTimeString(Firecracker.engineInstance.elapsedTime) + 
                " Survivors: " + Firecracker.engineInstance.numPeoples.ToString());
            
            base.Update(deltaT);
        }

        public override void Init()
        {

            base.Init();
        }
    }
}
