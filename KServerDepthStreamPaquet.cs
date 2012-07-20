using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.Kinect;

namespace KinectServer
{
    class KServerDepthStreamPaquet : KServerStreamPaquet
    {

        private const byte StreamId = 202;
        private int idSensor;
        private DepthImageFrame depthFrame;
        private short[] pxlData;


        public KServerDepthStreamPaquet(DepthImageFrame _depthFrame, int _idSensor)
        {
            depthFrame = _depthFrame;
            idSensor = _idSensor;

            pxlData = new short[depthFrame.PixelDataLength];
            depthFrame.CopyPixelDataTo(pxlData);

            setBodySize((uint)(depthFrame.PixelDataLength * sizeof(short) + 13));

            byte[] size = BitConverter.GetBytes((uint)(depthFrame.PixelDataLength * sizeof(short) + 13));

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
            data[headerSize()] = (byte)((byte)(idSensor) << 4 | (byte)(depthFrame.Format));


            byte[] frameNumber = BitConverter.GetBytes((UInt32)(depthFrame.FrameNumber));
            byte[] frameNumber2 = BitConverter.GetBytes((UInt32)(depthFrame.FrameNumber));
            
            byte[] playerIndexBitmask = BitConverter.GetBytes((UInt32)(DepthImageFrame.PlayerIndexBitmask));
            byte[] playerIndexBitmaskWidth = BitConverter.GetBytes((UInt32)(DepthImageFrame.PlayerIndexBitmaskWidth));
            
            Array.Reverse(frameNumber);
            Array.Reverse(playerIndexBitmask);
            Array.Reverse(playerIndexBitmaskWidth);

            Console.WriteLine(frameNumber[0] + " " + frameNumber[1] + " " + frameNumber[2] + " " + frameNumber[3]);
            Console.WriteLine(frameNumber2[0] + " " + frameNumber2[1] + " " + frameNumber2[2] + " " + frameNumber2[3]);

            /* Copies the data in the paquet buffer */
            Buffer.BlockCopy(frameNumber, 0, data, (int)headerSize() + 1, frameNumber.Length);
            Buffer.BlockCopy(playerIndexBitmask, 0, data, (int)headerSize() + 5, playerIndexBitmask.Length);
            Buffer.BlockCopy(playerIndexBitmaskWidth, 0, data, (int)headerSize() + 9, playerIndexBitmaskWidth.Length);
            Buffer.BlockCopy(pxlData, 0, data, (int)headerSize() + 13, pxlData.Length * sizeof(short));

            for (int i = 0; i < headerSize()+13; i++)
                Console.Write(data[i] + " ");

            Console.WriteLine(" ");
        }
    }
}
