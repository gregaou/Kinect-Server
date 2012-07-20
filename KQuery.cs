using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;

namespace KinectServer
{
    class KQuery
    {

        private const string sep = "||";
        
        private string str_query;

        private NetworkStream ns;

        public string classname { get; private set; }

        public string methodname { get; private set; }

        public string[] args { get; private set; }

        public KAction action { get; private set; }

        public KQuery(string _str, NetworkStream _ns)
        {
            str_query = _str;
            ns = _ns;
        }

        public byte process()
        {
            string[] tmp;

            MethodInfo mInfo;
            Type objType;
           
            tmp = str_query.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (tmp.Length < 2)
                throw new KActionException(KError.WrongQuery);

            classname = tmp[0];
            methodname = tmp[1];

            args = new string[tmp.Length - 2];
            for (int i = 2; i < tmp.Length; ++i)
            {
                args[i - 2] = tmp[i];
            }
                
            objType = Type.GetType("KinectServer.K" + classname + "Action");

            Console.WriteLine("KinectServer.K" + classname + "Action");
            try
            {
                action = (KAction)Activator.CreateInstance(objType, new object[] { ns });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            mInfo = action.GetType().GetMethod(methodname);
            Console.WriteLine("KinectServer.K" + classname + "Action");
            
            return (byte)mInfo.Invoke(action, new object[] { args });
    }





    }
}
