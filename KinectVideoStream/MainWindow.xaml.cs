using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor __kinect;
        public MainWindow()
        {
            InitializeComponent();
            SetupKinect();
        }
            //setup kinect
            void SetupKinect( )
        {
            //get first kinect sensor
            if ( KinectSensor.KinectSensors.Count > 0 )
            {
                __kinect = KinectSensor.KinectSensors[ 0 ];

                if ( __kinect.Status == KinectStatus.Connected )
                {
                    //start the video stream only if the program in run from the
                    //Calibration Menu

                    __kinect.ColorStream.Enable( ColorImageFormat.RgbResolution640x480Fps30 );

                    // init the Depth Stream, with Near Mode Enabled
                    //KinectSensor.DepthStream.Enable( DepthImageFormat.Resolution640x480Fps30 );
                    //KinectSensor.DepthStream.Range = DepthRange.Near;

                    __kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>( _sensor_AllFramesReady );

                    __kinect.Start( );
                }
            }
        }


        void _sensor_AllFramesReady( object sender, AllFramesReadyEventArgs e )
        {

            using ( ColorImageFrame colorFrame = e.OpenColorImageFrame( ) )
            {
                if ( colorFrame == null )
                    return;


                byte[] pixels = new byte[ colorFrame.PixelDataLength ];
                colorFrame.CopyPixelDataTo( pixels );

                int stride = colorFrame.Width * 4;
                Video.Source =
                    BitmapSource.Create( colorFrame.Width, colorFrame.Height,
                    96, 96, PixelFormats.Bgr32, null, pixels, stride ); 


            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            __kinect.Stop();
        }
    }
}