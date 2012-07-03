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

        protected byte getQuery(Type type, object obj, string member, string[] args, int[] n)
        {
            try
            {
                verifArgs(n, args);

                try
                {
                    PropertyInfo property = type.GetProperty(member);
                    object value = property.GetValue(obj, null);
                    rData = value.ToString();
                    return KSuccess.QueryOk;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Invalid operation");
                    throw new KActionException(KError.SensorMustRunning);
                }
                catch (Exception e)
                {
                    Console.WriteLine("other exception");
                    throw new KActionException(new object[] { "142", "getQuery" });
                }
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }
        }

        protected byte getQuery(Type type, object obj, string member, string[] args, int n)
        {
            return getQuery(type, obj, member, args, new int[] { n });
        }

        //public byte exec();
    }
}
