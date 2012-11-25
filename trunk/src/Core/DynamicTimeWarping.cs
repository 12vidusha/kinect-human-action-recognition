using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Utility;
using Microsoft.Kinect;

namespace Core
{
	public static class DynamicTimeWarping
	{
		public static double CompareActivities(ActivityRecord record, ActivityWindow window){
			double similarity = 0.0f;
			double[,] DTW = new double[record.Frames.Count, window.Frames.Count];

			for (int i = 1; i < window.Frames.Count; i++)
			{
				DTW[0, i] = double.PositiveInfinity;
			}

			for (int i = 1; i < record.Frames.Count; i++)
			{
				DTW[i, 0] = double.PositiveInfinity;
			}

			DTW[0, 0] = 0;

			for (int i = 1; i < record.Frames.Count; i++)
			{
				for (int j = 1; j < window.Frames.Count; j++)
				{
                    similarity = SkeletonComparer.CompareWithSMIJ(record.Frames[i], window.Frames[j], record.MostInformativeJoints);

					DTW[i, j] = similarity + 
						
						Math.Min(
							Math.Min(
								DTW[i - 1, j], DTW[i, j - 1]
							), 
							DTW[i - 1, j - 1]
						);
				}
			}

			return DTW[record.Frames.Count-1, window.Frames.Count-1]/record.MostInformativeJoints.Count;
		}
	}
}
