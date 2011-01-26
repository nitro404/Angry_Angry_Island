using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    static class Helpers
    {
        public static Vector2 ParseVector2(string inString)
        {
            Vector2 returnVec = new Vector2();
            
            string[] stringList;
            stringList = inString.Split(" ".ToCharArray(), 2);

            if (stringList.Length >= 2)
            {
                returnVec.X = float.Parse(stringList[0]);
                returnVec.Y = float.Parse(stringList[1]);
            }
            
            return returnVec;
        }
        public static Vector3 ParseVector3(string inString)
        {
            Vector3 returnVec = new Vector3();

            string[] stringList;
            stringList = inString.Split(" ".ToCharArray(), 3);

            if (stringList.Length >= 3)
            {
                returnVec.X = float.Parse(stringList[0]);
                returnVec.Y = float.Parse(stringList[1]);
                returnVec.Z = float.Parse(stringList[2]);
            }

            return returnVec;
        }
        public static Vector4 ParseVector4(string inString)
        {
            Vector4 returnVec = new Vector4();

            string[] stringList;
            stringList = inString.Split(" ".ToCharArray(), 4);

            if (inString.Length >= 4)
            {
                returnVec.X = float.Parse(stringList[0]);
                returnVec.Y = float.Parse(stringList[1]);
                returnVec.Z = float.Parse(stringList[2]);
                returnVec.W = float.Parse(stringList[3]);
            }

            return returnVec;
        }

        public static string Vecto2ToSaveString(Vector2 inVector)
        {
            object[] formatList = new object[2];
            formatList[0] = inVector.X;
            formatList[1] = inVector.Y;
            return string.Format("%f %f", formatList);
        }
        public static string Vecto3ToSaveString(Vector3 inVector)
        {
            object[] formatList = new object[3];
            formatList[0] = inVector.X;
            formatList[1] = inVector.Y;
            formatList[2] = inVector.Z;
            return string.Format("%f %f %f", formatList);
        }
        public static string Vecto4ToSaveString(Vector4 inVector)
        {
            object[] formatList = new object[4];
            formatList[0] = inVector.X;
            formatList[1] = inVector.Y;
            formatList[2] = inVector.Z;
            formatList[3] = inVector.W;
            return string.Format("%f %f %f %f", formatList);
        }
    }
}
