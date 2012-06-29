using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Net.Sockets;
using System.Threading;

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

        public byte getStatus(string[] args)
        {
            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                byte status = (byte)sensor.Status;
                rData = status.ToString();

                Console.WriteLine(sensor.DeviceConnectionId);

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
                rData = "";
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

    }
}
