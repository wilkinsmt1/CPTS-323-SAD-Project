using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Emgu.CV;
using SAD.Core.Devices;
using SAD.Core.IO;

namespace SADGUI
{
    class MainViewModel : ViewModelBase
    {
        private BitmapSource m_cameraImage;
        private Capture m_capture;

        public MainViewModel()
        {
            GetImageCommand = new MyCommands(GetImage);
            LoadTargetsFromFileCommand = new MyCommands(LoadTargetsFromFile);
        }

        private void GetImage()
        {
            if (m_capture == null)
            {
                m_capture = new Capture(0);
            }
            var image = m_capture.QueryFrame();
            image.Save(@"c:\testfolder\test.png");
            var wpfImage = ConvertImageToBitmap(image);

            CameraImage = wpfImage;

        }
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr ptr);
        private static BitmapSource ConvertImageToBitmap(IImage image)
        {
            if (image != null)
            {
                using (var source = image.Bitmap)
                {
                    var hbitmap = source.GetHbitmap();
                    try
                    {
                        var bitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero,
                                                     Int32Rect.Empty,
                                                     BitmapSizeOptions.FromEmptyOptions());
                        DeleteObject(hbitmap);
                        bitmap.Freeze();
                        return bitmap;
                    }
                    catch
                    {
                        image = null;
                    }
                }
            }
            return null;
        }
        public BitmapSource CameraImage
        {
            get { return m_cameraImage; }
            set
            {
                if (Equals(value, m_cameraImage))
                {
                    return;
                }
                m_cameraImage = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoadTargetsFromFileCommand { get; set; }
        public ICommand GetImageCommand{ get; set; }

        private void LoadTargetsFromFile()
        {
            var openFileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            var worked = openFileDialog.ShowDialog();
            if (worked == true)
            {
                MessageBox.Show("We loaded: " + openFileDialog.FileName);
                var iniReader = FRFactory.CreateReader(FRType.INIReader, openFileDialog.FileName);
            }
        }
    }

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
