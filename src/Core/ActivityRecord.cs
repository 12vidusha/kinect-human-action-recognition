using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Utility;

namespace Core
{
	public class ActivityRecord
	{
		public ActivityRecord() {
			Frames = new List<ImportedSkeleton>();
		}

		public ActivityRecord(List<ImportedSkeleton> aFrames)
		{
			Frames = aFrames;
			MostInformativeJoints = MostInformativeJointsSelector.GetJoints(Frames, Frames.Count, QuaternionsStyles.Hierarchical);
		}

		public List<ImportedSkeleton> Frames;

		public List<JointType> MostInformativeJoints { get; set; }
	}
}
