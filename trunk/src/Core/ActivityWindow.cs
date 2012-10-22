using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Utility;

namespace Core
{
	public class ActivityWindow
	{
		private int windowFixedSize = 30;

		public int Size
		{
			get
			{
				return Frames.Count;
			}
			set
			{
				windowFixedSize = value;
			}
		}

		private void FixFramesLenght()
		{
			if (Frames.Count > windowFixedSize)
			{
				Frames.RemoveAt(0);
			}
		}

		public ActivityWindow(List<ImportedSkeleton> aFrames)
		{
			Frames = aFrames;
		}

		public ActivityWindow(int maxSize)
		{
			windowFixedSize = maxSize;
			Frames = new List<ImportedSkeleton>();
		}

		public void Add(ImportedSkeleton skel, int maxSize)
		{
			windowFixedSize = maxSize;
			Frames.Add(skel);
			FixFramesLenght();
		}

		public List<ImportedSkeleton> Frames;

		public bool IsMoving()
		{
			double result = 0.0;

			JointAnglesEvaluator evaluator = new JointAnglesEvaluator(Frames);

			var allJoints = MostInformativeJointsSelector.GetJoints(evaluator.evaluationData, windowFixedSize);

			for(int i = 0; i < Frames.Count-1; i++){
				result += SkeletonComparer.Compare(Frames[i], Frames[i + 1], allJoints);
			}

			Console.WriteLine(result);

			return true;
		}
	}
}
