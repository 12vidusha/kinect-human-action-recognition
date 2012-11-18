using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Utility
{
	static class CompareJoints
	{
		public static double Compare(JointRotation firstJoint, JointRotation secondJoint)
		{
			Vector4 mainQuaternion = new Vector4();
			mainQuaternion.X = firstJoint.X;
			mainQuaternion.Y = firstJoint.Y;
			mainQuaternion.Z = firstJoint.Z;
			mainQuaternion.W = firstJoint.W;

			Vector4 secondaryQuaternion = new Vector4();
			secondaryQuaternion.X = secondJoint.X;
			secondaryQuaternion.Y = secondJoint.Y;
			secondaryQuaternion.Z = secondJoint.Z;
			secondaryQuaternion.W = secondJoint.W;

			double distanceX = mainQuaternion.X - secondaryQuaternion.X;
			double distanceY = mainQuaternion.Y - secondaryQuaternion.Y;
			double distanceZ = mainQuaternion.Z - secondaryQuaternion.Z;
			double distanceW = mainQuaternion.W - secondaryQuaternion.W;

			double similarity = distanceX*distanceX +
				distanceY * distanceY +
				distanceZ * distanceZ +
				distanceW * distanceW;

			return similarity;
		}
	}
}
