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
        int hotMins = 0;
        double kinkySecs = 0;
        double tribbles = 0;

        public InfoBar()
            : base(Vector2.Zero, new Vector2(BAR_WIDTH, BAR_HEIGHT), HAlign.Left, VAlign.Top, false, ContentType.Image, "testbutton")
        {
            InnerElements = new List<UIObject>();
            infoLabel = new Label(new Vector2(BAR_PADDING, 0), HAlign.Left, VAlign.Center, "Age: Dark Age       Llamas Licked: " + Firecracker.engineInstance.numPeoples.ToString()); //+ Firecracker.engineInstance.theCamera.GetCameraPos());
            InnerElements.Add(infoLabel);
        }

        public override void Update(float deltaT)
        {
            if (Firecracker.engineInstance.elapsedTime > 60)
            {
                kinkySecs = Firecracker.engineInstance.elapsedTime;
                kinkySecs = kinkySecs / 60;
                if (kinkySecs > 1)
                {
                    hotMins = (int)kinkySecs;
                    //kinkySecs -= 60;
                }
                tribbles = Firecracker.engineInstance.elapsedTime - (60 * hotMins);
            }
            else
                tribbles = Firecracker.engineInstance.elapsedTime;

            int difTimeMins = Firecracker.engineInstance.TimeRemaining(Firecracker.engineInstance.maxTime[0], hotMins);
            int difTimeSecs = Firecracker.engineInstance.TimeRemaining(Firecracker.engineInstance.maxTime[1], tribbles);

            infoLabel.SetText("Time: " + ((int)hotMins).ToString() //((int)(Firecracker.engineInstance.maxTime[0] - Firecracker.engineInstance.elapsedTime[0])).ToString()
                + ":" + ((int)tribbles).ToString() //((int)(Firecracker.engineInstance.maxTime[1] - Firecracker.engineInstance.elapsedTime[1])).ToString() 
                + " Survivors: " + Firecracker.engineInstance.numPeoples.ToString());
            
            base.Update(deltaT);
        }

        public override void Init()
        {

            base.Init();
        }
    }
}
