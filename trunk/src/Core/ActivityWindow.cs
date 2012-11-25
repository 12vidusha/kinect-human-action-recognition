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
				Frames.Dequeue();
				//Console.WriteLine(Frames.Dequeue().HiararchicalQuaternions[JointType.KneeLeft].W);
			}
		}

		public ActivityWindow(Queue<ImportedSkeleton> aFrames)
		{
			Frames = aFrames;
		}

		public ActivityWindow(int maxSize)
		{
			windowFixedSize = maxSize;
			Frames = new Queue<ImportedSkeleton>();

		}

		public void Add(ImportedSkeleton skel, int maxSize)
		{
			windowFixedSize = maxSize;
			Frames.Enqueue(skel);
			FixFramesLenght();
		}

		public Queue<ImportedSkeleton> Frames;

		
	}
}
