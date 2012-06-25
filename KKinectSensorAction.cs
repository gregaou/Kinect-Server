using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectServer
{
    class KKinectSensorAction : KAction
    {

        public KKinectSensorAction() : base()
        {
        }

        public byte Status(string[] args)
        {
            byte status;

            if (args.Length != 1)
                throw new Exception("Wrong args number !");

            KinectSensor sensor;
            int id = int.Parse(args[0]);

            //if (id + 1 > sensors.Count)
            //{
            //    rData = "Bad Index";
            //    return KError.StatusKinectSensorError;
            //}


            try
            {
                sensor = sensors[id];
            }
            catch (ArgumentOutOfRangeException e)
            {
                rData = "Bad Index !";
                return KError.StatusKinectSensorError;
            }

            status = (byte)sensor.Status;
            //rData = status.ToString(); 

            return KSuccess.StatusKinectSensorSucess;
        }

    }
}
