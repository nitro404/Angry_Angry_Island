using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Firecracker_Engine
{
    public class PopulationManager
    {
        public enum Age
        {
            Primitive,
            Developing,
            Developed,
            Advanced
        }

        public Age age;
        public static PopulationManager Instance;
        int populationForDeveloping = 50;
        int populationForDeveloped = 150;
        int populationForAdvanced = 350;
        int populationToLose = 450;
        float TimeRequiredForDeveloping = 20;
        float TimeRequiredForDeveloped = 20;
        float TimeRequiredForAdvanced = 20;
        float TimeRequiredToLose = 30;
        public float TimeSpentAboveThreshold = 0;

        public PopulationManager()
        {
            Instance = this;
            Init();
        }

        public void Init()
        {
            age = Age.Primitive;
            TimeSpentAboveThreshold = 0;
        }

        public void Update(GameTime gameTime)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (age == Age.Primitive)
            {
                if (Firecracker.engineInstance.numPeoples >= populationForDeveloping)
                {
                    TimeSpentAboveThreshold += deltaT;
                    if (TimeSpentAboveThreshold > TimeRequiredForDeveloping)
                    {
                        age = Age.Developing;
                        TimeSpentAboveThreshold = 0;
                    }
                }
                else
                {
                    TimeSpentAboveThreshold = 0;
                }
            }
            else if (age == Age.Developing)
            {
                if (Firecracker.engineInstance.numPeoples >= populationForDeveloped)
                {
                    TimeSpentAboveThreshold += deltaT;
                    if (TimeSpentAboveThreshold > TimeRequiredForDeveloped)
                    {
                        age = Age.Developed;
                        TimeSpentAboveThreshold = 0;
                    }
                }
                else
                {
                    TimeSpentAboveThreshold = 0;
                }
            }
            else if (age == Age.Developed)
            {
                if (Firecracker.engineInstance.numPeoples >= populationForAdvanced)
                {
                    TimeSpentAboveThreshold += deltaT;
                    if (TimeSpentAboveThreshold > TimeRequiredForAdvanced)
                    {
                        age = Age.Advanced;
                        TimeSpentAboveThreshold = 0;
                    }
                }
                else
                {
                    TimeSpentAboveThreshold = 0;
                }
            }
            else if (age == Age.Advanced)
            {
                if (Firecracker.engineInstance.numPeoples >= populationToLose)
                {
                    TimeSpentAboveThreshold += deltaT;
                    if (TimeSpentAboveThreshold > TimeRequiredToLose)
                    {
                        PopupNotification.instance.ShowNotification("You lose."  ,"You get nothing.", true);
                    }
                }
                else
                {
                    TimeSpentAboveThreshold = 0;
                }
            }
            if (Firecracker.engineInstance.numPeoples <= 0)
            {
                PopupNotification.instance.ShowNotification("You win!", "Congrats!",true);
            }
        }
    }
}
