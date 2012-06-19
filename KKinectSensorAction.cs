using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectServer
{
    class KKinectSensorAction : KAction
    {
        private KinectSensorCollection KSensorCol;
        
        
        public byte Status(object[] args){
            if (args.Length != 1)
                return (byte)KErrors.err.ERROR_NB_ARGS;
        }
    }
}
