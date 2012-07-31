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
        private Thread threadWaitClient;

        private List<KClient> clients;

        private KinectSensorCollection sensorList;
        

        public KTcp(short _port) : base(IPAddress.Any,_port)
        {
            clients = new List<KClient>();
            sensorList = KinectSensor.KinectSensors;
            threadWaitClient = new Thread(new ThreadStart(waitClient));
            threadWaitClient.Name = "waitClient";
            threadWaitClient.Start();
            threadWaitClient.Join();
            Console.WriteLine("Kinect Server : Server Stopped !");
            Console.WriteLine("Press Any Key to continue !");
            Console.ReadKey();
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

                while (Thread.CurrentThread.IsAlive)
                {
                    KClient newKClient = new KClient(this, AcceptTcpClient(), sensorList);
                    addClient(newKClient);
                    Console.WriteLine(clients.Count);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Kinect Server Error : " + e.Message);
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
