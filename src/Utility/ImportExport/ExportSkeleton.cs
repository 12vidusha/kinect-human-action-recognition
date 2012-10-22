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
	public class RecordSkeleton
	{
		public static double Count = 0;
		private int framesCount = 0;
		DateTime startTime; bool TimeRecorded = false;

		/// <summary>
		/// Writes to the files
		/// </summary>
		StreamWriter streamWriterXmlAngles;
		/// <summary>
		/// the folder to export to
		/// </summary>
		string exportFolder = null;
		string exportFile = null;

		public RecordSkeleton()
		{
			exportFolder = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Export");
			if (!Directory.Exists(exportFolder))
			{
				Directory.CreateDirectory(exportFolder);
			}

			streamWriterXmlAngles = new StreamWriter(exportFile + "recording.xml", false);
			streamWriterXmlAngles.WriteLine("<action>");
		}

		public void StopRecording()
		{
			framesCount = 0;
			streamWriterXmlAngles.WriteLine("</action>");
			streamWriterXmlAngles.Close();
		}

		public void AnglesExportToXML(ImportedSkeleton skeleton)
		{
			if (!TimeRecorded)
			{
				startTime = DateTime.Now;
				TimeRecorded = true;
			}

			
			streamWriterXmlAngles.WriteLine("<skeleton>");
			streamWriterXmlAngles.WriteLine("<frame>" + framesCount + "</frame>");

			foreach (var joint in skeleton.Quaterions)
			{
				streamWriterXmlAngles.Write("<" + joint.Key + ">");

				streamWriterXmlAngles.Write("<angles>");
				streamWriterXmlAngles.Write("<Qx>" + joint.Value.X + "</Qx>");
				streamWriterXmlAngles.Write("<Qy>" + joint.Value.Y + "</Qy>");
				streamWriterXmlAngles.Write("<Qz>" + joint.Value.Z + "</Qz>");
				streamWriterXmlAngles.Write("<Qw>" + joint.Value.W + "</Qw>");
				streamWriterXmlAngles.Write("</angles>");

				streamWriterXmlAngles.Write("<positions>");
				streamWriterXmlAngles.Write("<x>" + skeleton.Joints[joint.Key].Position.X + "</x>");
				streamWriterXmlAngles.Write("<y>" + skeleton.Joints[joint.Key].Position.Y + "</y>");
				streamWriterXmlAngles.Write("<z>" + skeleton.Joints[joint.Key].Position.Z + "</z>");
				streamWriterXmlAngles.Write("</positions>");

				streamWriterXmlAngles.WriteLine("</" + joint.Key + ">");
			}

			framesCount++;
			streamWriterXmlAngles.WriteLine("</skeleton>");

			streamWriterXmlAngles.WriteLine();
		}
	}
}
