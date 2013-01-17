using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Core
{
	public static class ElasticMatchingWithFreedomDegree
	{
		public static double CompareActivities(ActivityRecord record, ActivityWindow window)
		{
			double result = 0;

			var mostInformativeJoints = record.MostInformativeJoints;

			List<ImportedSkeleton> windowPresentedByImportedSkeleton = new List<ImportedSkeleton>();

			foreach (var frame in window.Frames)
			{
				windowPresentedByImportedSkeleton.Add(window.Frames.Dequeue());
			}

			for (int i = 1; i < record.Frames.Count; i++)
			{
				for (int j = 0; j < windowPresentedByImportedSkeleton.Count; j++)
				{
					int index = (j / record.Frames.Count) * windowPresentedByImportedSkeleton.Count;
					result += SkeletonComparer.CompareWithSMIJ(record.Frames[i], windowPresentedByImportedSkeleton[index], mostInformativeJoints);
				}
			}
			return result;
		}
	}
}
