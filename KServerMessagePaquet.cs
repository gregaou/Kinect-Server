using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectServer
{
    class KServerMessagePaquet : KServerPaquet
    {
        private byte code;
        private byte[] msg;

        public KServerMessagePaquet(byte code, string msg)
        {
            this.code = code;
            this.msg = KServerPaquet.StrToByteArray(msg);

            setBodySize((uint)msg.Length);
            byte[] size = BitConverter.GetBytes((UInt32)msg.Length);
            Array.Reverse(size);

            Buffer.BlockCopy(size, 0, data, 0, size.Length);

            build();
        }

        protected override byte id()
        {
            return code;
        }

        protected override void buildBody()
        {
            Buffer.BlockCopy(msg, 0, data, (int)headerSize(), (int)bodySize());
        }
    }
}
