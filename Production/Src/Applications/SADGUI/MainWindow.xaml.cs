using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;


namespace SADGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Capture m_camera;
        public MainWindow()
        {
            InitializeComponent();
            m_camera = new Capture();
            
        }

        private void GetImage()
        {
            
            Image<Bgr, byte> image = m_camera.QueryFrame(); //Take a frame with the webcam
            System.Drawing.Image windowsImage = image.ToBitmap(); //convert it to bitmap.

            //stream image to a memory stream and then save it
            BitmapImage bImage = new BitmapImage();
            bImage.BeginInit();
            MemoryStream memStream = new MemoryStream();
            windowsImage.Save(memStream, ImageFormat.Bmp);

            //Set the memory stream back to the beginning
            memStream.Seek(0, SeekOrigin.Begin);

            //Set the stream source and assign it to pictureBox1's image source
            bImage.StreamSource = memStream;
            bImage.EndInit();
            PictureBox.Source = bImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetImage();
        }

    }
}
