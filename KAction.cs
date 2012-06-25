using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectServer
{
    public abstract class KAction
    {

        protected KinectSensorCollection sensors;

        public string rData { get; protected set; }

        public KAction()
        {
            sensors = KinectSensor.KinectSensors;
        }

        protected bool sensorsIsNull()
        {
            return (sensors.Equals(null));
        }


        //public byte exec();

    }
}
