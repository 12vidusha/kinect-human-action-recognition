using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SkeletalTracking.Utility
{
    public static class MostInformativeJointsSelector
    {
        public static List<JointType> GetJoints(Dictionary<JointType, SkeletonPoint> aEvaluatedJoints, int aFrames)
        {
            List<KeyValuePair<float, JointType>> overallResult = new List<KeyValuePair<float, JointType>>();

            foreach (var data in aEvaluatedJoints)
            {
                //Console.WriteLine(data.Key.ToString() + ": ");
                //Console.WriteLine("\tX:" + data.Value.X);
                //Console.WriteLine("\tY:" + data.Value.Y);
                //Console.WriteLine("\tZ:" + data.Value.Z);
                //Console.WriteLine("\tOverall: " + (data.Value.X + data.Value.Y + data.Value.Z));

                overallResult.Add(new KeyValuePair<float, JointType>((float)(data.Value.X + data.Value.Y + data.Value.Z)/aFrames, data.Key));
            }

            overallResult.Sort(ResultComparer);

            //TODO: Think of a way to get a few joints from the sorted Joints list

            var toReturn = new List<JointType>();

            foreach (var joint in overallResult)
            {
                if (joint.Key != 0)
                {
                    toReturn.Add(joint.Value); //JointType value
                }
            }

            toReturn.Reverse();

            toReturn.Remove(JointType.WristLeft);
            toReturn.Remove(JointType.WristRight);


            return toReturn;
        }


        static int ResultComparer(KeyValuePair<float, JointType> a, KeyValuePair<float, JointType> b)
        {
            return a.Key.CompareTo(b.Key);
        }
    }
}
