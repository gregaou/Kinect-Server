using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using Microsoft.Kinect;

namespace KinectServer
{
    class KClient
    {

        public TcpClient client { get; private set; }
        private KTcp server;
        private KinectSensorCollection sensorList;

        private Thread threadClient;
        

        public KClient(KTcp _server, TcpClient _client, KinectSensorCollection _sensorList)
        {
            client = _client;
            server = _server;
            sensorList = _sensorList;

            threadClient = new Thread(new ThreadStart(clientComm));
            threadClient.Name = "Client " + ((IPEndPoint)client.Client.RemoteEndPoint).Address;
            threadClient.Start();
        }

        ~KClient()
        {
            if (threadClient.IsAlive)
                threadClient.Abort();
  
        }


        private void closeClient()
        {
            server.delClient(this);

            if (client.Connected)
                client.Close();

        }

        private void clientComm()
        {

            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    KClientPaquet cp = new KClientPaquet(client.GetStream());
                    string s = cp.getQuery();
                    System.Console.WriteLine("Chaine reçue : " + s);


                    KQuery q = new KQuery(cp.getQuery(),client.GetStream());
                    byte code = q.process();
                    KServerPaquet sp = new KServerMessagePaquet(code, q.action.rData);
                    sp.send(client.GetStream());
                }
                catch (KActionException e)
                {
                    Console.WriteLine(e.exceptionNumber);
                    KServerPaquet sp = new KServerMessagePaquet(222, "SMURK");
                    sp.send(client.GetStream());
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Erreur : " + Thread.CurrentThread.Name + " : " + e.Message);
                    break;
                }
           
            }

            closeClient();
            

        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] bArr)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(bArr);
        }


    }
}
