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
using Microsoft.Kinect;

namespace KinectServer
{
    class KServerColorStreamPaquet : KServerStreamPaquet
    {
        private const byte StreamId = 201;
        private MemoryStream jpgImage;
        private ColorImageFormat format;
        private int idSensor;

        public KServerColorStreamPaquet(BitmapFrame pixels, ColorImageFormat format, int idSensor)
        {
            this.format = format;
            this.idSensor = idSensor;

            Stream img = new FileStream("test.jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            jpgImage = new MemoryStream();

            /* Encodes the frame into JPG format */
            encoder.Frames.Add(pixels);
            encoder.Save(jpgImage);
            //encoder.Save(img);

            /* Sets the size of the paquet */
            setBodySize((uint)(jpgImage.Length+1));
            byte[] size = BitConverter.GetBytes((UInt32)(jpgImage.Length+1));
            
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
            data[headerSize()] = (byte)((byte)(idSensor) << 4 |(byte)(format));
            Buffer.BlockCopy(toWrite, 0, data, (int)headerSize()+1, toWrite.Length);
        }
    }
}
