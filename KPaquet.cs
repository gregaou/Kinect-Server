using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace KinectServer
{
    abstract class KPaquet
    {
        protected byte[] data { get; private set; }

        public KPaquet()
        {
            data = new byte[headerSize()];
        }

        protected void setBodySize(uint size)
        {
            byte[] tmp = new byte[data.Length];
            data.CopyTo(tmp, 0);
            data = new byte[headerSize() + size];
            tmp.CopyTo(data, 0);
        }

        public abstract uint headerSize();
        protected abstract void buildHeader();
        protected abstract void buildBody();

        protected void build()
        {
            buildHeader();
            buildBody();
        }

        public UInt32 bodySize()
        {
            return (UInt32)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 0));
        }

        public UInt32 totalSize()
        {
            return bodySize() + headerSize();
        }

        public UInt32 timestamp()
        {
            return (UInt32)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 4));
        }
    }
}
