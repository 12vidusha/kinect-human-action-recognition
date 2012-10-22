using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public delegate void PoseRecognizedEventHandler(object o, PoseRecognizedEventArgs e);

	public class PoseRecognizedEventArgs : EventArgs
	{
		public double Result;

		public PoseRecognizedEventArgs(double result)
		{
			Result = result;
		}
	}
}
