using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.IO;
using System.Threading;
using System.Collections;

namespace Utility.ImportExport
{
	public class SkeletonRecordingManager
	{
		public static double Count = 0;
		private int framesCount = 0;
		DateTime startTime; bool TimeRecorded = false;

		/// <summary>
		/// XMLWriter
		/// </summary>
		StreamWriter streamWriterXmlAngles;

		List<ImportedSkeleton> recordedSkeletonCollection = new List<ImportedSkeleton>();

		public SkeletonRecordingManager()
		{

			streamWriterXmlAngles = new StreamWriter("recording.xml", false);
			streamWriterXmlAngles.WriteLine("<action>");
		}

		public void StopRecording()
		{
			List<JointType> SMIJ = new List<JointType>();

			SMIJ = MostInformativeJointsSelector.GetJoints(recordedSkeletonCollection, recordedSkeletonCollection.Count, QuaternionsStyles.Absolute);

			foreach (var recordedSkeleton in recordedSkeletonCollection)
			{
				AnglesExportToXML(recordedSkeleton, SMIJ);
			}


			framesCount = 0;
			streamWriterXmlAngles.WriteLine("</action>");
			streamWriterXmlAngles.Close();
		}

		public void AddSkeletonToTheRecord(Skeleton skeleton)
		{
			recordedSkeletonCollection.Add(new ImportedSkeleton(skeleton));
		}

		public void AnglesExportToXML(ImportedSkeleton skeleton, List<JointType> SMIJ)
		{
			if (!TimeRecorded)
			{
				startTime = DateTime.Now;
				TimeRecorded = true;
			}

			
			streamWriterXmlAngles.WriteLine("<skeleton>");
			streamWriterXmlAngles.WriteLine("<frame>" + framesCount + "</frame>");

			foreach (var joint in SMIJ)
			{
				streamWriterXmlAngles.Write("<" + joint + ">");

				streamWriterXmlAngles.Write("<angles>");
				streamWriterXmlAngles.Write("<Qx>" + skeleton.HiararchicalQuaternions[joint].X + "</Qx>");
				streamWriterXmlAngles.Write("<Qy>" + skeleton.HiararchicalQuaternions[joint].Y + "</Qy>");
				streamWriterXmlAngles.Write("<Qz>" + skeleton.HiararchicalQuaternions[joint].Z + "</Qz>");
				streamWriterXmlAngles.Write("<Qw>" + skeleton.HiararchicalQuaternions[joint].W + "</Qw>");
				streamWriterXmlAngles.Write("</angles>");

				streamWriterXmlAngles.Write("<positions>");
				streamWriterXmlAngles.Write("<x>" + skeleton.Joints[joint].Position.X + "</x>");
				streamWriterXmlAngles.Write("<y>" + skeleton.Joints[joint].Position.Y + "</y>");
				streamWriterXmlAngles.Write("<z>" + skeleton.Joints[joint].Position.Z + "</z>");
				streamWriterXmlAngles.Write("</positions>");

				streamWriterXmlAngles.WriteLine("</" + joint + ">");
			}

			framesCount++;
			streamWriterXmlAngles.WriteLine("</skeleton>");

			streamWriterXmlAngles.WriteLine();
		}
	}
}
