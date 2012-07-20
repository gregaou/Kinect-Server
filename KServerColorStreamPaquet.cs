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
        private ColorImageFrame imageFrame;
        private int idSensor;

        public KServerColorStreamPaquet(ColorImageFrame _imageFrame, int _idSensor)
        {
            idSensor = _idSensor;
            imageFrame = _imageFrame;

            byte[] pxlData = new byte[imageFrame.PixelDataLength];
            imageFrame.CopyPixelDataTo(pxlData);

            BitmapFrame image = BitmapFrame.Create(BitmapSource.Create(
                imageFrame.Width,
                imageFrame.Height,
                96,
                96,
                PixelFormats.Bgr32,
                BitmapPalettes.Halftone256Transparent,
                pxlData,
                imageFrame.Width * 4));

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            jpgImage = new MemoryStream();

            /* Encodes the frame into JPG format */
            encoder.Frames.Add(image);
            encoder.Save(jpgImage);

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
