using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Utility.Model;
using System.Collections;
using Utility;

namespace Core
{
	public class Core
	{
		private const int SKELETON_COUNT = 6;
		private const double ACCEPTABLE_SKELETON_SIMILARITY = 0.1;
		private const int ACCEPTABLE_WINDOW_SIZE = 30;
		private const double ACCEPTABLE_ACTION_SIMILARITY = 1000;

		private KinectSensor kinectSensor;

		private Skeleton[] allSkeletons = new Skeleton[SKELETON_COUNT];

		public Mode CurrentMode { get; set; }

		private Skeleton mainSkeleton = new Skeleton();

		private ImportedSkeleton mainSkeletonWithAngles;

		List<Activity> Activities = new List<Activity>();

		ActivityWindow window = new ActivityWindow(ACCEPTABLE_WINDOW_SIZE);

		public event PoseRecognizedEventHandler PoseReconized;

		public event ActivityRecognizingEventHandler ActivityRecognizingStarted;
		public event ActivityRecognizingEventHandler ActivityRecognizingEnded;

		public Core(KinectSensor aKinectSensor)
		{
			kinectSensor = aKinectSensor;

			CurrentMode = Mode.None;
		}

		public void LoadTrainedData(List<Activity> aActivities)
		{
			Activities = aActivities;
		}

		Vector4 shoulderLeft;

		public int currentFrame = 0;

		bool activityRecognizingStartedTriggered = false;
		bool activityRecognizingEndedTriggered = false;

		string recognizedActivityName = "";



		public void AllFramesReady(object sender, AllFramesReadyEventArgs e)
		{
			currentFrame++;

			using (DepthImageFrame depth = e.OpenDepthImageFrame())
			{
				#region Checks
				#region First
				if (kinectSensor == null)
				{
					return;
				}
				GC.Collect();
				#endregion

				Skeleton first = Utility.GetFirstSkeleton.Get(e, allSkeletons);

				#region Second
				if (first == null)
				{
					return;
				}
				#endregion
				#endregion

				if (CurrentMode == Mode.FillingWindow)
				{
					JointAnglesManager jointManager = new JointAnglesManager(first);
					mainSkeletonWithAngles = jointManager.GetComputedAngles(first);

					window.Add(mainSkeletonWithAngles, ACCEPTABLE_WINDOW_SIZE);

					if (window.Size >= ACCEPTABLE_WINDOW_SIZE)
					{
						CurrentMode = Mode.AlgorithmRunning;
					}
				}

				if (CurrentMode == Mode.AlgorithmRunning)
				{
					if (currentFrame % 2 == 0)
					{
						foreach (var activity in Activities)
						{
							double overallResult = 0.0;
							foreach (var activityRecord in activity.Recordings)
							{
								var activityRecordResult = DTW(activityRecord, window);
								overallResult += activityRecordResult;
							}

							overallResult /= activity.Recordings.Count;

							Console.WriteLine(overallResult);

							if (overallResult <= ACCEPTABLE_ACTION_SIMILARITY && !activityRecognizingStartedTriggered)
							{
								activityRecognizingStartedTriggered = true;
								activityRecognizingEndedTriggered = false;
								ActivityRecognizingStarted.Invoke(this, new ActivityRecognizingEventArgs(activity, overallResult));
								recognizedActivityName = activity.Name;
							}
							else if (overallResult > ACCEPTABLE_ACTION_SIMILARITY && !activityRecognizingEndedTriggered && recognizedActivityName == activity.Name)
							{
								activityRecognizingEndedTriggered = true;
								activityRecognizingStartedTriggered = false;
								ActivityRecognizingEnded.Invoke(this, new ActivityRecognizingEventArgs(activity, overallResult));
							}

						}
						Console.WriteLine();
					}
					CurrentMode = Mode.FillingWindow;
				}

				if (CurrentMode == Mode.ToSelectMainSkeleton)
				{
					CurrentMode = Mode.None;

					mainSkeleton = first;

					JointAnglesManager jointManager = new JointAnglesManager(first);
					mainSkeletonWithAngles = jointManager.GetComputedAngles(mainSkeleton);
				}

				if (CurrentMode == Mode.ComparingSkeletons)
				{

					///Debug data - Most informative joints
					List<JointType> joints = new List<JointType>();
					joints.Add(JointType.ShoulderLeft);
					joints.Add(JointType.ShoulderRight);
					joints.Add(JointType.Head);
					joints.Add(JointType.ElbowLeft);
					joints.Add(JointType.ElbowRight);

					///

					Skeleton secondarySkeleton = first;

					var jointManager = new JointAnglesManager(secondarySkeleton);
					var secondarySkeletonWithAngles = jointManager.GetComputedAngles(secondarySkeleton);

					var result = SkeletonComparer.Compare(mainSkeletonWithAngles, secondarySkeletonWithAngles, joints);

					Console.WriteLine("Skeleton comparison: {0}", result);

					if (result < ACCEPTABLE_SKELETON_SIMILARITY)
					{
						PoseRecognizedEventArgs args = new PoseRecognizedEventArgs(result);
						PoseReconized.Invoke(this, args);
					}


					//Vector4 shoulder = first.BoneOrientations[JointType.ElbowLeft].HierarchicalRotation.Quaternion;

					//var s = first.BoneOrientations[JointType.ElbowLeft].StartJoint;
					//var end = first.BoneOrientations[JointType.ElbowLeft].EndJoint;

					//var c = shoulder.X * shoulderLeft.X +
					//    shoulder.Y * shoulderLeft.Y +
					//    shoulder.Z * shoulderLeft.Z +
					//    shoulder.Z * shoulderLeft.Z;

					//Console.WriteLine(c);

					//Console.WriteLine(Math.Cos(c));

					//Console.WriteLine(Math.Pow((Math.Cos(c) * 100), 2));

					//Console.WriteLine(Math.Pow((c * 100), 2));

					//Console.WriteLine(Math.Acos(c) * 2.0f);

					//Console.WriteLine();

					//Console.WriteLine();

					//Console.WriteLine();
				}
			}
		}

		public double DTW(ActivityRecord record, ActivityWindow window)
		{
			return (double)DynamicTimeWarping.CompareActivities(record, window);
		}
	}
}
