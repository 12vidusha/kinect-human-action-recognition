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

			foreach (var key in Enum.GetNames(typeof(JointType)))
			{
				var joint = (JointType)Enum.Parse(typeof(JointType), key);
				Quaterions.Add(joint, new JointRotation());
			}

			CopyDataFromSkeleton(skeleton);
		}

		public ImportedSkeleton()
			: base()
		{
			foreach (var key in Enum.GetNames(typeof(JointType)))
			{
				var joint = (JointType)Enum.Parse(typeof(JointType), key);
				Quaterions.Add(joint, new JointRotation());
			}

			var skeleton = new Skeleton();
			CopyDataFromSkeleton(skeleton);
		}

		private void CopyDataFromSkeleton(Skeleton skeleton)
		{
			this.BoneOrientations = skeleton.BoneOrientations;
			
			foreach (var key in Enum.GetNames(typeof(JointType)))
			{
				var joint = (JointType)Enum.Parse(typeof(JointType), key);
				this.Joints[joint]  = skeleton.Joints[joint];

				this.Quaterions[joint].X = BoneOrientations[joint].HierarchicalRotation.Quaternion.X;
				this.Quaterions[joint].Y = BoneOrientations[joint].HierarchicalRotation.Quaternion.Y;
				this.Quaterions[joint].Z = BoneOrientations[joint].HierarchicalRotation.Quaternion.Z;
				this.Quaterions[joint].W = BoneOrientations[joint].HierarchicalRotation.Quaternion.W;
			}

			this.ClippedEdges = skeleton.ClippedEdges;
			this.Position = skeleton.Position;
			this.TrackingId = skeleton.TrackingId;
			this.TrackingState = skeleton.TrackingState;
		}

		public Dictionary<JointType, JointRotation> Quaterions = new Dictionary<JointType,JointRotation>();
	}
}
