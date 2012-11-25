using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using MatrixVector;

namespace Utility
{
	public static class SkeletonComparer
	{
		public static Vector3 RotateVectorByQuaternion(Vector3 vec, Vector4 q)
		{
			vec.Normalize();

			Vector4 v = new Vector4(), r = new Vector4();

			v.X = vec.X;
			v.Y = vec.Y;
			v.Z = vec.Z;
			v.W = 0.0f;

			r = MultiplyQuaternion(v, GetConjugateQuaternion(q));
			r = MultiplyQuaternion(r, q);

			Vector3 res = new Vector3();

			res.X = r.X;
			res.Y = r.Y;
			res.Z = r.Z;

			return res;
		}

		private static Vector4 GetConjugateQuaternion(Vector4 q)
		{
			Vector4 conjugate = new Vector4();

			conjugate.X = -q.X;
			conjugate.Y = -q.Y;
			conjugate.Z = -q.Z;
			conjugate.W = q.W;

			return conjugate;
		}


		private static Vector4 MultiplyQuaternion(Vector4 r, Vector4 rq)
		{
			Vector4 res = new Vector4();

			res.X = r.W * rq.X + r.X * rq.W + r.Y * rq.Z - r.Z * rq.Y;
			res.Y = r.W * rq.Y + r.Y * rq.W + r.Z * rq.X - r.X * rq.Z;
			res.Y = r.W * rq.Z + r.Z * rq.W + r.X * rq.Y - r.Y * rq.X;
			res.W = r.W * rq.W - r.X * rq.X - r.Y * rq.Y - r.Z * rq.Z;
			return res;
		}

		public static double CompareWithSMIJ(ImportedSkeleton mainSkeleton, Skeleton secondarySkeleton, List<JointType> mostInformativeJoints)
		{
			double overall = 0;

			int jointID = 0;
			foreach (var joint in mostInformativeJoints)
			{
				var jointWeight = (mostInformativeJoints.Count - jointID) == 0 ? 1 : (mostInformativeJoints.Count - jointID);

				var similarity = CompareQuaternions(mainSkeleton.HiararchicalQuaternions[joint], 
						secondarySkeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion);

				overall += (similarity * 1000);// * jointWeight;
				#region Projections method
				//Vector3 mainVector = new Vector3(
				//    mainSkeleton.AngledJoints[joint].XY,
				//    mainSkeleton.AngledJoints[joint].XZ,
				//    mainSkeleton.AngledJoints[joint].YZ);

				//mainVector.Normalize();

				//Vector3 secondaryVector = new Vector3(
				//    secondarySkeleton.AngledJoints[joint].XY,
				//    secondarySkeleton.AngledJoints[joint].XZ,
				//    secondarySkeleton.AngledJoints[joint].YZ);

				//secondaryVector.Normalize();

				//var similarity = mainVector.DistanceToWithoutSquare(secondaryVector);

				//if(!double.IsNaN(similarity)){
				//    overall += similarity * jointWeight;
				//}

				#endregion

				#region Rotating vectors by quaternions and measuring the distance between them
				//var q = mainSkeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion;
				//var r = secondarySkeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion;

				//var first = new Vector3();
				//first.x = 1; first.y = 0; first.z = 0; 
				//var second = new Vector3();
				//second.x = 1; second.y = 0; second.z = 0;

				//first = RotateVectorByQuaternion(first, q);

				//second = RotateVectorByQuaternion(second, r);

				//overall += first.DistanceToWithoutSquare(second);
				#endregion

				jointID++;
			}

			#region Hip rotation calculation

			//Vector3 mainSkeletonDirection = new Vector3(
			//    mainSkeleton.Joints[JointType.HipLeft].Position.X - mainSkeleton.Joints[JointType.HipCenter].Position.X,
			//    mainSkeleton.Joints[JointType.HipLeft].Position.Y - mainSkeleton.Joints[JointType.HipCenter].Position.Y,
			//    mainSkeleton.Joints[JointType.HipLeft].Position.Z - mainSkeleton.Joints[JointType.HipCenter].Position.Z);

			//Vector3 secondarySkeletonDirection = new Vector3(
			//    secondarySkeleton.Joints[JointType.HipLeft].Position.X - secondarySkeleton.Joints[JointType.HipCenter].Position.X,
			//    secondarySkeleton.Joints[JointType.HipLeft].Position.Y - secondarySkeleton.Joints[JointType.HipCenter].Position.Y,
			//    secondarySkeleton.Joints[JointType.HipLeft].Position.Z - secondarySkeleton.Joints[JointType.HipCenter].Position.Z);

			//mainSkeletonDirection.Normalize();
			//secondarySkeletonDirection.Normalize();

			//var advance = mainSkeletonDirection.DistanceToWithoutSquare(secondarySkeletonDirection);

			//overall *= advance;

			//Console.WriteLine(advance);
			#endregion

			return overall;
		}

		public static double CompareQuaternions(JointRotation mainQuaternion, JointRotation secondaryQuaternion)
		{
			//(x1-x2)^2 + (y1-y2)^2 + (z1-z2)^2 + (w1 - w2)^2

			double distanceX = mainQuaternion.X - secondaryQuaternion.X;
			double distanceY = mainQuaternion.Y - secondaryQuaternion.Y;
			double distanceZ = mainQuaternion.Z - secondaryQuaternion.Z;
			double distanceW = mainQuaternion.W - secondaryQuaternion.W;

			double similarity = distanceX * distanceX +
				distanceY * distanceY +
				distanceZ * distanceZ +
				distanceW * distanceW;

			return similarity;
		}

		public static double CompareQuaternions(JointRotation mainQuaternion, Vector4 secondaryQuaternion)
		{
			JointRotation newSecondaryQuaternion = new JointRotation(secondaryQuaternion.X, secondaryQuaternion.Y, secondaryQuaternion.Z, secondaryQuaternion.W);

			return CompareQuaternions(mainQuaternion, newSecondaryQuaternion);
		}

		public static double CompareQuaternions(Vector4 mainQuaternion, Vector4 secondaryQuaternion)
		{
			JointRotation newMainQuaternion = new JointRotation(mainQuaternion.X, mainQuaternion.Y, mainQuaternion.Z, mainQuaternion.W);
			JointRotation newSecondaryQuaternion = new JointRotation(secondaryQuaternion.X, secondaryQuaternion.Y, secondaryQuaternion.Z, secondaryQuaternion.W);
			

			return CompareQuaternions(newMainQuaternion, newSecondaryQuaternion);
		}
	}
}
