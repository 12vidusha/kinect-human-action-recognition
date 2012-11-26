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

		public static List<JointType> GetJoints(List<ImportedSkeleton> aSkeletonCollection, int aFrames, QuaternionsStyles savingStyle)
		{
			Dictionary<JointType, double> overallResult = InitiazlizeDictionary();

			for (int i = 1; i < aSkeletonCollection.Count; i++)
			{
				var firstSkel = aSkeletonCollection[i - 1];
				var secondSkel = aSkeletonCollection[i];

				foreach (var joint in Enum.GetNames(typeof(JointType)))
				{
					var jointType = (JointType)Enum.Parse(typeof(JointType), joint);

					double jointEvaluation = 0;

					try
					{
						if (savingStyle == QuaternionsStyles.Absolute)
						{
							jointEvaluation = SkeletonComparer.CompareQuaternions(
								firstSkel.AbsoluteQuaternions[jointType], secondSkel.AbsoluteQuaternions[jointType]);
						}
						else
						{
							jointEvaluation = SkeletonComparer.CompareQuaternions(
								firstSkel.HiararchicalQuaternions[jointType], secondSkel.HiararchicalQuaternions[jointType]);
						}
					}
					catch (Exception e) { }

					overallResult[jointType] += jointEvaluation / aFrames;
				}
			}

			return SortAndPrepeareToReturn(overallResult.ToList());
		}


		private static Dictionary<JointType, double> InitiazlizeDictionary()
		{
			Dictionary<JointType, double> overallResult = new Dictionary<JointType, double>();
			foreach (var joint in Enum.GetNames(typeof(JointType)))
			{
				overallResult.Add((JointType)Enum.Parse(typeof(JointType), joint), 0.0);
			}
			return overallResult;
		}

		private static List<JointType> SortAndPrepeareToReturn(List<KeyValuePair<JointType, double>> overallResultList)
		{
			overallResultList.Sort(ResultComparer);

			var toReturn = new List<JointType>();
			var array = overallResultList.ToArray();
			for (int i = overallResultList.Count - 1; i >= overallResultList.Count - 7; --i)
			{
				if (array[i].Value != 0)
				{
					toReturn.Add(array[i].Key);
				}
			}

			try
			{
				toReturn.Remove(JointType.FootLeft);
				toReturn.Remove(JointType.FootRight);
				toReturn.Remove(JointType.WristLeft);
				toReturn.Remove(JointType.WristRight);
			}
			catch { }

			return toReturn;
		}

		static int ResultComparer(KeyValuePair<JointType, double> a, KeyValuePair<JointType, double> b)
		{
			return a.Value.CompareTo(b.Value);
		}
	}
}
