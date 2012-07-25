using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Microsoft.Kinect;


namespace KinectServer
{
    class KTcp : TcpListener
    {

        private static int _port = 1337;

        private Thread threadWaitClient;

        private List<KClient> clients;

        private KinectSensorCollection sensorList;
        

        public KTcp() : base(IPAddress.Any,_port)
        {
            clients = new List<KClient>();
            sensorList = KinectSensor.KinectSensors;
            threadWaitClient = new Thread(new ThreadStart(waitClient));
            threadWaitClient.Name = "waitClient";
            threadWaitClient.Start();
        }

        ~KTcp()
        {

            if (threadWaitClient.IsAlive)
                threadWaitClient.Abort();
        }

        private void waitClient() 
        {

            try
            {
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                System.Environment.Exit(1);
            }
            while (Thread.CurrentThread.IsAlive)
            {
                KClient newKClient = new KClient(this, AcceptTcpClient(), sensorList);
                addClient(newKClient);
                Console.WriteLine(clients.Count);
            }

            

            Stop();
        }


        private void addClient(KClient c)
        {
            lock(clients)
                clients.Add(c);
        }

        public void delClient(KClient c)
        {
            lock(clients)
                clients.Remove(c);
        }

    }
}
