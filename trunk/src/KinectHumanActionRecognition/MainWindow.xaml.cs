//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
	using System.IO;
	using System.Windows;
	using System.Windows.Media;
	using Microsoft.Kinect;
	using Microsoft.Win32;
	using Utility.ImportExport;
	using System.Linq;
	using System.Collections;
	using Utility;
	using System;
	using Utility;
	using System.Xml;
	using System.Collections.Generic;
	using Utility.Model;
	using System.Globalization;
	using Core;
	using System.Threading;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Properties

		#region Rendering

		/// <summary>
		/// Width of output drawing
		/// </summary>
		private const float RenderWidth = 640.0f;

		/// <summary>
		/// Height of our output drawing
		/// </summary>
		private const float RenderHeight = 480.0f;

		/// <summary>
		/// Thickness of drawn joint lines
		/// </summary>
		private const double JointThickness = 3;

		/// <summary>
		/// Thickness of body center ellipse
		/// </summary>
		private const double BodyCenterThickness = 10;

		/// <summary>
		/// Thickness of clip edge rectangles
		/// </summary>
		private const double ClipBoundsThickness = 10;

		/// <summary>
		/// Brush used to draw skeleton center point
		/// </summary>
		private readonly Brush centerPointBrush = Brushes.Blue;

		/// <summary>
		/// Brush used for drawing joints that are currently tracked
		/// </summary>
		private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

		/// <summary>
		/// Brush used for drawing joints that are currently inferred
		/// </summary>        
		private readonly Brush inferredJointBrush = Brushes.Yellow;

		/// <summary>
		/// Pen used for drawing bones that are currently tracked
		/// </summary>
		private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

		/// <summary>
		/// Pen used for drawing bones that are currently inferred
		/// </summary>        
		private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

		/// <summary>
		/// Active Kinect sensor
		/// </summary>
		private KinectSensor sensor;

		/// <summary>
		/// Drawing group for skeleton rendering output
		/// </summary>
		private DrawingGroup drawingGroup;

		/// <summary>
		/// Drawing image that we will display
		/// </summary>
		private DrawingImage imageSource;

		#endregion

		#region Recording, importing Xml and evaluating joints

		/// <summary>
		/// Recorded skeleton
		/// </summary>
		private RecordSkeleton recordSkeleton = null;

		/// <summary>
		/// The number of skeletons supported to be detected from the Kinect
		/// </summary>
		private const int skeletonCount = 6;
		/// <summary>
		/// Stores all the skeletons
		/// </summary>
		private Skeleton[] allSkeletons = new Skeleton[skeletonCount];

		public List<Skeleton> recordedAction = new List<Skeleton>();

		#endregion

		#endregion

		#region Functions

		#region Rendering

		/// <summary>
		/// Draws indicators to show which edges are clipping skeleton data
		/// </summary>
		/// <param name="skeleton">skeleton to draw clipping information for</param>
		/// <param name="drawingContext">drawing context to draw to</param>
		private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
		{
			if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
			{
				drawingContext.DrawRectangle(
					Brushes.Red,
					null,
					new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
			}

			if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
			{
				drawingContext.DrawRectangle(
					Brushes.Red,
					null,
					new Rect(0, 0, RenderWidth, ClipBoundsThickness));
			}

			if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
			{
				drawingContext.DrawRectangle(
					Brushes.Red,
					null,
					new Rect(0, 0, ClipBoundsThickness, RenderHeight));
			}

			if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
			{
				drawingContext.DrawRectangle(
					Brushes.Red,
					null,
					new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
			}
		}
		/// <summary>
		/// Draws a skeleton's bones and joints
		/// </summary>
		/// <param name="skeleton">skeleton to draw</param>
		/// <param name="drawingContext">drawing context to draw to</param>
		private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
		{
			// Render Torso
			this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
			this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
			this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
			this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
			this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
			this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
			this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

			// Left Arm
			this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
			this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
			this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

			// Right Arm
			this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
			this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
			this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

			// Left Leg
			this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
			this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
			this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

			// Right Leg
			this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
			this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
			this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

			// Render Joints
			foreach (Joint joint in skeleton.Joints)
			{
				Brush drawBrush = null;

				if (joint.TrackingState == JointTrackingState.Tracked)
				{
					drawBrush = this.trackedJointBrush;
				}
				else if (joint.TrackingState == JointTrackingState.Inferred)
				{
					drawBrush = this.inferredJointBrush;
				}

				if (drawBrush != null)
				{
					drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
				}
			}
		}

		/// <summary>
		/// Maps a SkeletonPoint to lie within our render space and converts to Point
		/// </summary>
		/// <param name="skelpoint">point to map</param>
		/// <returns>mapped point</returns>
		private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
		{
			// Convert point to depth space.  
			// We are not using depth directly, but we do want the points in our 640x480 output resolution.
			DepthImagePoint depthPoint = this.sensor.MapSkeletonPointToDepth(
																			 skelpoint,
																			 DepthImageFormat.Resolution640x480Fps30);
			return new Point(depthPoint.X, depthPoint.Y);
		}

		/// <summary>
		/// Draws a bone line between two joints
		/// </summary>
		/// <param name="skeleton">skeleton to draw bones from</param>
		/// <param name="drawingContext">drawing context to draw to</param>
		/// <param name="jointType0">joint to start drawing from</param>
		/// <param name="jointType1">joint to end drawing at</param>
		private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
		{
			Joint joint0 = skeleton.Joints[jointType0];
			Joint joint1 = skeleton.Joints[jointType1];

			// If we can't find either of these joints, exit
			if (joint0.TrackingState == JointTrackingState.NotTracked ||
				joint1.TrackingState == JointTrackingState.NotTracked)
			{
				return;
			}

			// Don't draw if both points are inferred
			if (joint0.TrackingState == JointTrackingState.Inferred ||
				joint1.TrackingState == JointTrackingState.Inferred)
			{
				return;
			}

			// We assume all drawn bones are inferred unless BOTH joints are tracked
			Pen drawPen = this.inferredBonePen;
			if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
			{
				drawPen = this.trackedBonePen;
			}

			drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
		}

		#endregion

		#region UI, initialization WPF & Kinect events handlers

		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		Core recognitionCore;
		List<Activity> Activities = new List<Activity>();

		/// <summary>
		/// Execute startup tasks
		/// </summary>
		/// <param name="sender">object sending the event</param>
		/// <param name="e">event arguments</param>
		private void WindowLoaded(object sender, RoutedEventArgs e)
		{

			// Create the drawing group we'll use for drawing
			this.drawingGroup = new DrawingGroup();

			// Create an image source that we can use in our image control
			this.imageSource = new DrawingImage(this.drawingGroup);

			// Display the drawing using our image control
			Image.Source = this.imageSource;

			// Look through all sensors and start the first connected one.
			// This requires that a Kinect is connected at the time of app startup.
			// To make your app robust against plug/unplug, 
			// it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit
			foreach (var potentialSensor in KinectSensor.KinectSensors)
			{
				if (potentialSensor.Status == KinectStatus.Connected)
				{
					this.sensor = potentialSensor;
					break;
				}
			}

			if (null != this.sensor)
			{
				// Turn on the skeleton stream to receive skeleton frames
				this.sensor.SkeletonStream.Enable();

				// Add an event handler to be called whenever there is new color frame data
				this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

				// Start the sensor!
				try
				{
					this.sensor.Start();
					recognitionCore = new Core(sensor);
					recognitionCore.ActivityRecognizingStarted += new ActivityRecognizingEventHandler(recognitionCore_ActivityRecognizingStarted);
					recognitionCore.ActivityRecognizingEnded += new ActivityRecognizingEventHandler(recognitionCore_ActivityRecognizingEnded);
				}
				catch (IOException)
				{
					this.sensor = null;
				}
			}

			if (null == this.sensor)
			{
				this.statusBarText.Text = KinectHumanActionRecognition.Properties.Resources.NoKinectReady;
			}
			sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(recognitionCore.AllFramesReady);
			sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinectSensor_AllFramesReady);
			recognitionCore.PoseReconized += new PoseRecognizedEventHandler(recognitionCore_PoseReconized);
		}

		void recognitionCore_ActivityRecognizingStarted(object o, ActivityRecognizingEventArgs e)
		{
			SimilarityDetectedLabel.Text = e.Activity.Name;
			SimilarityDetectedLabel.Visibility = System.Windows.Visibility.Visible;
		}

		void recognitionCore_ActivityRecognizingEnded(object o, ActivityRecognizingEventArgs e)
		{
			SimilarityDetectedLabel.Visibility = System.Windows.Visibility.Hidden;
		}

		void recognitionCore_PoseReconized(object sender, PoseRecognizedEventArgs e)
		{
			poseRecognitionResult = e.Result;
			toDrawSuccessText = true;
		}

		private bool toDrawSuccessText = false;

		/// <summary>
		/// Execute shutdown tasks
		/// </summary>
		/// <param name="sender">object sending the event</param>
		/// <param name="e">event arguments</param>
		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (null != this.sensor)
			{
				this.sensor.Stop();
			}
		}

		/// <summary>
		/// Event handler for Kinect sensor's SkeletonFrameReady event
		/// </summary>
		/// <param name="sender">object sending the event</param>
		/// <param name="e">event arguments</param>
		private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
		{
			Skeleton[] skeletons = new Skeleton[0];

			using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
			{
				if (skeletonFrame != null)
				{
					skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
					skeletonFrame.CopySkeletonDataTo(skeletons);
				}
			}

			using (DrawingContext dc = this.drawingGroup.Open())
			{
				//if (toDrawSuccessText)
				//{
				//    SimilarityDetectedLabel.Visibility = System.Windows.Visibility.Visible;
				//    SimilarityDetectedLabel.Text = "Similarity detected:" + poseRecognitionResult.ToString();
				//    toDrawSuccessText = false;
				//}
				//else
				//{
				//    SimilarityDetectedLabel.Visibility = System.Windows.Visibility.Hidden;
				//}

				// Draw a transparent background to set the render size
				dc.DrawRectangle(Brushes.White, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

				if (skeletons.Length != 0)
				{
					foreach (Skeleton skel in skeletons)
					{
						RenderClippedEdges(skel, dc);

						if (skel.TrackingState == SkeletonTrackingState.Tracked)
						{
							this.DrawBonesAndJoints(skel, dc);
						}
						else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
						{
							dc.DrawEllipse(
							this.centerPointBrush,
							null,
							this.SkeletonPointToScreen(skel.Position),
							BodyCenterThickness,
							BodyCenterThickness);
						}
					}
				}

				// prevent drawing outside of our render area
				this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
			}
		}

		/// <summary>
		/// Handles the checking or unchecking of the seated mode combo box
		/// </summary>
		/// <param name="sender">object sending the event</param>
		/// <param name="e">event arguments</param>
		private void CheckBoxSeatedModeChanged(object sender, RoutedEventArgs e)
		{
			if (null != this.sensor)
			{
				if (this.checkBoxSeatedMode.IsChecked.GetValueOrDefault())
				{
					this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
				}
				else
				{
					this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
				}
			}
		}

		#endregion

		#region Recording, importing Xml and evaluating joints

		RecordingStates recordingState = RecordingStates.Pending;

		private void buttonStartRecordingToXml_Click(object sender, RoutedEventArgs e)
		{
			if (recordingState == RecordingStates.Recording) return;
			recordSkeleton = null;
			recordingState = RecordingStates.Recording;

			if (recordSkeleton == null)
			{
				recordSkeleton = new Utility.ImportExport.RecordSkeleton();
			}
		}

		private void buttonStopRecordingToXml_Click(object sender, RoutedEventArgs e)
		{
			if (recordingState == RecordingStates.Recording)
			{
				recordingState = RecordingStates.Stopping;
				HandleRecordingState(null);
			}
		}

		/// <summary>
		/// Handles the states of the recording engine. See RecordingStates.
		/// </summary>
		/// <param name="data">Skeleton to be saved</param>
		void HandleRecordingState(ImportedSkeleton data)
		{
			if (recordingState == RecordingStates.Stopping)
			{
				if (recordSkeleton != null)
				{
					recordSkeleton.StopRecording();
					recordingState = RecordingStates.Pending;
				}
				return;
			}
			if (recordingState == RecordingStates.Pending) return;

			var jointManager = new JointAnglesManager(data);

			var skeleton = jointManager.GetComputedAngles(data);
			recordSkeleton.AnglesExportToXML(skeleton);
		}

		private List<ImportedSkeleton> mSkeletonCollection = new List<ImportedSkeleton>();

		private void buttonLoadXml_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openXml = new OpenFileDialog();
			openXml.ShowDialog();

			ImportSkeleton importSkeletonManager = new ImportSkeleton();
			importSkeletonManager.ImportAction(openXml.FileName);

			mSkeletonCollection = importSkeletonManager.SkeletonCollection;
		}

		Skeleton mainSkeleton = new Skeleton();

		private void buttonCompareSkeletons_Click(object sender, RoutedEventArgs e)
		{
			recognitionCore.CurrentMode = Mode.ComparingSkeletons;
		}

		#endregion

		private void buttonSelectMainSkeleton_Click(object sender, RoutedEventArgs e)
		{
			recognitionCore.CurrentMode = Mode.ToSelectMainSkeleton;
		}

		#endregion

		private double poseRecognitionResult { get; set; }

		public void kinectSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
		{
			var skeleton = GetFirstSkeleton.Get(e, allSkeletons);
			if (skeleton != null)
			{
				var skeletonWithAngles = new ImportedSkeleton(skeleton);
				HandleRecordingState(skeletonWithAngles);
			}
		}

		List<ImportedSkeleton> mFirstAction;
		List<ImportedSkeleton> mSecondAction;

		private void buttonLoadFirstAction_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openXml = new OpenFileDialog();
			openXml.ShowDialog();

			try
			{
				ImportSkeleton importSkeletonManager = new ImportSkeleton();
				importSkeletonManager.ImportAction(openXml.FileName);

				mFirstAction = importSkeletonManager.SkeletonCollection;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void buttonLoadSecondAction_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openXml = new OpenFileDialog();
			openXml.ShowDialog();

			try
			{
				ImportSkeleton importSkeletonManager = new ImportSkeleton();
				importSkeletonManager.ImportAction(openXml.FileName);

				mSecondAction = importSkeletonManager.SkeletonCollection;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private void buttonCompareActions_Click(object sender, RoutedEventArgs e)
		{
			double result = recognitionCore.DTW(new ActivityRecord(mFirstAction), new ActivityWindow(mSecondAction));
			Console.WriteLine(result);
		}

		private void buttonStartRecognizing_Click(object sender, RoutedEventArgs e)
		{
			if (mFirstAction != null)
			{
				var firstActivity = new Activity("first");
				firstActivity.Recordings.Add(new ActivityRecord(mFirstAction));

				Activities.Add(firstActivity);
			}
			 
			


			recognitionCore.LoadTrainedData(Activities);

			recognitionCore.CurrentMode = Mode.FillingWindow;
		}

		private void buttonLoadActionSet_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();

			openFile.ShowDialog();

			List<Activity> tempActivities = new List<Activity>();

			try
			{
				using (var streamReader = new StreamReader(openFile.FileName))
				{
					string line = "";

					while ((line = streamReader.ReadLine()) != null)
					{
						ImportSkeleton action = new ImportSkeleton();
						action.ImportAction(line);
						List<ImportedSkeleton> skeleton = action.SkeletonCollection;

						Activity currentActivity = new Activity(Path.GetFileNameWithoutExtension(line));
						currentActivity.Recordings.Add(new ActivityRecord(action.SkeletonCollection));

						tempActivities.Add(currentActivity);
					}

					if (line != "")
					{
						Activities = tempActivities;

					}
				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}
		}
	}
}