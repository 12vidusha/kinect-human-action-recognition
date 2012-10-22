using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.IO;
using System.Threading;
using System.Collections;

namespace SkeletalTracking.Utility.Export
{
    public class RecordSkeleton
    {
        public static double Count = 0;
        double[] recordedSamples = new double[4];
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
        public bool running=false;

        public RecordSkeleton()
        {
            exportFolder = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Export");
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            streamWriterXmlAngles = new StreamWriter(exportFile + "_angle.xml", false);
            streamWriterXmlAngles.WriteLine("<action>");
        }
                
        public void StopRecording()
        {
            streamWriterXmlAngles.WriteLine("</action>");
            streamWriterXmlAngles.Close();
        }

        public void AnglesExportToXML(Hashtable data)
        {
            if (!TimeRecorded)
            {
                startTime = DateTime.Now;
                TimeRecorded = true;
            }
            recordedSamples[2]++;
            streamWriterXmlAngles.WriteLine("<skeleton>");
            double a = DateTime.Now.TimeOfDay.TotalSeconds - startTime.TimeOfDay.TotalSeconds;
            int id = (int)recordedSamples[2];
            id -= 1;
            streamWriterXmlAngles.WriteLine("<recordedSampleID>" + id + "</recordedSampleID>");
            foreach (JointType key in data.Keys)
            {
                double[] b = (double[])data[key];
                streamWriterXmlAngles.Write("<" + key.ToString() + ">");
                if (!double.IsNaN(b[0])){
                    streamWriterXmlAngles.Write("<x>" + (int)b[0] + "</x>");
                }
                if (!double.IsNaN(b[1])){
                    streamWriterXmlAngles.Write("<y>" + (int)b[1] + "</y>");
                }
                if (!double.IsNaN(b[2])){
                    streamWriterXmlAngles.Write("<z>" + (int)b[2] + "</z>");
                }
                streamWriterXmlAngles.WriteLine("</" + key.ToString() + ">");
            }
            streamWriterXmlAngles.WriteLine("</skeleton>");
            streamWriterXmlAngles.WriteLine();
        }
    }
}
