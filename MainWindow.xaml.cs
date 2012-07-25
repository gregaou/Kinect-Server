using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows;

namespace KinectServer
{
    public partial class MainWindow : Window
    {
        /* Server */
        KTcp KinectTcp;

        public int nb_client_co = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(Object sender, System.Windows.RoutedEventArgs e)
        {
            KinectTcp = new KTcp();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
