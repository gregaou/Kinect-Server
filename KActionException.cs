using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectServer
{
    class KActionException : Exception
    {
        public byte exceptionNumber { get; private set; }

        public KActionException(object[] error)
            : base((string)error[1])
        {
            exceptionNumber = (byte)int.Parse((string)error[0]);
        }
    }
}
