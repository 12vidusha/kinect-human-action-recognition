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

			ActivityRecord longerRecord;
			ActivityRecord shorterRecord;


			List<ImportedSkeleton> windowPresentedByImportedSkeleton = new List<ImportedSkeleton>();
			foreach (var frame in window.Frames)
			{
				windowPresentedByImportedSkeleton.Add(new ImportedSkeleton(frame));
			}

			if (record.Frames.Count > window.Size)
			{
				longerRecord = record;


				shorterRecord = new ActivityRecord(windowPresentedByImportedSkeleton);
			}
			else
			{
				longerRecord = new ActivityRecord(windowPresentedByImportedSkeleton);
				shorterRecord = record;
			}

			var generalStep = (int)ComputeStep(longerRecord.Frames.Count, shorterRecord.Frames.Count);
			var innerCycleStep = generalStep;
			for (int i = 0; i < shorterRecord.Frames.Count; i++)
			{
				result += SkeletonComparer.CompareWithSMIJ(longerRecord.Frames[innerCycleStep], shorterRecord.Frames[i], mostInformativeJoints);
				innerCycleStep += generalStep;
			}

			return result;
		}

		private static double ComputeStep(int recordLenght, int windowLenght)
		{
			double result = 1;

			if (recordLenght > windowLenght)
			{
				result = recordLenght / windowLenght;
			}

			else
			{
				result = windowLenght / recordLenght;
			}

			return result;
		}
	}
}
