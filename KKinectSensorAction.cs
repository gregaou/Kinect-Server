using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Net;

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

        private KinectSensor sensor;
        /*
        protected byte getQuery(string member, string[] args, int[] n)
        {
            try
            {
                verifArgs(n, args);
                getKinectSensor(int.Parse(args[0]));
                return getQuery(sensor.GetType(), sensor, member, args, n);
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        protected byte getQuery(string member, string[] args, int n)
        {
            byte ret = getQuery(member, args, new int[] {n});
            KActionException op = new KActionException(KError.InvalidOperation);
            KActionException run = new KActionException(KError.SensorMustRunning);

            if (ret == op.exceptionNumber)
            {
                rData = run.Message;
                return run.exceptionNumber;
            }

            return ret;
        }
        */

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
            /*
            return getQuery("ElevationAngle", args, 1);
            /*/
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
            //*/
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
            /*
            byte ret = getQuery("Status", args, 1);

            KinectStatus status = (KinectStatus)Enum.Parse(typeof(KinectStatus), rData);
            rData = ((byte)status).ToString();

            Console.WriteLine(rData);

            return ret;
            /*/
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
            //*/
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

                Console.WriteLine("fin fonction");

                byte[] tab = new byte[4*tabSize*2];
                byte[] tmp = new byte[4];

                for (int i = 0; i < 2; i++)
                {
                    ColorImagePoint p = colorCoordinates[i];
                    int x = IPAddress.HostToNetworkOrder(p.X);
                    int y = IPAddress.HostToNetworkOrder(p.Y);

                    Console.WriteLine("(" + p.X + ", " + p.Y + ") "); 

                    tmp = BitConverter.GetBytes(p.X);
                    Buffer.BlockCopy(tmp, 0, tab, 4*i*2, 4);
                    tmp = BitConverter.GetBytes(p.Y);
                    Buffer.BlockCopy(tmp, 0, tab, 4*(i*2+1), 4);
                }

                Console.WriteLine("Fin boucle");

                rData = KClient.ByteArrayToStr(tab);

                Console.WriteLine("Fin conversion");

                /*
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
                string[] tab = rData.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("fin boucle : " + tab.Length);
                Console.WriteLine(rData.Length);
                Console.WriteLine(colorCoordinates[0].X);
                Console.WriteLine(colorCoordinates[tabSize - 1].Y);
                */

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
                    Console.WriteLine("start");
                    sensor.Start();
                    Console.WriteLine("Start 2");
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
    }
}
