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
            /*
            NetworkStream ns = client.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;

            int nb_k = 0;

            foreach (KinectSensor s in sensorList)
            {
                if(s.Status==KinectStatus.Connected)
                    nb_k++;
            }

            if (nb_k == 0)
            {
                byte[] mess = StrToByteArray("No Kinect Ready Found");
                ns.Write(mess, 0, mess.Length);
            }
            */

            while (Thread.CurrentThread.IsAlive)
            {
                try
                {
                    KClientPaquet cp = new KClientPaquet(client.GetStream());
                    string s = cp.getQuery();
                    System.Console.WriteLine("Chaine reçue : " + s);

                    KServerPaquet sp = new KServerMessagePaquet(100, s);
                    sp.send(client.GetStream());
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    break;
                }

                /*
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = ns.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                // The client has disconnected from the server
                if (bytesRead == 0)
                    break;



                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
                ns.Write(BitConverter.GetBytes('k'), 0, 1);
                */
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
