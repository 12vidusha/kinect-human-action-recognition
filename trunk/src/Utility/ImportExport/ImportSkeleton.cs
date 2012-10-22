using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Xml;

namespace Utility.ImportExport
{
	public class ImportSkeleton
	{
		public List<ImportedSkeleton> SkeletonCollection = new List<ImportedSkeleton>();

		public List<ImportedSkeleton> ImportAction(string xmlFilepath)
		{
			var xmlReadingState = ReadingXmlState.Skeleton;

			int currentSkeletonID = 0;
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
							SkeletonCollection.Add(new ImportedSkeleton());
						}
						else if (xmlReader.Name == "frame")
						{
							xmlReadingState = ReadingXmlState.Frame;
						}
						else if (xmlReader.Name == "angles")
						{
							xmlReadingState = ReadingXmlState.Quaterions;
						}
						else if (xmlReader.Name == "position")
						{
							xmlReadingState = ReadingXmlState.Position;
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
						else if (xmlReader.Name == "Qx")
						{
							xmlReadingState = ReadingXmlState.Qx;
						}
						else if (xmlReader.Name == "Qy")
						{
							xmlReadingState = ReadingXmlState.Qy;
						}
						else if (xmlReader.Name == "Qz")
						{
							xmlReadingState = ReadingXmlState.Qz;
						}
						else if (xmlReader.Name == "Qw")
						{
							xmlReadingState = ReadingXmlState.Qw;
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
								float x = 0;
								if (float.TryParse(xmlReader.Value, out x))
								{
									SkeletonPoint pointX;
									CopyAllPositionAtributes(out pointX, currentSkeletonID, currentJointType);
									pointX.X = x;

									var jointX = SkeletonCollection[currentSkeletonID].Joints[currentJointType];
									jointX.Position = pointX;
									SkeletonCollection[currentSkeletonID].Joints[currentJointType] = jointX;
								}

								break;
							case ReadingXmlState.y:
								float y = 0;
								if (float.TryParse(xmlReader.Value, out y))
								{
									SkeletonPoint pointY;
									CopyAllPositionAtributes(out pointY, currentSkeletonID, currentJointType);
									pointY.Y = y;

									var jointY = SkeletonCollection[currentSkeletonID].Joints[currentJointType];
									jointY.Position = pointY;
									SkeletonCollection[currentSkeletonID].Joints[currentJointType] = jointY;
								}

								break;
							case ReadingXmlState.z:
								float z = 0;
								if (float.TryParse(xmlReader.Value, out z))
								{
									SkeletonPoint pointZ;
									CopyAllPositionAtributes(out pointZ, currentSkeletonID, currentJointType);
									pointZ.Z = z;

									var jointZ = SkeletonCollection[currentSkeletonID].Joints[currentJointType];
									jointZ.Position = pointZ;
									SkeletonCollection[currentSkeletonID].Joints[currentJointType] = jointZ;
								}
								break;

							case ReadingXmlState.Qx:
								float qx = 0;
								if (float.TryParse(xmlReader.Value, out qx))
								{
									SkeletonCollection[currentSkeletonID].Quaterions[currentJointType].X = qx;
								}
								break;

							case ReadingXmlState.Qy:

								float qy = 0;
								if (float.TryParse(xmlReader.Value, out qy))
								{
									SkeletonCollection[currentSkeletonID].Quaterions[currentJointType].Y = qy;
								}

								break;

							case ReadingXmlState.Qz:

								float qz = 0;
								if (float.TryParse(xmlReader.Value, out qz))
								{
									SkeletonCollection[currentSkeletonID].Quaterions[currentJointType].Z = qz;
								}

								break;

							case ReadingXmlState.Qw:

								float qw = 0;
								if (float.TryParse(xmlReader.Value, out qw))
								{
									SkeletonCollection[currentSkeletonID].Quaterions[currentJointType].W = qw;
								}

								break;


							case ReadingXmlState.Joint:
								//nothing, just waiting for the angles data
								break;
							case ReadingXmlState.Frame:
								int.TryParse(xmlReader.Value.ToString(), out currentSkeletonID);
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
					Console.WriteLine("Position: ");
					Console.WriteLine("\tX: " + SkeletonCollection[i].Joints[type].Position.X);
					Console.WriteLine("\tY: " + SkeletonCollection[i].Joints[type].Position.Y);
					Console.WriteLine("\tZ: " + SkeletonCollection[i].Joints[type].Position.Z);
					Console.WriteLine("Angles: ");
					Console.WriteLine("\tX: " + SkeletonCollection[i].Quaterions[type].X);
					Console.WriteLine("\tY: " + SkeletonCollection[i].Quaterions[type].Y);
					Console.WriteLine("\tZ: " + SkeletonCollection[i].Quaterions[type].Z);
					Console.WriteLine("\tW: " + SkeletonCollection[i].Quaterions[type].W);
					Console.WriteLine();
				}
			}
		}
	}
}
