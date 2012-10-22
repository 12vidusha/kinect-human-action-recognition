using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Xml;

namespace SkeletalTracking.Utility.ImportExport
{
    public class ImportSkeleton
    {
        public List<Skeleton> SkeletonCollection = new List<Skeleton>();

        public List<Skeleton> ImportAction(string xmlFilepath)
        {
            var xmlReadingState = ReadingXmlState.Skeleton;

            int currentID = 0;
            JointType currentJointType = JointType.AnkleRight;

            XmlTextReader xmlReader = new XmlTextReader(xmlFilepath);
            while (xmlReader.Read())
            {
                xmlReader.MoveToElement();

                if (xmlReader.NodeType != XmlNodeType.EndElement)
                {
                    #region Managing reading states
                    if (xmlReader.Name != string.Empty)
                    {
                        if (xmlReader.Name == "skeleton")
                        {
                            xmlReadingState = ReadingXmlState.Skeleton;
                            SkeletonCollection.Add(new Skeleton());
                        }
                        else if (xmlReader.Name == "recordedSampleID")
                        {
                            xmlReadingState = ReadingXmlState.RecordedSampleID;
                        }
                        else if (xmlReader.Name == "x")
                        {
                            xmlReadingState = ReadingXmlState.x;
                        }
                        else if (xmlReader.Name == "y")
                        {
                            xmlReadingState = ReadingXmlState.y;
                        }
                        else if (xmlReader.Name == "z")
                        {
                            xmlReadingState = ReadingXmlState.z;
                        }
                        else
                        {
                            foreach (var joint in Enum.GetValues(typeof(JointType)))
                            {
                                if (xmlReader.Name == joint.ToString())
                                {
                                    currentJointType = (JointType)Enum.Parse(typeof(JointType), joint.ToString());
                                }

                                xmlReadingState = ReadingXmlState.Joint;
                            }
                        }
                    #endregion
                    }
                    else if (xmlReader.Value != "\r\n" && xmlReader.Value != string.Empty)
                    {
                        switch (xmlReadingState)
                        {
                            case ReadingXmlState.x:
                                int x = 0;

                                int.TryParse(xmlReader.Value, out x);

                                SkeletonPoint pointX;
                                CopyAllPositionAtributes(out pointX, currentID, currentJointType);
                                pointX.X = x;

                                var jointX = SkeletonCollection[currentID].Joints[currentJointType];
                                jointX.Position = pointX;
                                SkeletonCollection[currentID].Joints[currentJointType] = jointX;

                                break;
                            case ReadingXmlState.y:
                                int y = 0;
                                int.TryParse(xmlReader.Value, out y);

                                SkeletonPoint pointY;
                                CopyAllPositionAtributes(out pointY, currentID, currentJointType);
                                pointY.Y = y;

                                var jointY = SkeletonCollection[currentID].Joints[currentJointType];
                                jointY.Position = pointY;
                                SkeletonCollection[currentID].Joints[currentJointType] = jointY;

                                break;
                            case ReadingXmlState.z:
                                int z = 0;
                                int.TryParse(xmlReader.Value, out z);

                                SkeletonPoint pointZ;
                                CopyAllPositionAtributes(out pointZ, currentID, currentJointType);
                                pointZ.Z = z;

                                var jointZ = SkeletonCollection[currentID].Joints[currentJointType];
                                jointZ.Position = pointZ;
                                SkeletonCollection[currentID].Joints[currentJointType] = jointZ;

                                break;
                            case ReadingXmlState.Joint:
                                //nothing, just waiting for the angles data
                                break;
                            case ReadingXmlState.RecordedSampleID:
                                int.TryParse(xmlReader.Value.ToString(), out currentID);
                                break;
                            case ReadingXmlState.Skeleton:
                                //recordedAction.Add(new Skeleton());
                                break;
                        }
                    }
                }
            }

            return SkeletonCollection;
        }

        private void CopyAllPositionAtributes(out SkeletonPoint point, int currentID, JointType currentJointType)
        {
            point = new SkeletonPoint();
            point.X = SkeletonCollection[currentID].Joints[currentJointType].Position.X;
            point.Y = SkeletonCollection[currentID].Joints[currentJointType].Position.Y;
            point.Z = SkeletonCollection[currentID].Joints[currentJointType].Position.Z;
        }

        public void PrintDebugData()
        {
            Console.WriteLine("SkeletonAction.Count: " + SkeletonCollection.Count);

            for (int i = 0; i < SkeletonCollection.Count; i++)
            {
                foreach (var joint in Enum.GetNames(typeof(JointType)))
                {
                    JointType type = (JointType)Enum.Parse(typeof(JointType), joint);

                    Console.WriteLine(SkeletonCollection[i].Joints[type].JointType.ToString());
                    Console.WriteLine("X: " + SkeletonCollection[i].Joints[type].Position.X);
                    Console.WriteLine("Y: " + SkeletonCollection[i].Joints[type].Position.Y);
                    Console.WriteLine("Z: " + SkeletonCollection[i].Joints[type].Position.Z);
                    Console.WriteLine();
                }
            }
        }
    }
}
