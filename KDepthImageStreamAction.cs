using System;
using System.Net.Sockets;
using Microsoft.Kinect;

namespace KinectServer
{
    class KDepthImageStreamAction : KAction
    {

        public KDepthImageStreamAction(NetworkStream arg)
            : base(arg)
        {}

        private KinectSensor sensor;

        private void getKinectSensor(int id)
        {
            try
            {
                sensor = sensors[id];
            }
            catch (Exception e)
            {
                throw new KActionException(KError.ArgumentOutOfRange);
            }
        }

        public byte getFormat(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (byte)sensor.DepthStream.Format;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getMaxDepth(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (int)sensor.DepthStream.MaxDepth;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getMinDepth(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (int)sensor.DepthStream.MinDepth;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getNominalDiagonalFieldOfView(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.NominalDiagonalFieldOfView;
                rData = rData.Replace(',', '.');

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getNominalFocalLengthInPixels(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.NominalFocalLengthInPixels;
                rData = rData.Replace(',', '.');

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }


        public byte getNominalHorizontalFieldOfView(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.NominalHorizontalFieldOfView;
                rData = rData.Replace(',', '.');

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getNominalInverseFocalLengthInPixels(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.NominalInverseFocalLengthInPixels;
                rData = rData.Replace(',', '.');

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getNominalVerticalFieldOfView(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.NominalVerticalFieldOfView;
                rData = rData.Replace(',', '.');

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getRange(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (byte)sensor.DepthStream.Range;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }


        public byte setRange(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.DepthStream.Range = (DepthRange)int.Parse(args[1]);

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }


        public byte getTooFarDepth(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.TooFarDepth;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getTooNearDepth(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.TooNearDepth;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        public byte getUnknownDepth(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.DepthStream.UnknownDepth;

                return KSuccess.QueryOk;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }


        public byte enable(string[] args)
        {
            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));

                sensor.DepthStream.Enable((DepthImageFormat)int.Parse(args[1]));

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
