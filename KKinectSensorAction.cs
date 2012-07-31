using System;
using Microsoft.Kinect;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Text;

namespace KinectServer
{
    class KKinectSensorAction : KAction
    {

        public KKinectSensorAction(NetworkStream arg)
            : base(arg)
        {
            
        }

        private void getKinectSensor(int id){
            try
            {
                sensor = sensors[id];
            }
            catch (Exception e)
            {
                throw new KActionException(KError.ArgumentOutOfRange);
            }
        }

        private Thread audioThread;
        private Stream audioStream;
        private bool isReading = false;
        private KinectSensor sensor;
        /// <summary>
        /// Number of milliseconds between each read of audio data from the stream.
        /// </summary>
        private const int AudioPollingInterval = 50;

        /// <summary>
        /// Number of samples captured from Kinect audio stream each millisecond.
        /// </summary>
        private const int SamplesPerMillisecond = 16;

        /// <summary>
        /// Number of bytes in each Kinect audio stream sample.
        /// </summary>
        private const int BytesPerSample = 2;

        /// <summary>
        /// Buffer used to hold audio data read from audio stream.
        /// </summary>
        private readonly byte[] audioBuffer = new byte[AudioPollingInterval * SamplesPerMillisecond * BytesPerSample];

        public byte getDeviceConnectionId(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));

