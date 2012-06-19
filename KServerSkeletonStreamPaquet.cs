using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Microsoft.Kinect;

namespace KinectServer
{
    class KServerSkeletonStreamPaquet : KServerStreamPaquet
    {
        private const byte streamId = 2;
        private const uint dataHeaderSize = 1;
        private const uint skeletonHeaderSize = 2;
        private KinectSensor sensor;
        private Skeleton[] skeletons;

        public KServerSkeletonStreamPaquet(KinectSensor sensor, Skeleton[] skeletons)
        {
            this.sensor = sensor;
            this.skeletons = skeletons;

            uint length = paquetSize();

            setBodySize(length);
            byte[] size = BitConverter.GetBytes((UInt32)length);
            Array.Reverse(size);
            Buffer.BlockCopy(size, 0, data, 0, size.Length);

            buildBody();
        }

        protected override byte id()
        {
            return streamId;
        }

        protected override void buildBody()
        {
            uint i = headerSize();

            data[i++] = (byte)skeletons.Count();

            foreach (Skeleton s in skeletons)
            {
                data[i++] = (byte)s.TrackingState;
                data[i++] = (byte)s.Joints.Count();

                foreach (Joint j in s.Joints)
                {
                    Point p = pointToScreen(j.Position);

                    data[i++] = (byte)j.TrackingState;

                    byte[] XByteArray = BitConverter.GetBytes((UInt16)p.X);
                    Array.Reverse(XByteArray);
                    Buffer.BlockCopy(XByteArray, 0, data, (int)i, XByteArray.Length);
                    i += (uint)XByteArray.Length;

                    byte[] YByteArray = BitConverter.GetBytes((UInt16)p.Y);
                    Array.Reverse(YByteArray);
                    Buffer.BlockCopy(YByteArray, 0, data, (int)i, YByteArray.Length);
                    i += (uint)XByteArray.Length;
                }
            }
        }

        private uint paquetSize()
        {
            uint size = dataHeaderSize;

            foreach (Skeleton s in skeletons)
                size += skeletonHeaderSize + 5 * (uint)s.Joints.Count();

            return size;
        }

        private Point pointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = sensor.MapSkeletonPointToDepth(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
    }
}
