using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Net.Sockets;
using System.Reflection;

namespace KinectServer
{
    public abstract class KAction
    {

        protected KinectSensorCollection sensors;
        protected NetworkStream ns;

        public string rData { get; protected set; }

        public KAction(NetworkStream arg)
        {
            sensors = KinectSensor.KinectSensors;
            ns = arg;
            rData = "";
        }

        /*
         *  Vérification des Argument  
         */

        protected void verifArgs(int[] n, string[] args)
        {
            
            for (int i = 0; i < n.Length; i++)
            {
                if (n[i] == args.Length)
                    return;
            }
            throw new KActionException(KError.WrongNumberArguments);
        }

        protected void verifArgs(int n, string[] args)
        {
            int[] tab = new int[1];
            tab[0] = n;
            verifArgs(tab, args);
        }

        //public byte exec();
    }
}
