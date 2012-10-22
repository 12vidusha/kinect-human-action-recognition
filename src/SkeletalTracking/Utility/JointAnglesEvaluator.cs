using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SkeletalTracking.Utility
{
    public class JointAnglesEvaluator
    {
        private List<Skeleton> mSkeletonCollection = null;

        public Dictionary<JointType, SkeletonPoint> evaluationData = new Dictionary<JointType, SkeletonPoint>();

        public JointAnglesEvaluator(List<Skeleton> aSkeletonCollection)
        {
            mSkeletonCollection = aSkeletonCollection;

            EvaluateAngles();
        }

        private void EvaluateAngles()
        {
            foreach (var joint in Enum.GetNames(typeof(JointType)))
            {
                evaluationData.Add((JointType)Enum.Parse(typeof(JointType), joint),  new SkeletonPoint());
            }

            for (int i = 1; i < mSkeletonCollection.Count; i++)
            {
                var firstSkel = mSkeletonCollection[i - 1];
                var secondSkel = mSkeletonCollection[i];

                foreach (var joint in Enum.GetNames(typeof(JointType)))
                {
                    var jointType = (JointType)Enum.Parse(typeof(JointType), joint);

                    var skelPoint = evaluationData[jointType];

                    if (skelPoint.X != -1)
                    {
                        skelPoint.X += Math.Abs(secondSkel.Joints[jointType].Position.X - firstSkel.Joints[jointType].Position.X);
                    }
                    if (skelPoint.Y != -1)
                    {
                        skelPoint.Y += Math.Abs(secondSkel.Joints[jointType].Position.Y - firstSkel.Joints[jointType].Position.Y);
                    }
                    if (skelPoint.Z != -1)
                    {
                        skelPoint.Z += Math.Abs(secondSkel.Joints[jointType].Position.Z - firstSkel.Joints[jointType].Position.Z);
                    }

                    evaluationData[jointType] = skelPoint;
                }
            }
        }
    }
}
