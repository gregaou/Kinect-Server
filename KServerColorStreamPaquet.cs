using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectServer
{
    class KServerColorStreamPaquet : KServerStreamPaquet
    {
        private const byte StreamId = 1;
        private byte[] pixels;
        private int stride;
        private MemoryStream jpgImage;

        public KServerColorStreamPaquet(byte[] pixels, int stride)
        {
            this.pixels = pixels;
            this.stride = stride;

            jpgImage = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            /* Encodes the frame into JPG format */
            encoder.Frames.Add(BitmapFrame.Create(new MemoryStream(pixels)));
            encoder.Save(jpgImage);

            /* Sets the size of the paquet */
            setBodySize((uint)jpgImage.Length);
            byte[] size = BitConverter.GetBytes((UInt32)jpgImage.Length);
            Array.Reverse(size);
            Buffer.BlockCopy(size, 0, data, 0, size.Length);

            /* Builds the paquet */
            build();
        }

        protected override byte id()
        {
            return StreamId;
        }

        protected override void buildBody()
        {
            /* Copies the data in the paquet buffer */
            byte[] toWrite = jpgImage.ToArray();
            Buffer.BlockCopy(toWrite, 0, data, (int)headerSize(), toWrite.Length);
        }
    }
}
