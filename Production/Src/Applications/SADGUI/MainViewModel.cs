using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using SADGUI.Annotations;

namespace SADGUI
{
    class MainViewModel : ViewModelBase
    {
        private Capture m_camera;
        private ICommand m_getImageCommand;
        private BitmapImage m_source;
        public MainViewModel()
        {
            m_camera = new Capture();
            m_getImageCommand = new MyCommands(GetImage);
            m_source = new BitmapImage();
        }
        private void GetImage()
        {

            Image<Bgr, byte> image = m_camera.QueryFrame(); //Take a frame with the webcam
            Image windowsImage = image.ToBitmap(); //convert it to bitmap.
            //windowsImage.Save("C:\\testfolder\\test.bmp", ImageFormat.Bmp);
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
            SourceImage = bImage;
        }

        public BitmapImage SourceImage
        {
            get { return m_source; }
            set
            {
                m_source = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetImageCommand
        {
            get { return m_getImageCommand; }
            set
            {
                m_getImageCommand = value;
                OnPropertyChanged();
            }
        }
    }

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
