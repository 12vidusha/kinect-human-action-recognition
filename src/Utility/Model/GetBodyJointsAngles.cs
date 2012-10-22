using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows;
using MatrixVector;

namespace Utility.Model
{
    public class GetBodyJointsAngles
    {
        static public List<double> GetBodySegmentAngle(Joint aStartJoint, Joint aMiddleJoint, Joint aEndJoint)
        {

			Joint startJoint = aStartJoint;
			Joint middleJoint = aMiddleJoint;
			Joint endJoint = aEndJoint;
			List<double> result = new List<double>();

			//XY
			Vector V = new Vector(startJoint.Position.X - middleJoint.Position.X,
									startJoint.Position.Y - middleJoint.Position.Y);

			Vector Q = new Vector(middleJoint.Position.X - endJoint.Position.X,
									middleJoint.Position.Y - endJoint.Position.Y);

			result.Add(Vector.AngleBetween(V, Q));

			//XZ
			V = new Vector(startJoint.Position.X - middleJoint.Position.X,
							startJoint.Position.Z - middleJoint.Position.Z);

			Q = new Vector(middleJoint.Position.X - endJoint.Position.X,
							middleJoint.Position.Z - endJoint.Position.Z);

			result.Add(Vector.AngleBetween(V, Q));

			//YZ
			V = new Vector(startJoint.Position.Y - middleJoint.Position.Y,
							startJoint.Position.Z - middleJoint.Position.Z);

			Q = new Vector(middleJoint.Position.Y - endJoint.Position.Y,
							middleJoint.Position.Z - endJoint.Position.Z);

			result.Add(Vector.AngleBetween(V, Q));

			
			
            return result;
        }
    }
}
