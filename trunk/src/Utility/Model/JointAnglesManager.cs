using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Collections;
using System.Data;
using MatrixVector;

namespace Utility.Model
{
    /// <summary>
    /// how to start:
    /// start with the center of the hips
    /// </summary>
    public class JointAnglesManager
    {
        /// <summary>
        /// The whole skeleton
        /// </summary>
        private Skeleton skeletonData;

		public JointAnglesManager(Skeleton aSkeleton)
		{
			skeletonData = aSkeleton;
		}

		public ImportedSkeleton GetComputedAngles(Skeleton aSkeleton)
		{
			
			ImportedSkeleton skeleton = new ImportedSkeleton(aSkeleton);

			#region Getting angles based on the projection of the coordinates
			//Hashtable angles = GetAllAnglesRaw();

			//foreach (JointType key in angles.Keys)
			//{
			//    List<double> anglesInDouble = (List<double>)angles[key];

			//    SkeletonPoint point = new SkeletonPoint();

			//    if (!double.IsNaN(anglesInDouble[0]))
			//    {
			//        skeleton.Quaterions[key].XY = (float)anglesInDouble[0];
			//    }
			//    if (!double.IsNaN(anglesInDouble[1]))
			//    {
			//        skeleton.Quaterions[key].XZ = (float)anglesInDouble[1];
			//    }
			//    if (!double.IsNaN(anglesInDouble[2]))
			//    {
			//        skeleton.Quaterions[key].YZ = (float)anglesInDouble[2];
			//    }
			//}
			#endregion

			return skeleton;
		}
	}
}
