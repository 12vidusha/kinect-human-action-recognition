using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Collections;
using System.Data;
using MatrixVector;

namespace SkeletalTracking.Model
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

        /// <summary>
        /// link between the joints
        /// </summary>
        private Hashtable jointLinks = new Hashtable();

        public SkeletonPoint kneeCenter = new SkeletonPoint();

        public double kinectElevation = 0;

		public JointAnglesManager(Skeleton aSkeleton)
		{
			data = aSkeleton;
		}


        /// <summary>
        /// includes init() and calculation of kneeCenter
        /// </summary>
        private Skeleton data
        {
            set
            {
                skeletonData = new Skeleton();
                skeletonData = value;
                kneeCenter.X = (skeletonData.Joints[JointType.KneeLeft].Position.X + skeletonData.Joints[JointType.KneeRight].Position.X) / 2;
                kneeCenter.Y = (skeletonData.Joints[JointType.KneeLeft].Position.Y + skeletonData.Joints[JointType.KneeRight].Position.Y) / 2;
                kneeCenter.Z = (skeletonData.Joints[JointType.KneeLeft].Position.Z + skeletonData.Joints[JointType.KneeRight].Position.Z) / 2;
                //kneeCenter = Utils.Kinect.fixElevation(kneeCenter);
                init();
            }
            get
            {
                return skeletonData;
            }
        }
        private void init()
        {
            SkeletonPoint h = new SkeletonPoint();
            SkeletonPoint v = new SkeletonPoint();
            SkeletonPoint z = new SkeletonPoint();
            Vector3 tmp;
            if (jointLinks.Count > 0)
            {
                jointLinks = new Hashtable();
            }
            //init the joint links
            h.X = skeletonData.Joints[JointType.KneeLeft].Position.X - skeletonData.Joints[JointType.KneeRight].Position.X;
            h.Y = skeletonData.Joints[JointType.KneeLeft].Position.Y - skeletonData.Joints[JointType.KneeRight].Position.Y;
            h.Z = skeletonData.Joints[JointType.KneeLeft].Position.Z - skeletonData.Joints[JointType.KneeRight].Position.Z;
            v.X = skeletonData.Joints[JointType.HipCenter].Position.X - kneeCenter.X;
            v.Y = skeletonData.Joints[JointType.HipCenter].Position.Y - kneeCenter.Y;
            v.Z = skeletonData.Joints[JointType.HipCenter].Position.Z - kneeCenter.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;

            jointLinks.Add(JointType.HipCenter,
                new SkeletonPoint[] { kneeCenter, 
                    skeletonData.Joints[JointType.HipCenter].Position,
                    skeletonData.Joints[JointType.HipCenter].Position,
                    skeletonData.Joints[JointType.ShoulderCenter].Position,
                    z
                });

            ///---->hip left
            h.X = skeletonData.Joints[JointType.HipLeft].Position.X - skeletonData.Joints[JointType.HipRight].Position.X;
            h.Y = skeletonData.Joints[JointType.HipLeft].Position.Y - skeletonData.Joints[JointType.HipRight].Position.Y;
            h.Z = skeletonData.Joints[JointType.HipLeft].Position.Z - skeletonData.Joints[JointType.HipRight].Position.Z;
            v.X = skeletonData.Joints[JointType.HipLeft].Position.X - skeletonData.Joints[JointType.ShoulderLeft].Position.X;
            v.Y = skeletonData.Joints[JointType.HipLeft].Position.Y - skeletonData.Joints[JointType.ShoulderLeft].Position.Y;
            v.Z = skeletonData.Joints[JointType.HipLeft].Position.Z - skeletonData.Joints[JointType.ShoulderLeft].Position.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;

            jointLinks.Add(JointType.HipLeft,
                new SkeletonPoint[] { skeletonData.Joints[JointType.HipLeft].Position,
                    skeletonData.Joints[JointType.ShoulderLeft].Position,
                    skeletonData.Joints[JointType.KneeLeft].Position,
                    skeletonData.Joints[JointType.HipLeft].Position,
                    z
                });
            ///----->hip right
            h.X = skeletonData.Joints[JointType.HipRight].Position.X - skeletonData.Joints[JointType.HipLeft].Position.X;
            h.Y = skeletonData.Joints[JointType.HipRight].Position.Y - skeletonData.Joints[JointType.HipLeft].Position.Y;
            h.Z = skeletonData.Joints[JointType.HipRight].Position.Z - skeletonData.Joints[JointType.HipLeft].Position.Z;
            v.X = skeletonData.Joints[JointType.HipRight].Position.X - skeletonData.Joints[JointType.ShoulderRight].Position.X;
            v.Y = skeletonData.Joints[JointType.HipRight].Position.Y - skeletonData.Joints[JointType.ShoulderRight].Position.Y;
            v.Z = skeletonData.Joints[JointType.HipRight].Position.Z - skeletonData.Joints[JointType.ShoulderRight].Position.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;

            jointLinks.Add(JointType.HipRight,
                new SkeletonPoint[] { skeletonData.Joints[JointType.HipRight].Position,
                    skeletonData.Joints[JointType.ShoulderRight].Position,
                    skeletonData.Joints[JointType.KneeRight].Position,
                    skeletonData.Joints[JointType.HipRight].Position,
                    z
                });

            ///---->knee left
            z.X = skeletonData.Joints[JointType.KneeLeft].Position.X - skeletonData.Joints[JointType.HipLeft].Position.X;
            z.Y = skeletonData.Joints[JointType.KneeLeft].Position.Y - skeletonData.Joints[JointType.HipLeft].Position.Y;
            z.Z = skeletonData.Joints[JointType.KneeLeft].Position.Z - skeletonData.Joints[JointType.HipLeft].Position.Z;
            jointLinks.Add(JointType.KneeLeft,//only one angle
                new SkeletonPoint[] { skeletonData.Joints[JointType.AnkleLeft].Position,
                    skeletonData.Joints[JointType.KneeLeft].Position,
                    skeletonData.Joints[JointType.KneeLeft].Position,
                    skeletonData.Joints[JointType.HipLeft].Position,
                    z
                });
            ///----> knee right
            z.X = skeletonData.Joints[JointType.KneeRight].Position.X - skeletonData.Joints[JointType.HipRight].Position.X;
            z.Y = skeletonData.Joints[JointType.KneeRight].Position.Y - skeletonData.Joints[JointType.HipRight].Position.Y;
            z.Z = skeletonData.Joints[JointType.KneeRight].Position.Z - skeletonData.Joints[JointType.HipRight].Position.Z;
            jointLinks.Add(JointType.KneeRight,//only one angle
                new SkeletonPoint[] { skeletonData.Joints[JointType.AnkleRight].Position,
                    skeletonData.Joints[JointType.KneeRight].Position,
                    skeletonData.Joints[JointType.KneeRight].Position,
                    skeletonData.Joints[JointType.HipRight].Position,
                    z
                });

            ///---->shoulder left
            h.X = skeletonData.Joints[JointType.HipLeft].Position.X - skeletonData.Joints[JointType.HipRight].Position.X;
            h.Y = skeletonData.Joints[JointType.HipLeft].Position.Y - skeletonData.Joints[JointType.HipRight].Position.Y;
            h.Z = skeletonData.Joints[JointType.HipLeft].Position.Z - skeletonData.Joints[JointType.HipRight].Position.Z;
            v.X = skeletonData.Joints[JointType.HipLeft].Position.X - skeletonData.Joints[JointType.ShoulderLeft].Position.X;
            v.Y = skeletonData.Joints[JointType.HipLeft].Position.Y - skeletonData.Joints[JointType.ShoulderLeft].Position.Y;
            v.Z = skeletonData.Joints[JointType.HipLeft].Position.Z - skeletonData.Joints[JointType.ShoulderLeft].Position.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;
            jointLinks.Add(JointType.ShoulderLeft,
                new SkeletonPoint[]{skeletonData.Joints[JointType.HipLeft].Position,
                    skeletonData.Joints[JointType.ShoulderLeft].Position,
                    skeletonData.Joints[JointType.ElbowLeft].Position,
                    skeletonData.Joints[JointType.ShoulderLeft].Position,
                    z
                });
            ///---->shoulder right
            h.X = skeletonData.Joints[JointType.HipRight].Position.X - skeletonData.Joints[JointType.HipLeft].Position.X;
            h.Y = skeletonData.Joints[JointType.HipRight].Position.Y - skeletonData.Joints[JointType.HipLeft].Position.Y;
            h.Z = skeletonData.Joints[JointType.HipRight].Position.Z - skeletonData.Joints[JointType.HipLeft].Position.Z;
            v.X = skeletonData.Joints[JointType.HipRight].Position.X - skeletonData.Joints[JointType.ShoulderRight].Position.X;//TODO: test hip center shoulder center because hip right is not on the same as shoulder right
            v.Y = skeletonData.Joints[JointType.HipRight].Position.Y - skeletonData.Joints[JointType.ShoulderRight].Position.Y;
            v.Z = skeletonData.Joints[JointType.HipRight].Position.Z - skeletonData.Joints[JointType.ShoulderRight].Position.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;
            jointLinks.Add(JointType.ShoulderRight,
                new SkeletonPoint[]{skeletonData.Joints[JointType.HipRight].Position,
                    skeletonData.Joints[JointType.ShoulderRight].Position,
                    skeletonData.Joints[JointType.ElbowRight].Position,
                    skeletonData.Joints[JointType.ShoulderRight].Position,
                    z
                });

            h.X = skeletonData.Joints[JointType.HipLeft].Position.X - skeletonData.Joints[JointType.HipRight].Position.X;
            h.Y = skeletonData.Joints[JointType.HipLeft].Position.Y - skeletonData.Joints[JointType.HipRight].Position.Y;
            h.Z = skeletonData.Joints[JointType.HipLeft].Position.Z - skeletonData.Joints[JointType.HipRight].Position.Z;
            v.X = skeletonData.Joints[JointType.ShoulderCenter].Position.X - skeletonData.Joints[JointType.HipCenter].Position.X;
            v.Y = skeletonData.Joints[JointType.ShoulderCenter].Position.Y - skeletonData.Joints[JointType.HipCenter].Position.Y;
            v.Z = skeletonData.Joints[JointType.ShoulderCenter].Position.Z - skeletonData.Joints[JointType.HipCenter].Position.Z;
            tmp = Vector3.findOrthonormalVector(new Vector3(h.X, h.Y, h.Z), new Vector3(v.X, v.Y, v.Z));
            z.X = tmp.X;
            z.Y = tmp.Y;
            z.Z = tmp.Z;
            jointLinks.Add(JointType.Head,
                new SkeletonPoint[]{skeletonData.Joints[JointType.HipCenter].Position,
                    skeletonData.Joints[JointType.ShoulderCenter].Position,
                    skeletonData.Joints[JointType.ShoulderCenter].Position,
                    skeletonData.Joints[JointType.Head].Position,
                    z
                });

            ///------> ankle left
            z.X = skeletonData.Joints[JointType.FootLeft].Position.X - skeletonData.Joints[JointType.AnkleLeft].Position.X;
            z.Y = skeletonData.Joints[JointType.FootLeft].Position.Y - skeletonData.Joints[JointType.AnkleLeft].Position.Y;
            z.Z = skeletonData.Joints[JointType.FootLeft].Position.Z - skeletonData.Joints[JointType.AnkleLeft].Position.Z;
            jointLinks.Add(JointType.AnkleLeft,//--> 1 DOF (y,z) remove the 90deg
                new SkeletonPoint[]{skeletonData.Joints[JointType.AnkleLeft].Position,
                    skeletonData.Joints[JointType.KneeLeft].Position,
                    skeletonData.Joints[JointType.AnkleLeft].Position,
                    skeletonData.Joints[JointType.FootLeft].Position,
                    z
                });
            ///----->ankle right
            z.X = skeletonData.Joints[JointType.FootRight].Position.X - skeletonData.Joints[JointType.AnkleRight].Position.X;
            z.Y = skeletonData.Joints[JointType.FootRight].Position.Y - skeletonData.Joints[JointType.AnkleRight].Position.Y;
            z.Z = skeletonData.Joints[JointType.FootRight].Position.Z - skeletonData.Joints[JointType.AnkleRight].Position.Z;
            jointLinks.Add(JointType.AnkleRight,//--> 1 DOF (y,z) remove the 90deg
                new SkeletonPoint[]{skeletonData.Joints[JointType.AnkleRight].Position,
                    skeletonData.Joints[JointType.KneeRight].Position,
                    skeletonData.Joints[JointType.AnkleRight].Position,
                    skeletonData.Joints[JointType.FootRight].Position,
                    z
                });

            ///----->elbow left
            z.X = skeletonData.Joints[JointType.WristLeft].Position.X - skeletonData.Joints[JointType.ElbowLeft].Position.X;
            z.Y = skeletonData.Joints[JointType.WristLeft].Position.Y - skeletonData.Joints[JointType.ElbowLeft].Position.Y;
            z.Z = skeletonData.Joints[JointType.WristLeft].Position.Z - skeletonData.Joints[JointType.ElbowLeft].Position.Z;
            jointLinks.Add(JointType.ElbowLeft,//-->1 DOF (y,z)
                new SkeletonPoint[]{
                    skeletonData.Joints[JointType.ElbowLeft].Position,
                    skeletonData.Joints[JointType.WristLeft].Position,
                    skeletonData.Joints[JointType.ElbowLeft].Position,
                    skeletonData.Joints[JointType.ShoulderLeft].Position,
                    z
                });

            ///----->elbow right
            z.X = skeletonData.Joints[JointType.WristRight].Position.X - skeletonData.Joints[JointType.ElbowRight].Position.X;
            z.Y = skeletonData.Joints[JointType.WristRight].Position.Y - skeletonData.Joints[JointType.ElbowRight].Position.Y;
            z.Z = skeletonData.Joints[JointType.WristRight].Position.Z - skeletonData.Joints[JointType.ElbowRight].Position.Z;
            jointLinks.Add(JointType.ElbowRight,//-->1 DOF (y,z)
                new SkeletonPoint[]{
                    skeletonData.Joints[JointType.ElbowRight].Position,
                    skeletonData.Joints[JointType.WristRight].Position,
                    skeletonData.Joints[JointType.ElbowRight].Position,
                    skeletonData.Joints[JointType.ShoulderRight].Position,
                    z
                });


            ///----->wrist left
            //two angles but we don't know which so, we take the 3 angles->no y angle (works according to vectPrevious)
            z.X = skeletonData.Joints[JointType.WristLeft].Position.X - skeletonData.Joints[JointType.HandLeft].Position.X;
            z.Y = skeletonData.Joints[JointType.WristLeft].Position.Y - skeletonData.Joints[JointType.HandLeft].Position.Y;
            z.Z = skeletonData.Joints[JointType.WristLeft].Position.Z - skeletonData.Joints[JointType.HandLeft].Position.Z;
            jointLinks.Add(JointType.WristLeft,
                new SkeletonPoint[]{
                    skeletonData.Joints[JointType.HandLeft].Position,
                    skeletonData.Joints[JointType.WristLeft].Position,
                    skeletonData.Joints[JointType.WristLeft].Position,
                    skeletonData.Joints[JointType.ElbowLeft].Position,
                    z
                });

            ///--->wrist right
            //two angles but we don't know which so, we take the 3 angles->no y angle (works according to vectPrevious)
            z.X = skeletonData.Joints[JointType.WristRight].Position.X - skeletonData.Joints[JointType.HandRight].Position.X;
            z.Y = skeletonData.Joints[JointType.WristRight].Position.Y - skeletonData.Joints[JointType.HandRight].Position.Y;
            z.Z = skeletonData.Joints[JointType.WristRight].Position.Z - skeletonData.Joints[JointType.HandRight].Position.Z;
            jointLinks.Add(JointType.WristRight,
                new SkeletonPoint[]{
                    skeletonData.Joints[JointType.HandRight].Position,
                    skeletonData.Joints[JointType.WristRight].Position,
                    skeletonData.Joints[JointType.WristRight].Position,
                    skeletonData.Joints[JointType.ElbowRight].Position,
                    z
                });
            
        }

        /// <summary>
        /// Returns the angles for the 14 joints
        /// </summary>
        /// <returns></returns>
        public Hashtable getAllAngles()
        {
            Hashtable angles = new Hashtable();
            angles.Add(JointType.AnkleLeft, getAngles(JointType.AnkleLeft));
            angles.Add(JointType.AnkleRight, getAngles(JointType.AnkleRight));
            angles.Add(JointType.ElbowLeft, getAngles(JointType.ElbowLeft));
            angles.Add(JointType.ElbowRight, getAngles(JointType.ElbowRight));
            angles.Add(JointType.Head, getAngles(JointType.Head));
            angles.Add(JointType.HipCenter, getAngles(JointType.HipCenter));
            angles.Add(JointType.HipLeft, getAngles(JointType.HipLeft));
            angles.Add(JointType.HipRight, getAngles(JointType.HipRight));
            angles.Add(JointType.KneeLeft, getAngles(JointType.KneeLeft));
            angles.Add(JointType.KneeRight, getAngles(JointType.KneeRight));
            angles.Add(JointType.ShoulderLeft, getAngles(JointType.ShoulderLeft));
            angles.Add(JointType.ShoulderRight, getAngles(JointType.ShoulderRight));
            angles.Add(JointType.WristLeft, getAngles(JointType.WristLeft));
            angles.Add(JointType.WristRight, getAngles(JointType.WristRight));
            return angles;
        }

        public Vector3 vectNext;
        public Vector3 vectPrevious;
        /// <summary>
        /// To add the new joint, add the case here and then in the initialization function add the joints' vectors
        /// </summary>
        /// <param name="currentJoint"></param>
        /// <returns></returns>
        public List<double> getAngles(JointType currentJoint)
        {
            var angles = new List<double>();

            Joint firstJoint = data.Joints[JointType.HipCenter];
            Joint secondJoint = data.Joints[JointType.KneeRight];

            switch (currentJoint)
            {
                case JointType.ElbowLeft:
                    firstJoint = data.Joints[JointType.ShoulderLeft];
                    secondJoint = data.Joints[JointType.WristLeft];
                    break;
                case JointType.ElbowRight: 
                    firstJoint = data.Joints[JointType.ShoulderRight];
                    secondJoint = data.Joints[JointType.WristRight];
                    break;
                case JointType.HipLeft: 
                    firstJoint = data.Joints[JointType.HipCenter];
                    secondJoint = data.Joints[JointType.KneeLeft];
                    break;
                case JointType.HipRight: 
                    firstJoint = data.Joints[JointType.HipCenter];
                    secondJoint = data.Joints[JointType.KneeRight];
                    break;
                case JointType.KneeLeft: 
                    firstJoint = data.Joints[JointType.HipLeft];
                    secondJoint = data.Joints[JointType.AnkleLeft];
                    break;
                case JointType.KneeRight: 
                    firstJoint = data.Joints[JointType.HipRight];
                    secondJoint = data.Joints[JointType.AnkleRight];
                    break;
                case JointType.ShoulderLeft:
                    firstJoint = data.Joints[JointType.ShoulderCenter];
                    secondJoint = data.Joints[JointType.ElbowLeft];
                    break;
                case JointType.ShoulderRight: 
                    firstJoint = data.Joints[JointType.ShoulderCenter];
                    secondJoint = data.Joints[JointType.ElbowRight];
                    break;
                case JointType.Spine:
                    firstJoint = data.Joints[JointType.HipCenter];
                    secondJoint = data.Joints[JointType.ShoulderCenter];
                    break;
            }

            angles = GetBodyJointsAngles.GetBodySegmentAngle(firstJoint, data.Joints[currentJoint], secondJoint);

            return angles;
        }

        static int counter = 0;
        /// <summary>
        /// Calculate hip angle
        /// [0]: calculates the (x,y) angle
        /// [1]: calculates the (y,z) angle
        /// [2]: calculates the (x,z) angle
        /// </summary>
        /// <returns>double[]</returns>
        public double[] rotationAngles(JointType currentJoint)
        {
            double[] angles = new double[4];
            if (currentJoint == JointType.ElbowLeft || currentJoint == JointType.ElbowRight
                || currentJoint == JointType.KneeLeft || currentJoint == JointType.KneeRight)
            {
                angles[1] = Vector3.GetAngle(vectNext, vectPrevious);
                return angles;
            }

            SkeletonPoint[] _obj = (SkeletonPoint[])jointLinks[currentJoint];
            SkeletonPoint zAxis = new SkeletonPoint();

            if (_obj.Length == 5)
            {
                zAxis = (SkeletonPoint)(_obj[4]);//the axis for measuring the rotation around the y-axis
            }

            double angle;
            Matrix3 rotationMatrixY = new Matrix3();
            if (_obj.Length == 5)
            {
                angle = angles[3] = Vector2.signedAngle(new Vector2(0, 1), new Vector2(zAxis.X, zAxis.Z));
                rotationMatrixY = RotationMatrix.GetRotationMatrix3Y(angle * Math.PI / 180);
            }
            //find the rotation according to the vertical axis
            angle = Vector2.signedAngle(new Vector2(1, 0), new Vector2(vectPrevious.Y, vectPrevious.Z));
            Matrix3 rotationMatrixX = RotationMatrix.GetRotationMatrix3X(angle * Math.PI / 180);

            angle = Vector2.signedAngle(new Vector2(0, 1), new Vector2(vectPrevious.X, vectPrevious.Y));
            Matrix3 rotationMatrixZ = RotationMatrix.GetRotationMatrix3Z(angle * Math.PI / 180);

            if (_obj.Length == 5)
            {
                vectNext = rotationMatrixY * vectNext;
            }
            vectNext = (rotationMatrixZ * (rotationMatrixX * vectNext));

            angles[0] = Vector2.signedAngle(new Vector2(0, 1), new Vector2(vectNext.X, vectNext.Y));
            angles[1] = Vector2.signedAngle(new Vector2(1, 0), new Vector2(vectNext.Y, vectNext.Z));
            angles[2] = Vector2.signedAngle(new Vector2(0, 1), new Vector2(vectNext.X, vectNext.Z));

            
            counter++;

            return angles;
        }

		public void AssignAnglesToSkeleton(Skeleton skeleton)
		{
			Hashtable angles = getAllAngles();

			foreach (JointType key in angles.Keys)
			{
				List<double> anglesInDouble = (List<double>)angles[key];

				SkeletonPoint point = new SkeletonPoint();

				if (!double.IsNaN(anglesInDouble[0]))
				{
					point.X = (int)anglesInDouble[0];
				}
				if (!double.IsNaN(anglesInDouble[1]))
				{
					point.Y = (int)anglesInDouble[1];
				}
				if (!double.IsNaN(anglesInDouble[2]))
				{
					point.Z = (int)anglesInDouble[2];
				}

				var joint = skeleton.Joints[key];
				joint.Position = point;
				skeleton.Joints[key] = joint;
			}
		}
	}
}
