using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Utility
{
	public class ImportedSkeleton : Skeleton
	{
		public ImportedSkeleton(Skeleton skeleton) : base() {

			InitializeDictionaries();
			CopyDataFromSkeleton(skeleton);
		}

		public ImportedSkeleton()
			: base()
		{
			InitializeDictionaries();

			var skeleton = new Skeleton();
			CopyDataFromSkeleton(skeleton);
		}

		private void InitializeDictionaries()
		{
			foreach (var key in Enum.GetNames(typeof(JointType)))
			{
				var joint = (JointType)Enum.Parse(typeof(JointType), key);
				HiararchicalQuaternions.Add(joint, new JointRotation());
				AbsoluteQuaternions.Add(joint, new JointRotation());
			}
		}

		private void CopyDataFromSkeleton(Skeleton skeleton)
		{
			this.BoneOrientations = skeleton.BoneOrientations;
			
			foreach (var key in Enum.GetNames(typeof(JointType)))
			{
				var joint = (JointType)Enum.Parse(typeof(JointType), key);
				this.Joints[joint]  = skeleton.Joints[joint];

				this.HiararchicalQuaternions[joint].X = skeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion.X;
				this.HiararchicalQuaternions[joint].Y = skeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion.Y;
				this.HiararchicalQuaternions[joint].Z = skeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion.Z;
				this.HiararchicalQuaternions[joint].W = skeleton.BoneOrientations[joint].HierarchicalRotation.Quaternion.W;

				this.AbsoluteQuaternions[joint].X = skeleton.BoneOrientations[joint].AbsoluteRotation.Quaternion.X;
				this.AbsoluteQuaternions[joint].Y = skeleton.BoneOrientations[joint].AbsoluteRotation.Quaternion.Y;
				this.AbsoluteQuaternions[joint].Z = skeleton.BoneOrientations[joint].AbsoluteRotation.Quaternion.Z;
				this.AbsoluteQuaternions[joint].W = skeleton.BoneOrientations[joint].AbsoluteRotation.Quaternion.W;
			}

			this.ClippedEdges = skeleton.ClippedEdges;
			this.Position = skeleton.Position;
			this.TrackingId = skeleton.TrackingId;
			this.TrackingState = skeleton.TrackingState;
		}

		public Dictionary<JointType, JointRotation> HiararchicalQuaternions = new Dictionary<JointType, JointRotation>();
		public Dictionary<JointType, JointRotation> AbsoluteQuaternions = new Dictionary<JointType, JointRotation>();
	}
}
