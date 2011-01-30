using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Firecracker_Engine
{
    public static class Helpers
    {
        public static float ParseFloat(string inString)
        {
            return float.Parse(inString);
        }

        public static Vector2 ParseVector2(string inString)
        {
            Vector2 returnVec = new Vector2();
            
            string[] stringList;
            stringList = inString.Trim("{}".ToCharArray()).Split(", ".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);

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
            stringList = inString.Trim("{}".ToCharArray()).Split(", ".ToCharArray(), 3, StringSplitOptions.RemoveEmptyEntries);

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
            stringList = inString.Trim("{}".ToCharArray()).Split(", ".ToCharArray(), 4, StringSplitOptions.RemoveEmptyEntries);

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

        public static int StringToEnum<EnumType>(string sEnumVal)
        {
            return (int)Enum.Parse(typeof(EnumType), sEnumVal);
        }

        public static List<int[]> ParseIntX4Array(string sArrayString)
        {
            // assume that the string is a list of {1,1,1,1} entries

            List<int[]> theReturnList = new List<int[]>();
            int[] theListNums = new int[4];

            string[] theStringList = sArrayString.Split("},{".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < theStringList.Length; i++)
            {
                theListNums[i % 4] = int.Parse(theStringList[i]);
                if ((i+1) % 4 == 0)
                {
                    theReturnList.Add(theListNums);
                    theListNums = new int[4];
                }
            }
            return theReturnList;
        }

        /// <summary>
        /// Count occurrences of strings.
        /// </summary>
        public static int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        public static Point ParsePoint(string sPoint)
        {
            Point newPoint = new Point();
            string tempString = sPoint.Substring(1, sPoint.Length - 2);
            newPoint.X = int.Parse(tempString.Substring(0, tempString.IndexOf(",")));
            newPoint.Y = int.Parse(tempString.Substring(tempString.IndexOf(",")+1));
            return newPoint;
        }

        public static float GetAngle(Vector2 vector)
        {
            float radians = (float)Math.Atan2(vector.X, vector.Y);

            float degrees = MathHelper.ToDegrees(radians);

            return degrees;
        }
    }
}
