using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Net.Sockets;

namespace KinectServer
{
    class KKinectSensorCollectionAction : KAction
    {
        
        public KKinectSensorCollectionAction(NetworkStream arg)
            : base(arg)
        {
        }

        public byte Count(string[] args)
        {
            try
            {
                verifArgs(0, args);
                rData += sensors.Count;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
            return KSuccess.QueryOk;
        }

        public byte StatusChanged(string[] args)
        {
            try
            {
                
                verifArgs(0, args);
                Console.WriteLine("kikoo1");
                sensors.StatusChanged += KinectSensorsStatusChanged;
                Console.WriteLine("kikoo2");
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        void KinectSensorsStatusChanged(object sender, StatusChangedEventArgs e)
        {
            KinectSensor k;
            KinectStatus s;
            Console.WriteLine("kikoo22");
            k = e.Sensor;
            s = e.Status;
            int id=-1;

            for (int i = 0; i < KinectSensor.KinectSensors.Count; i++)
            {
                if (KinectSensor.KinectSensors[i].Equals(k))
                {
                    id = (byte)i;
                    break;
                }
            }


            KServerPaquet sp = new KServerMessagePaquet(200, "1||1");
            sp.send(ns);

        } 

    }
}
