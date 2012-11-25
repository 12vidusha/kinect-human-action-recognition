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
            
        }
    }
}
