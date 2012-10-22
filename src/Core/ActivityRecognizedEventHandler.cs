using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public delegate void ActivityRecognizingEventHandler(object o, ActivityRecognizingEventArgs e);

	public class ActivityRecognizingEventArgs : EventArgs
	{
		public Activity Activity;
		public double Confidence;

		public ActivityRecognizingEventArgs(Activity aActivity, double aConfidence)
		{
			Activity = aActivity;
			Confidence = aConfidence;
		}
	}
}
