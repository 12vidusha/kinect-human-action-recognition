using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Utility
{
	public static class GetFirstSkeleton
	{
		public static Skeleton Get(AllFramesReadyEventArgs e, Skeleton[] allSkeletons)
		{
			using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
			{
				if (skeletonFrameData != null)
				{
					skeletonFrameData.CopySkeletonDataTo(allSkeletons);
					//get the first tracked skeleton
					Skeleton first = (from s in allSkeletons
									  where s.TrackingState == SkeletonTrackingState.Tracked
									  select s).FirstOrDefault();
					return first;
				}
			}
			return null;
		}
	}
}
