using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Microsoft.Kinect;

namespace KinectServer
{
    class KServerColorStreamPaquet : KServerStreamPaquet
    {
        private const byte StreamId = 201;
        private MemoryStream jpgImage;
        private ColorImageFrame imageFrame;
        private int idSensor;

        public KServerColorStreamPaquet(ColorImageFrame _imageFrame, int _idSensor)
        {
            idSensor = _idSensor;
            imageFrame = _imageFrame;

            byte[] pxlData = new byte[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pxlData);

            jpgImage = new MemoryStream();
            unsafe {
               fixed (byte* ptr = pxlData) {
                  using (Bitmap image = new Bitmap(imageFrame.Width, imageFrame.Height, imageFrame.Width*4, PixelFormat.Format32bppArgb, new IntPtr(ptr))) {
                     image.Save(jpgImage, ImageFormat.Jpeg);
                  }
               }
            }

            /* Sets the size of the paquet */
            setBodySize((uint)(jpgImage.Length+5));
            byte[] size = BitConverter.GetBytes((UInt32)(jpgImage.Length+5));
            
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
            byte[] frameNumber = BitConverter.GetBytes((UInt32)IPAddress.HostToNetworkOrder(imageFrame.FrameNumber));

            data[headerSize()] = (byte)((byte)(idSensor) << 4 |(byte)(imageFrame.Format));
            Buffer.BlockCopy(frameNumber,0,data,(int)headerSize()+1,frameNumber.Length);
            Buffer.BlockCopy(toWrite, 0, data, (int)headerSize()+frameNumber.Length+1, toWrite.Length);
        }
    }
}
