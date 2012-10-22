using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
	public class Activity
	{
		public Activity(string aName)
		{
			Name = aName;
			Recordings = new List<ActivityRecord>();
		}

		public string Name { get; set; }
		public List<ActivityRecord> Recordings;
	}
}
