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
		static List<ActivityRecord> recordDerivatives = new List<ActivityRecord>();

		public static double CompareActivities(ActivityRecord record, ActivityWindow window, DynamicTimeWarpingCalculationType dtwType, DynamicTimeWarpingPathTypes pathType = DynamicTimeWarpingPathTypes.Standart, bool toUseSakoeChibaBand = false, double bandWidthInProcentage = 0.1)
		{
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

			var windowPresentedByImportedSkeletons = new List<ImportedSkeleton>();

			foreach (var skeleton in window.Frames)
			{
				windowPresentedByImportedSkeletons.Add(new ImportedSkeleton(skeleton));
			}

			int bandWidth = (int)(windowPresentedByImportedSkeletons.Count * bandWidthInProcentage);

			for (int i = 1; i < record.Frames.Count; i++)
			{
				for (int j = 1; j < windowPresentedByImportedSkeletons.Count; j++)
				{
					if (toUseSakoeChibaBand)
					{
						int currentCellOnMiddleDiagonal = (int)((j * windowPresentedByImportedSkeletons.Count) / record.Frames.Count);
						if (j > currentCellOnMiddleDiagonal - bandWidth && j < currentCellOnMiddleDiagonal + bandWidth) // Checking if the current cell is in the range
						{
							if (dtwType == DynamicTimeWarpingCalculationType.Standart)
							{
								similarity = SkeletonComparer.CompareWithSMIJ(record.Frames[i], windowPresentedByImportedSkeletons[j], record.MostInformativeJoints);

							}
							else if (dtwType == DynamicTimeWarpingCalculationType.Derivative)
							{
								if (i != record.Frames.Count)
								{
									ImportedSkeleton mainSkeletonDerivatives = CalculateSkeletonDerivative(record.Frames, i, record.MostInformativeJoints);
									ImportedSkeleton secondarySkeletonDerivatives = CalculateSkeletonDerivative(windowPresentedByImportedSkeletons, i, record.MostInformativeJoints);
									similarity = SkeletonComparer.CompareWithSMIJ(mainSkeletonDerivatives, secondarySkeletonDerivatives, record.MostInformativeJoints);
								}
							}
						}
					}
					else //Start a DTW search without using Sakoe-Chiba band
					{
						if (dtwType == DynamicTimeWarpingCalculationType.Derivative)
						{
							if (i != record.Frames.Count)
							{
								ImportedSkeleton mainSkeletonDerivatives = CalculateSkeletonDerivative(record.Frames, i, record.MostInformativeJoints);
								ImportedSkeleton secondarySkeletonDerivatives = CalculateSkeletonDerivative(windowPresentedByImportedSkeletons, i, record.MostInformativeJoints);
								similarity = SkeletonComparer.CompareWithSMIJ(mainSkeletonDerivatives, secondarySkeletonDerivatives, record.MostInformativeJoints);
							}
						}
						else if (dtwType == DynamicTimeWarpingCalculationType.Standart)
						{
							similarity = SkeletonComparer.CompareWithSMIJ(record.Frames[i], windowPresentedByImportedSkeletons[j], record.MostInformativeJoints);
						}
					}
					FillDTWTableCell(pathType, DTW, i, j, similarity);
				}
			}

			return DTW[record.Frames.Count - 1, window.Frames.Count - 1] / record.MostInformativeJoints.Count;
		}

		private static ImportedSkeleton CalculateSkeletonDerivative(List<ImportedSkeleton> query, int index, List<JointType> mostInformativeJoints)
		{
			var skeletonToReturn = new ImportedSkeleton();
			if (index < query.Count - 1)
			{
				foreach (var jointType in mostInformativeJoints)
				{

					var currentMinusPrev = new JointRotation(
						query[index].HiararchicalQuaternions[jointType].X - query[index - 1].HiararchicalQuaternions[jointType].X,
						query[index].HiararchicalQuaternions[jointType].Y - query[index - 1].HiararchicalQuaternions[jointType].Y,
						query[index].HiararchicalQuaternions[jointType].Z - query[index - 1].HiararchicalQuaternions[jointType].Z,
						query[index].HiararchicalQuaternions[jointType].W - query[index - 1].HiararchicalQuaternions[jointType].W);

					var nextMinusPrevDividedByTwo = new JointRotation(
						(query[index + 1].HiararchicalQuaternions[jointType].X - query[index - 1].HiararchicalQuaternions[jointType].X) / 2,
						(query[index + 1].HiararchicalQuaternions[jointType].Y - query[index - 1].HiararchicalQuaternions[jointType].Y) / 2,
						(query[index + 1].HiararchicalQuaternions[jointType].Z - query[index - 1].HiararchicalQuaternions[jointType].Z) / 2,
						(query[index + 1].HiararchicalQuaternions[jointType].W - query[index - 1].HiararchicalQuaternions[jointType].W) / 2);

					var calculatedDerivative = new JointRotation(
						(currentMinusPrev.X + nextMinusPrevDividedByTwo.X) / 2,
						(currentMinusPrev.Y + nextMinusPrevDividedByTwo.Y) / 2,
						(currentMinusPrev.Z + nextMinusPrevDividedByTwo.Z) / 2,
						(currentMinusPrev.W + nextMinusPrevDividedByTwo.W) / 2
						);

					skeletonToReturn.HiararchicalQuaternions[jointType] = calculatedDerivative;

				}
			}
			return skeletonToReturn;
		}

		private static void FillDTWTableCell(DynamicTimeWarpingPathTypes pathType, double[,] DTW, int i, int j, double similarity)
		{
			if (pathType == DynamicTimeWarpingPathTypes.Standart)
			{
				DTW[i, j] = similarity +
					Math.Min(
						Math.Min(
						DTW[i - 1, j],
						DTW[i, j - 1]),
						DTW[i - 1, j - 1]
					);
			}
			else if (pathType == DynamicTimeWarpingPathTypes.AlwaysDiagonally)
			{


				if (i == 1 || j == 1)
				{
					DTW[i, j] = similarity +
					Math.Min(
						Math.Min(
						DTW[i - 1, j],
						DTW[i, j - 1]),
						DTW[i - 1, j - 1]
					);
				}

				else
				{
					DTW[i, j] = similarity +
					Math.Min(
						Math.Min(
						DTW[i - 1, j - 2],
						DTW[i - 2, j - 1]),
						DTW[i - 1, j - 1]
					);
				}
			}
		}
	}
}
