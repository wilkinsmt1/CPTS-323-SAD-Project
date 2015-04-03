using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Emgu.CV.Features2D;
using SAD.Core.Data;
using SAD.Core.Devices;
using SAD.Core.IO;

namespace SADGUI
{
    class MainViewModel : ViewModelBase
    {
        private BitmapSource m_cameraImage;
        private Capture m_capture;
        private IMissileLauncher m_missileLauncher;
        private int move;
        private string m_missileCount;
        private TargetViewModel m_selectedTarget;
        private TargetManager m_targetManager;

        public MainViewModel()
        {
            GetImageCommand = new MyCommands(GetImage);
            LoadTargetsFromFileCommand = new MyCommands(LoadTargetsFromFile);
            MoveRightCommand = new MyCommands(MoveRight);
            MoveLeftCommand = new MyCommands(MoveLeft);
            MoveUpCommand = new MyCommands(MoveUp);
            MoveDownCommand = new MyCommands(MoveDown);
            FireCommand = new MyCommands(Fire);
            ClearTargetsCommand = new MyCommands(ClearTargets);
            ReloadMissilesCommand = new MyCommands(ReloadMissiles);
            TargetsCollection = new ObservableCollection<TargetViewModel>();
            m_targetManager  = TargetManager.GetInstance();
            //m_missileLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            //GetCount();
            move = 5;
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
        public string MissileCount
        {
            get { return m_missileCount; }
            set
            {
                m_missileCount = value;
                OnPropertyChanged();
            }
        }
        public TargetViewModel SelectedTarget
        {
            get
            {
                return m_selectedTarget;
            }
            set
            {
                m_selectedTarget = value;
                OnPropertyChanged();
            }
        }
        private void GetCount()
        {
            string propertyName = "MissileCount";
            Object MC;
            string SMC;

            MC = m_missileLauncher.GetType().GetProperty(propertyName).GetValue(m_missileLauncher, null);
            SMC = MC.ToString();
            MissileCount = SMC;
        }
        public ICommand LoadTargetsFromFileCommand { get; set; }
        public ICommand GetImageCommand{ get; set; }
        public ICommand MoveRightCommand { get; set; }
        public ICommand MoveLeftCommand { get; set; }
        public ICommand MoveUpCommand { get; set; }
        public ICommand MoveDownCommand { get; set; }
        public ICommand FireCommand { get; set; }
        public ICommand ReloadMissilesCommand { get; set; }
        public ObservableCollection<TargetViewModel> TargetsCollection { get; private set; }
        public ObservableCollection<Targets> TargetsList { get; set; }
        public ICommand ClearTargetsCommand { get; set; }

        public void ClearTargets()
        {
            TargetsCollection.Clear();
        }
        private void LoadTargetsFromFile()
        {
            var openFileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            var worked = openFileDialog.ShowDialog();
            if (worked == true)
            {
                MessageBox.Show("We loaded: " + openFileDialog.FileName);
                var iniReader = FRFactory.CreateReader(FRType.INIReader, openFileDialog.FileName);
                TargetsList = m_targetManager.GetTargetList();
                AddTarget();
            }
        }
        private void AddTarget()
        {
            foreach (var target in TargetsList)
            {
                var newTargetViewModel = new TargetViewModel(target);
                //var newTarget = new Target("newTarget", 0, 0, 0, true);
                //// But we need to wrap this target with a view model so that 
                //// the button (kill) will work 
                //var newTargetViewModel = new TargetViewModel(newTarget);

                TargetsCollection.Add(newTargetViewModel);
                SelectedTarget = newTargetViewModel;
            }
        }

        private void ReloadMissiles()
        {
            if (!(m_missileLauncher is DreamCheeky))
            {
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
            }
            else
            {
                m_missileLauncher.Reload();
                GetCount();
            }

        }
        private void MoveRight()
        {
            //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
            if (m_missileLauncher is DreamCheeky)
            {
                m_missileLauncher.MoveBy((0), (move * 22.2));
            }
            else if (m_missileLauncher is Mock)
            { //if m_missileLauncher is type Mock just show a message
                MessageBox.Show("Moving Mock launcher to the right by " + move + " degrees.");
            }
            else
            { //if m_missileLauncher has not be initilized, show an error
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
            }
            //m_missileLauncher.MoveTo(0,0);
        }
        private void MoveLeft()
        {
            //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
            if (m_missileLauncher is DreamCheeky)
            {
                m_missileLauncher.MoveBy((0), ((move * -1) * 22.2));
            }
            else if (m_missileLauncher is Mock)
            { //if m_missileLauncher is type Mock just show a message
                MessageBox.Show("Moving Mock launcher to the left by " + move + " degrees.");
            }
            else
            { //if m_missileLauncher has not be initilized, show an error
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
            }
            //m_missileLauncher.MoveTo(0,0);
            

        }
        private void MoveUp()
        {
            //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
            if (m_missileLauncher is DreamCheeky)
            {
                m_missileLauncher.MoveBy((move * 22.2), (0));
            }
            else if (m_missileLauncher is Mock)
            { //if m_missileLauncher is type Mock just show a message
                MessageBox.Show("Moving Mock launcher to the up by " + move + " degrees.");
            }
            else
            { //if m_missileLauncher has not be initilized, show an error
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
            }
            //m_missileLauncher.MoveTo(0,0);
            

        }
        private void MoveDown()
        {
            //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
            if (m_missileLauncher is DreamCheeky)
            {
                m_missileLauncher.MoveBy(((move * -1) * 22.2), (0));
            }
            else if (m_missileLauncher is Mock)
            { //if m_missileLauncher is type Mock just show a message
                MessageBox.Show("Moving Mock launcher to the down by " + move + " degrees.");
            }
            else
            { //if m_missileLauncher has not be initilized, show an error
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
            }
            //m_missileLauncher.MoveTo(0,0);

        }
        private void Fire()
        {
            //m_missileLauncher is already type DreamCheeky, call the Fire method
            // and then get the missile count.
            if (m_missileLauncher is DreamCheeky)
            {
                m_missileLauncher.Fire();
                GetCount();
            }
            else if (m_missileLauncher is Mock)
            { //if m_missileLauncher is type Mock just show a message
                MessageBox.Show("Firing ze missiles!");
            }
            else
            { //if m_missileLauncher has not be initilized, show an error
                MessageBox.Show("Error! Missile Launcher type has not be selected yet!");
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
