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

		public ActivityWindow(List<Skeleton> aFrames)
		{
			Frames = aFrames;
		}

		public ActivityWindow(int maxSize)
		{
			windowFixedSize = maxSize;
			Frames = new List<Skeleton>();
		}

		public void Add(ImportedSkeleton skel, int maxSize)
		{
			windowFixedSize = maxSize;
			Frames.Add(skel);
			FixFramesLenght();
		}

		public List<Skeleton> Frames;

		
	}
}
