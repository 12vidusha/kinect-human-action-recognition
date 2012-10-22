using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using MatrixVector;

namespace Utility
{
    public class JointAnglesEvaluator
    {
		private List<ImportedSkeleton> mSkeletonCollection = null;

        public Dictionary<JointType, MatrixVector.Vector3> evaluationData = new Dictionary<JointType, MatrixVector.Vector3>();

		public JointAnglesEvaluator(List<ImportedSkeleton> aSkeletonCollection)
        {
            mSkeletonCollection = aSkeletonCollection;

            EvaluateAngles();
        }

        private void EvaluateAngles()
        {
            foreach (var joint in Enum.GetNames(typeof(JointType)))
            {
                evaluationData.Add((JointType)Enum.Parse(typeof(JointType), joint), new MatrixVector.Vector3());
            }

            for (int i = 1; i < mSkeletonCollection.Count; i++)
            {
                var firstSkel = mSkeletonCollection[i - 1];
                var secondSkel = mSkeletonCollection[i];

                foreach (var joint in Enum.GetNames(typeof(JointType)))
                {
                    var jointType = (JointType)Enum.Parse(typeof(JointType), joint);

                    var jointEvaluation = new Vector3();


                    try
                    {
                        jointEvaluation.X = Math.Abs((
                            secondSkel.Joints[jointType].Position.X - secondSkel.Joints[jointType + 1].Position.X)
                            - (firstSkel.Joints[jointType].Position.X - firstSkel.Joints[jointType + 1].Position.X));


                        jointEvaluation.Y = Math.Abs((secondSkel.Joints[jointType].Position.Y - secondSkel.Joints[jointType + 1].Position.Y)
                            - (firstSkel.Joints[jointType].Position.Y - firstSkel.Joints[jointType + 1].Position.Y));


                        jointEvaluation.Z = Math.Abs((secondSkel.Joints[jointType].Position.Z - secondSkel.Joints[jointType + 1].Position.Z)
                            - (firstSkel.Joints[jointType].Position.Z - firstSkel.Joints[jointType + 1].Position.Z));
                    }
                    catch (Exception e) { }


                    //jointEvaluation.X += Math.Abs(secondSkel.AngledJoints[jointType].XY - firstSkel.AngledJoints[jointType].XY);
                    //jointEvaluation.Y += Math.Abs(secondSkel.AngledJoints[jointType].XZ - firstSkel.AngledJoints[jointType].XZ);
                    //jointEvaluation.Y += Math.Abs(secondSkel.AngledJoints[jointType].YZ - firstSkel.AngledJoints[jointType].YZ);

                    evaluationData[jointType] = jointEvaluation;
                }
            }
        }
    }
}
