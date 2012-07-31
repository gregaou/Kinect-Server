using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Deployment.Application;

namespace KinectServer
{
    class KServer
    {
        static void Main(string[] args)
        {
            Console.Title = Application.ProductName;
            Console.WriteLine(kinectServerVersion());
            
            short port = portNumber();
            KTcp KinectServer = new KTcp(port);
        }

        static string kinectServerVersion()
        {
            Version version = new Version();
            DateTime time = new DateTime();
            try
            {
                if (ApplicationDeployment.IsNetworkDeployed) {
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    time = ApplicationDeployment.CurrentDeployment.TimeOfLastUpdateCheck;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return string.Format("{0} , Version {1}, {2}", Application.ProductName, version, time.ToLocalTime());

        }

        static short portNumber(){
            
            bool correct = true;
            short port;

            do
            {

                Console.Write("Kinect Server : Choose port [1337] : ");
                string sPort = Console.ReadLine();
                port = 1337;

                try
                {
                    correct = false;
                    if(sPort != "")
                        port = Convert.ToInt16(sPort);
                    
                }
                catch (Exception e)
                {
                    correct = true;
                    Console.WriteLine(e.Message);
                }
            }
            while (correct);

            Console.WriteLine("Kinect Server : port " + port + " selected.");
            return port;
        }
    }
}
