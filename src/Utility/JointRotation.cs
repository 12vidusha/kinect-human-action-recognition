using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public class JointRotation
	{
		public JointRotation()
		{
			X = 0;
			Y = 0;
			Z = 0;
			W = 1;
		}

		public JointRotation(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float W { get; set; }
	}
}
