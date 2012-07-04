using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace KinectServer
{
    abstract class KServerPaquet : KPaquet
    {
        protected const uint headerLength = 9;

        public override uint headerSize()
        {
            return headerLength;
        }

        protected override void buildHeader()
        {
            /* Paquet body size */
            byte[] byteArray = BitConverter.GetBytes(bodySize());
            Array.Reverse(byteArray);
            Buffer.BlockCopy(byteArray, 0, data, 0, byteArray.Length);

            /* timestamp */
            byteArray = BitConverter.GetBytes((UInt32)0);
            Array.Reverse(byteArray);
            Buffer.BlockCopy(byteArray, 0, data, 4, byteArray.Length);

            /* id */
            data[8] = id();

        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        protected abstract byte id();

        public void send(NetworkStream stream)
        {
            if (!stream.CanWrite)
                throw new Exception("the stream cannot write");
                
            stream.Write(data, 0, data.Length);
        }
    }
}
