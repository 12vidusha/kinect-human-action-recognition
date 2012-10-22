using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using MatrixVector;

namespace SkeletalTracking.Utility
{
    public static class SkeletonComparer
    {
        public static double Compare(Skeleton mainSkeleton, Skeleton secondarySkeleton, List<JointType> mostInformativeJoints)
        {
            double overall = 0;

            foreach (var joint in mostInformativeJoints)
            {
                Vector3 mainVector = new Vector3(
                    mainSkeleton.Joints[joint].Position.X,
                    mainSkeleton.Joints[joint].Position.Y,
                    mainSkeleton.Joints[joint].Position.Z);

                Vector3 secondaryVector = new Vector3(
                    secondarySkeleton.Joints[joint].Position.X,
                    secondarySkeleton.Joints[joint].Position.Y,
                    secondarySkeleton.Joints[joint].Position.Z);

                overall += mainVector.DistanceToWithoutSquare(secondaryVector);
            }
            return overall;
        }
    }
}
