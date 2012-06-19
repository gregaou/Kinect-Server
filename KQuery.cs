using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace KinectServer
{
    class KQuery
    {

        private const string sep = "||";
        
        private string str_query;

        public string classname { get; private set; }

        public string methodname { get; private set; }

        public string[] args { get; private set; }

        public KAction action { get; private set; }

        public KQuery(string _str)
        {
            str_query = _str;
            
        }

        public void process()
        {
            string[] tmp;

            tmp = str_query.Split(sep.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

            if (tmp.Length < 2) 
                throw new Exception("Wrong Query");

            classname = tmp[0];
            methodname = tmp[1];

            args = new string[tmp.Length-2];
            for (int i = 2; i < tmp.Length; ++i)
            {
                 args[i-2] = tmp[i];
            }

            
            action = (KAction)Activator.CreateInstance(Type.GetType("K"+classname+"Action"));
            MethodInfo mInfo = typeof(KAction).GetMethod(methodname);

            mInfo.Invoke(action,args);

            
        }





    }
}
