using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using MatrixVector;

namespace Utility
{
    public static class MostInformativeJointsSelector
    {
        public static List<JointType> GetJoints(Dictionary<JointType, MatrixVector.Vector3> aEvaluatedJoints, int aFrames)
        {
			List<KeyValuePair<double, JointType>> overallResult = new List<KeyValuePair<double, JointType>>();

            foreach (var data in aEvaluatedJoints)
            {
                //Console.WriteLine(data.Key.ToString() + ": ");
                //Console.WriteLine("\tX:" + data.Value.X);
                //Console.WriteLine("\tY:" + data.Value.Y);
                //Console.WriteLine("\tZ:" + data.Value.Z);
                //Console.WriteLine("\tOverall: " + (data.Value.X + data.Value.Y + data.Value.Z));

				overallResult.Add(new KeyValuePair<double, JointType>(((data.Value.X + data.Value.Y + data.Value.Z)) / aFrames, data.Key));
            }

            overallResult.Sort(ResultComparer);

            //TODO: Think of a way to get a few joints from the sorted Joints list

            var toReturn = new List<JointType>();

            /*
            foreach (var joint in overallResult)
            {
                if (joint.Key / overallResult[overallResult.Count-1].Key > 10) //Relatively good working
                {
                    toReturn.Add(joint.Value); //JointType value
					//Console.WriteLine("Joint " + joint.Value +" " + joint.Key);
                }
            }
             */

            var array = overallResult.ToArray();

            for (int i = overallResult.Count-1; i >= overallResult.Count-7; --i)
            {
                toReturn.Add(array[i].Value);
            }
			
            Console.WriteLine();

            toReturn.Remove(JointType.WristLeft);
            toReturn.Remove(JointType.WristRight);
            try
            {
                toReturn.Remove(JointType.AnkleLeft);
                toReturn.Remove(JointType.AnkleRight);
            }
            catch { }

            return toReturn;
        }


		static int ResultComparer(KeyValuePair<double, JointType> a, KeyValuePair<double, JointType> b)
        {
            return a.Key.CompareTo(b.Key);
        }
    }
}
