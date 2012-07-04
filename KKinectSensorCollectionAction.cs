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
                sensors.StatusChanged += KinectSensorsStatusChanged;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public void KinectSensorsStatusChanged(object sender, StatusChangedEventArgs e)
        {
            try
            {
                KServerPaquet sp = new KServerMessagePaquet(200, "1||" + (byte)e.Status);
                sp.send(ns);
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("event statusChanged disconnected : " + exc.Message);
                sensors.StatusChanged -= KinectSensorsStatusChanged;
            }
        } 
    }
}
