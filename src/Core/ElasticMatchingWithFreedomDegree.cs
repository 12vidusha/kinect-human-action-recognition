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

			for (int i = 0; i < window.Frames.Count; i++)
			{
				var _frame = window.Frames.ToArray()[i];
				windowPresentedByImportedSkeleton.Add(new ImportedSkeleton(_frame));
				//Console.WriteLine(windowPresentedByImportedSkeleton[i].HiararchicalQuaternions[Microsoft.Kinect.JointType.KneeLeft].W);
			}

			if (record.Frames.Count > window.Size)
			{
				longerRecord = record;

				shorterRecord = new ActivityRecord(windowPresentedByImportedSkeleton);

				FindBeginning(longerRecord, shorterRecord);
			}
			else
			{
				longerRecord = new ActivityRecord(windowPresentedByImportedSkeleton);
				shorterRecord = record;

				FindBeginning(shorterRecord, longerRecord);
			}

			var generalStep = ComputeStep(longerRecord.Frames.Count, shorterRecord.Frames.Count);
			var innerCycleStep = generalStep + 1;
			for (int i = 0; i < shorterRecord.Frames.Count; i++)
			{
				result += Math.Min(
					SkeletonComparer.CompareWithSMIJ(longerRecord.Frames[(int)innerCycleStep-1], shorterRecord.Frames[i], mostInformativeJoints),
					Math.Min(SkeletonComparer.CompareWithSMIJ(longerRecord.Frames[(int)innerCycleStep], shorterRecord.Frames[i], mostInformativeJoints),
						SkeletonComparer.CompareWithSMIJ(longerRecord.Frames[(int)innerCycleStep+1], shorterRecord.Frames[i], mostInformativeJoints)));
				
				innerCycleStep += generalStep;
				if (innerCycleStep+1 >= longerRecord.Frames.Count)
				{
					break; 
				}
			}

			return result;
		}

		private static double ComputeStep(int recordLenght, int windowLenght)
		{
			double result = 1;

			if (recordLenght > windowLenght)
			{
				result = recordLenght / (double)windowLenght;
			}

			else
			{
				result = windowLenght / (double)recordLenght;
			}

			return result;
		}


		private static int FindBeginning(ActivityRecord record, ActivityRecord window)
		{
			int indexFound = 0;

			Dictionary<int, double> statistic = new Dictionary<int, double>();

			for (int i = 0; i < window.Frames.Count / 2; i++)
			{
				var skel = window.Frames[i];
				statistic.Add(i, SkeletonComparer.CompareWithSMIJ(record.Frames[2], skel, record.MostInformativeJoints));
			}

			var list = statistic.ToList();

			list.Sort(Comparer);

			return indexFound;
		}

		private static int Comparer(KeyValuePair<int, double> A, KeyValuePair<int, double> B)
		{
			return A.Value.CompareTo(B.Value);
		}
	}
}
