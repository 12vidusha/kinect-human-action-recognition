using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Utility;
using Microsoft.Kinect;
using Microsoft.DirectX;

namespace Core
{
	public static class DynamicTimeWarping
	{
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
				for (int j = 1; j < window.Frames.Count; j++)
				{
					int currentCellOnMiddleDiagonal = (int)((j * windowPresentedByImportedSkeletons.Count) / record.Frames.Count);

					if (toUseSakoeChibaBand)
					{
						if (j < currentCellOnMiddleDiagonal + bandWidth && j > currentCellOnMiddleDiagonal + bandWidth) // Checking if the current cell is in the range
						{
							if (dtwType == DynamicTimeWarpingCalculationType.Standart)
							{
								similarity = SkeletonComparer.CompareWithSMIJ(record.Frames[i], windowPresentedByImportedSkeletons[j], record.MostInformativeJoints);

							}
							else if (dtwType == DynamicTimeWarpingCalculationType.Derivative)
							{
								//similarity = SkeletonComparer.CompareDerivativesWithSMIJ(record.Frames[i], windowPresentedByImportedSkeletons[j], record.MostInformativeJoints);
								ImportedSkeleton mainSkeletonDerivatives = CalculateSkeletonDerivative(record.Frames, i);
								ImportedSkeleton secondarySkeletonDerivatives = CalculateSkeletonDerivative(windowPresentedByImportedSkeletons, i);
								similarity = SkeletonComparer.CompareWithSMIJ(mainSkeletonDerivatives, secondarySkeletonDerivatives, record.MostInformativeJoints);
							}
						}
					}
					else //Start a DTW search without using Sakoe-Chiba band
					{
						if (dtwType == DynamicTimeWarpingCalculationType.Derivative)
						{

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

		static ImportedSkeleton CalculateSkeletonDerivative(List<ImportedSkeleton> query, int index)
		{
			var skeletonToReturn = new ImportedSkeleton();

			int skelID = 0;
			foreach (var skel in query)
			{
				if (skelID == 0)
				{
					continue;
				}

				if (skelID == query.Count)
				{
					break;
				}

				foreach (var jointName in Enum.GetNames(typeof(JointType)))
				{
					var jointType = (JointType)(Enum.Parse(typeof(JointType), jointName));

					Quaternion currentQuaternion = new Quaternion(
						skel.HiararchicalQuaternions[jointType].X,
						skel.HiararchicalQuaternions[jointType].Y,
						skel.HiararchicalQuaternions[jointType].Z,
						skel.HiararchicalQuaternions[jointType].W);

					Quaternion previousQuaternion = new Quaternion(
						query[skelID - 1].HiararchicalQuaternions[jointType].X,
						query[skelID - 1].HiararchicalQuaternions[jointType].Y,
						query[skelID - 1].HiararchicalQuaternions[jointType].Z,
						query[skelID - 1].HiararchicalQuaternions[jointType].W);

					Quaternion nextQuaternion = new Quaternion(
						query[skelID + 1].HiararchicalQuaternions[jointType].X,
						query[skelID + 1].HiararchicalQuaternions[jointType].Y,
						query[skelID + 1].HiararchicalQuaternions[jointType].Z,
						query[skelID + 1].HiararchicalQuaternions[jointType].W);

					Quaternion previousMinusNextQuaternionDividedByTwo = Quaternion.Subtract(previousQuaternion, nextQuaternion);
					previousQuaternion.X /= 2;
					previousQuaternion.Y /= 2;
					previousQuaternion.Z /= 2;
					previousQuaternion.W /= 2;

					Quaternion calculatedQuaternionDerivative = Quaternion.Subtract(currentQuaternion, previousQuaternion) + previousMinusNextQuaternionDividedByTwo;
					calculatedQuaternionDerivative.X /= 2;
					calculatedQuaternionDerivative.Y /= 2;
					calculatedQuaternionDerivative.Z /= 2;
					calculatedQuaternionDerivative.W /= 2;

					skeletonToReturn.HiararchicalQuaternions[jointType].X = calculatedQuaternionDerivative.X;
					skeletonToReturn.HiararchicalQuaternions[jointType].Y = calculatedQuaternionDerivative.Y;
					skeletonToReturn.HiararchicalQuaternions[jointType].Y = calculatedQuaternionDerivative.Y;
					skeletonToReturn.HiararchicalQuaternions[jointType].Z = calculatedQuaternionDerivative.Z;
					}

				skelID++;
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
