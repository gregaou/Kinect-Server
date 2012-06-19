using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace KinectServer
{
    class KClientPaquet : KPaquet
    {
        private const uint headerLength = 8;
        NetworkStream stream;

        public KClientPaquet(NetworkStream stream) 
        {
            this.stream = stream;
            build();
        }

        public override uint headerSize()
        {
            return headerLength;
        }

        protected override void buildHeader()
        {
            readBuffer(data, 0, (int)headerSize());
            System.Console.WriteLine("taille reçue : " + bodySize());
            setBodySize(bodySize());
        }

        protected override void buildBody()
        {
            readBuffer(data, (int)headerSize(), (int)bodySize());
        }

        private void readBuffer(byte[] buffer, int start, int length)
        {
            int r;

            if (!stream.CanRead)
                throw new Exception("the stream cannot read");

            r = stream.Read(buffer, start, length);

            if (r == 0)
                throw new Exception("connexion lost");

            if (r != length)
                throw new Exception("couldn't read all the paquet (" + r + "/" + length + " bytes read)");
        }

        public string getQuery()
        {
            return ByteArrayToStr(data, headerSize(), bodySize());
        }

        private static string ByteArrayToStr(byte[] bArr, uint index, uint count)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(bArr, (int)index, (int)count);
        }

        private static string ByteArrayToStr(byte[] bArr)
        {
            return ByteArrayToStr(bArr, 0, (uint)bArr.Length);
        }
    }
}
