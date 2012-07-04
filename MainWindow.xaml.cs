using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace KinectServer
{
    public partial class MainWindow : Window
    {
        private KinectSensor sensor;
        private RenderTargetBitmap renderer;
        private const double dpiX = 96, dpiY = 96;

        /* ColorFrame */
        private ColorImageFormat lastImageFormat = ColorImageFormat.Undefined;
        private Byte[] pixelData;
        private WriteableBitmap outputColorImage;
        private static readonly int Bgr32BytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /* SkeletonFrame */
        private Skeleton[] skeletonData;
        private DrawingVisual drawingVisual;
        private const double jointThickness = 3;
        private static readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private static readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /* Server */
        KTcp KinectTcp;

        public MainWindow()
        {
            InitializeComponent();
            //printEnums();
        }

        private void printEnums()
        {
            System.Console.WriteLine(
                (byte)SkeletonTrackingState.NotTracked              + " " + 
                (byte)SkeletonTrackingState.PositionOnly            + " " + 
                (byte)SkeletonTrackingState.Tracked);

            System.Console.WriteLine(
                (byte)JointTrackingState.NotTracked                 + " " + 
                (byte)JointTrackingState.Inferred                   + " " + 
                (byte)JointTrackingState.Tracked);

            System.Console.WriteLine(
                (byte)KinectStatus.Undefined                        + " " +
                (byte)KinectStatus.Disconnected                     + " " +
                (byte)KinectStatus.Connected                        + " " +
                (byte)KinectStatus.Initializing                     + " " +
                (byte)KinectStatus.Error                            + " " +
                (byte)KinectStatus.NotPowered                       + " " +
                (byte)KinectStatus.NotReady                         + " " +
                (byte)KinectStatus.DeviceNotGenuine                 + " " +
                (byte)KinectStatus.DeviceNotSupported               + " " +
                (byte)KinectStatus.InsufficientBandwidth);
        }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            //*
            KinectTcp = new KTcp();
            
            drawingVisual = new DrawingVisual();

            foreach (KinectSensor potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    sensor = potentialSensor;
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    sensor.SkeletonStream.Enable();
                    sensor.DepthStream.Enable();
                    sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(allFramesReady);
                    sensor.Start();
                    return;
                }
            }
            //*/

            /* Pas de kinect connectée */
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void allFramesReady(Object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
                processColorFrame(imageFrame);
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                processSkeletonFrame(skeletonFrame);
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
                processDepthFrame(depthFrame);
        }

        private void processColorFrame(ColorImageFrame imageFrame)
        {
            if (imageFrame == null)
                return;

            bool newFormat = (lastImageFormat != imageFrame.Format);

            if (newFormat)
                pixelData = new Byte[imageFrame.PixelDataLength];

            imageFrame.CopyPixelDataTo(pixelData);
            /*
            KServerPaquet paquet = new KServerColorStreamPaquet(pixelData, imageFrame.Width * Bgr32BytesPerPixel);
            //paquet.send();
            /*/
            if (newFormat)
            {
                video.Visibility = Visibility.Visible;
                outputColorImage = new WriteableBitmap(
                    imageFrame.Width,
                    imageFrame.Height,
                    dpiX,
                    dpiY,
                    PixelFormats.Bgr32,
                    null);
                renderer = new RenderTargetBitmap((int)outputColorImage.Width, (int)outputColorImage.Height, dpiX, dpiY, PixelFormats.Pbgra32);
                video.Source = renderer;
            }

            outputColorImage.WritePixels(
                new Int32Rect(0, 0, imageFrame.Width, imageFrame.Height),
                pixelData,
                imageFrame.Width * Bgr32BytesPerPixel,
                0);
            //*/
            
            lastImageFormat = imageFrame.Format;
        }

        private void processSkeletonFrame(SkeletonFrame skeletonFrame)
        {
            if (skeletonFrame == null || renderer == null)
                return;

            if (skeletonData == null || skeletonData.Length != skeletonFrame.SkeletonArrayLength)
                skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

            skeletonFrame.CopySkeletonDataTo(skeletonData);
            /*
            KServerPaquet paquet = new KServerSkeletonStreamPaquet(sensor, skeletonData);
            //paquet.send();
            /*/
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawImage(outputColorImage, new Rect(0, 0, outputColorImage.Width, outputColorImage.Height));

                foreach (Skeleton skeleton in skeletonData)
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        drawSkeleton(skeleton, dc);
            }

            renderer.Render(drawingVisual);
            //*/
        }

        private void processDepthFrame(DepthImageFrame depthFrame)
        {
        }

        private void drawSkeleton(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.drawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.drawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.drawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.drawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.drawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.drawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.drawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.drawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.drawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.drawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.drawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.drawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.drawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // Left Leg
            this.drawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.drawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.drawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.drawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.drawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.drawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Point p = pointToScreen(joint.Position);

                if (pointInScreen(p))// && joint.TrackingState == JointTrackingState.Tracked)
                    drawingContext.DrawEllipse(trackedJointBrush, null, p, jointThickness, jointThickness);
            }
        }

        private void drawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            if (joint0.TrackingState != JointTrackingState.Tracked || joint1.TrackingState != JointTrackingState.Tracked)
                return;

            Point p0 = pointToScreen(joint0.Position), p1 = pointToScreen(joint1.Position);

            if (pointInScreen(p0) && pointInScreen(p1))
                drawingContext.DrawLine(trackedBonePen, pointToScreen(joint0.Position), pointToScreen(joint1.Position));
        }

        private Point pointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = sensor.MapSkeletonPointToDepth(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private bool pointInScreen(Point p)
        {
            if (outputColorImage == null)
                return true;
            return pointInScreen(p, (int)outputColorImage.Width, (int)outputColorImage.Height);
        }

        private bool pointInScreen(Point p, int w, int h)
        {
            return p.X >= 0 && p.X < w && p.Y >= 0 && p.Y < h;
        }
    }
}