                try
                {
                    rData = sensor.DeviceConnectionId;
                }
                catch (InvalidOperationException e)
                {
                    throw new KActionException(KError.SensorMustRunning);
                }

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getElevationAngle(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                try
                {
                    rData += sensor.ElevationAngle;
                }
                catch (InvalidOperationException e)
                {
                    throw new KActionException(KError.SensorMustRunning);
                }

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte setElevationAngle(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                try
                {
                    sensor.ElevationAngle = int.Parse(args[1]);
                }
                catch (InvalidOperationException e)
                {
                    if (!sensor.IsRunning)
                        throw new KActionException(KError.SensorMustRunning);

                    throw new KActionException(KError.InvalidOperation);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new KActionException(KError.ArgumentOutOfRange);
                }

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte isRunning(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData = (sensor.IsRunning ? "1" : "0");

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        // Exceptions ??
        public byte getMinElevationAngle(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.MinElevationAngle;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        // Exceptions ??
        public byte getMaxElevationAngle(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.MaxElevationAngle;
                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getStatus(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));

                try
                {
                    byte status = (byte)sensor.Status;
                    rData = status.ToString();
                }
                catch (InvalidOperationException e)
                {
                    throw new KActionException(KError.SensorMustRunning);
                }

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getUniqueKinectId(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.UniqueKinectId;
                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte dispose(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.Dispose();
                Console.WriteLine("dispose");
                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte MapDepthFrameToColorFrame(string[] args)
        {
            try
            {
                int size = int.Parse(args[1]);
                verifArgs(size + 1, args);

                getKinectSensor(int.Parse(args[0]));

                int tabSize = size - 3;

                DepthImageFormat depthImageFormat = (DepthImageFormat)int.Parse(args[2]);
                short[] depthPixelData = new short[tabSize];
                for (int i = 0; i < tabSize; i++)
                    depthPixelData[i] = short.Parse(args[i + 2]);
                ColorImageFormat colorImageFormat = (ColorImageFormat)int.Parse(args[3 + tabSize]);
                ColorImagePoint[] colorCoordinates = new ColorImagePoint[tabSize];

                Console.WriteLine(colorImageFormat);

                sensor.MapDepthFrameToColorFrame(
                      depthImageFormat,
                      depthPixelData,
                      colorImageFormat,
                      colorCoordinates);

                StringBuilder sb = new StringBuilder();

                for (int i=0; i<tabSize; i++)
                {
                    if (i > 0)
                        sb.Append("||");
                    ColorImagePoint p = colorCoordinates[i];
                    sb.Append(p.X);
                    sb.Append("||");
                    sb.Append(p.Y);
                }

                rData = sb.ToString();
                
                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte MapDepthToColorImagePoint(string[] args)
        {
            try
            {
                verifArgs(6, args);
                getKinectSensor(int.Parse(args[0]));

                DepthImageFormat depthImageFormat = (DepthImageFormat)int.Parse(args[1]);
                int depthX = int.Parse(args[2]);
                int depthY = int.Parse(args[3]);
                short depthPixelValue = short.Parse(args[4]);
                ColorImageFormat colorImageFormat = (ColorImageFormat)int.Parse(args[5]);

                ColorImagePoint point = sensor.MapDepthToColorImagePoint(
                    depthImageFormat,
                    depthX,
                    depthY,
                    depthPixelValue,
                    colorImageFormat);

                rData = point.X + "||" + point.Y;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte MapDepthToSkeletonPoint(string[] args)
        {
            try
            {
                verifArgs(5, args);
                getKinectSensor(int.Parse(args[0]));

                DepthImageFormat depthImageFormat = (DepthImageFormat)int.Parse(args[1]);
                int depthX = int.Parse(args[2]);
                int depthY = int.Parse(args[3]);
                short depthPixelValue = short.Parse(args[4]);

                SkeletonPoint point = sensor.MapDepthToSkeletonPoint(
                    depthImageFormat,
                    depthX,
                    depthY,
                    depthPixelValue);

                rData = point.X + "||" + point.Y + "||" + point.Z;
                rData = rData.Replace(',', '.');
                Console.WriteLine(rData);

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte MapSkeletonPointToColor(string[] args)
        {
            try
            {
                verifArgs(5, args);
                getKinectSensor(int.Parse(args[0]));

                SkeletonPoint skeletonPoint = new SkeletonPoint();
                skeletonPoint.X = float.Parse(args[1].Replace('.', ','));
                skeletonPoint.Y = float.Parse(args[2].Replace('.', ','));
                skeletonPoint.Z = float.Parse(args[3].Replace('.', ','));
                ColorImageFormat colorImageFormat = (ColorImageFormat)int.Parse(args[4]);

                ColorImagePoint point = sensor.MapSkeletonPointToColor(skeletonPoint, colorImageFormat);

                rData = point.X + "||" + point.Y;
                Console.WriteLine(rData);

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte MapSkeletonPointToDepth(string[] args)
        {
            try
            {
                verifArgs(5, args);
                getKinectSensor(int.Parse(args[0]));

                SkeletonPoint skeletonPoint = new SkeletonPoint();
                skeletonPoint.X = float.Parse(args[1].Replace('.', ','));
                skeletonPoint.Y = float.Parse(args[2].Replace('.', ','));
                skeletonPoint.Z = float.Parse(args[3].Replace('.', ','));
                DepthImageFormat depthImageFormat = (DepthImageFormat)int.Parse(args[4]);

                DepthImagePoint point = sensor.MapSkeletonPointToDepth(skeletonPoint, depthImageFormat);

                rData = point.X + "||" + point.Y + "||"  + point.Depth + "||" + point.PlayerIndex;
                Console.WriteLine(rData);

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte start(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));

                try
                {
                    sensor.Start();
                    sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                }
                catch (IOException e)
                {
                    throw new KActionException(KError.AlreadyInUse);
                }

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }
       
        // Exceptions ??
        public byte stop(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.Stop();

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte ColorFrameReady(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.ColorFrameReady += KinectSensorColorFrameReady;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public void KinectSensorColorFrameReady(Object Sender, ColorImageFrameReadyEventArgs e)
        {


            try
            {
                using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
                {
                    KServerPaquet sp = new KServerColorStreamPaquet(imageFrame, getIdSensor(sensor));
                    sp.send(ns);
                }
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("event ColorFrameReady disconnected : " + exc.Message);
                sensor.ColorFrameReady -= KinectSensorColorFrameReady;
            }
        }

        public byte DepthFrameReady(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.DepthFrameReady += KinectSensorDepthFrameReady;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public void KinectSensorDepthFrameReady(Object Sender, DepthImageFrameReadyEventArgs e)
        {
            try
            {
                using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
                {
                    KServerPaquet sp = new KServerDepthStreamPaquet(depthFrame, getIdSensor(sensor));
                    sp.send(ns);
                }
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("event DepthFrameReady disconnected : " + exc.Message);
                sensor.DepthFrameReady -= KinectSensorDepthFrameReady;
            }
        }

        public byte AudioDataReady(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                audioStream = sensor.AudioSource.Start();
                isReading = true;
                audioThread = new Thread(AudioReadingThread);
                audioThread.Start();
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        private void AudioReadingThread()
        {
            while (isReading)
            {
                int readCount = audioStream.Read(audioBuffer, 0, audioBuffer.Length);
                try
                {
                    KServerPaquet sp = new KServerAudioStreamPaquet(audioBuffer, getIdSensor(sensor));
                    sp.send(ns);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isReading = false;
                }
            }
        }

        private int getIdSensor(KinectSensor s)
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                if (s.Equals(sensors[i]))
                    return i;
            }
            return -1;
        }
    }
}
