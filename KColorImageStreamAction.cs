using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Microsoft.Kinect;

namespace KinectServer
{
    class KColorImageStreamAction : KAction
    {
        public KColorImageStreamAction(NetworkStream arg)
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
                rData += (byte)sensor.ColorStream.Format;

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
                rData += sensor.ColorStream.NominalDiagonalFieldOfView;
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
                rData += sensor.ColorStream.NominalFocalLengthInPixels;
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
                rData += sensor.ColorStream.NominalHorizontalFieldOfView;
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
                rData += sensor.ColorStream.NominalInverseFocalLengthInPixels;
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
                rData += sensor.ColorStream.NominalVerticalFieldOfView;
                rData = rData.Replace(',', '.');

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
                Console.WriteLine("enable" + args);
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));

                sensor.ColorStream.Enable((ColorImageFormat)int.Parse(args[1]));

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
