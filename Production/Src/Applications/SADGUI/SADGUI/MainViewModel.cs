using System;
using System.Collections.Concurrent;
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
using System.Diagnostics;
using System.Threading;
using Emgu.CV.Structure;

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
        private string m_LauncherPosition;
        private bool IsMLRunning;
        private CancellationTokenSource cts;
        private bool isRunning;

        private BlockingCollection<Image<Bgr, byte>> imageBlockingCollection;

        private BlockingCollection<Image<Bgr, byte>> processBuffer;

        public MainViewModel()
        {
            try
            {
                m_capture = new Capture();
            }
            catch (Exception)
            {

            }
            GetImageCommand = new MyCommands(GetImage);
            LoadTargetsFromFileCommand = new MyCommands(LoadTargetsFromFile);
            MoveRightCommand = new MyCommands(MoveRight);
            MoveLeftCommand = new MyCommands(MoveLeft);
            MoveUpCommand = new MyCommands(MoveUp);
            MoveDownCommand = new MyCommands(MoveDown);
            FireCommand = new MyCommands(Fire);
            ClearTargetsCommand = new MyCommands(ClearTargets);
            KillTargetsCommand = new MyCommands(KillTargets);
            ReloadMissilesCommand = new MyCommands(ReloadMissiles);
            TargetsCollection = new ObservableCollection<TargetViewModel>();
            m_targetManager  = TargetManager.GetInstance();
            CreateMockCommand = new MyCommands(CreateMock);
            CreateDCCommand = new MyCommands(CreateDC);
            StopCommand = new MyCommands(Stop);
            //m_missileLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            //GetCount();
            //GetPosition();
            move = 5;

            cts = new CancellationTokenSource();
            IsMLRunning = false;
            imageBlockingCollection = new BlockingCollection<Image<Bgr, byte>>();
            processBuffer = new BlockingCollection<Image<Bgr, byte>>();
            isRunning = false;
        }

        private void GetImage()
        {
            cts = new CancellationTokenSource();
            //in order for the video feed to start after being stopped
            //the new blocking collections must be created. 
            imageBlockingCollection = new BlockingCollection<Image<Bgr, byte>>();
            processBuffer = new BlockingCollection<Image<Bgr, byte>>();
            isRunning = true;
            var producerTask = Task.Run(() => this.ProduceFrame(imageBlockingCollection, cts.Token));
            var consumerTask = Task.Run(() => this.ConsumeFrame(imageBlockingCollection, cts.Token));
            //if (m_capture == null)
            //{
            //    m_capture = new Capture(0);
            //}
            //var image = m_capture.QueryFrame();
            ////image.Save(@"c:\testfolder\test.png");
            //var wpfImage = ConvertImageToBitmap(image);

            //CameraImage = wpfImage;

        }
        private void ProduceFrame(BlockingCollection<Image<Bgr, byte>> bc, CancellationToken ct)
        {
            
            if (m_capture != null)
            {
                while (!ct.IsCancellationRequested)
                {
                    var image = m_capture.QueryFrame();
                    bc.Add(image);
                }

                bc.CompleteAdding();
            }
        }
        private void ConsumeFrame(BlockingCollection<Image<Bgr, byte>> bc, CancellationToken ct)
        {
            while (!bc.IsCompleted)
            {
                var imageToDisplay = bc.Take();
                App.Current.Dispatcher.Invoke(
                    () => this.CameraImage = BitmapConverter.ToBitmapSource(image: imageToDisplay));
                processBuffer.Add(imageToDisplay);
                // property implements IPropertyNotify. Will update fairly quickly. Use dependency properties to make this even faster.
            }

            processBuffer.CompleteAdding();
        }

        private void Stop()
        {
            isRunning = false;
            cts.Cancel();

            
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
        public string LauncherPosition
        {
            get { return m_LauncherPosition; }
            set
            {
                m_LauncherPosition = value;
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
        private void GetPosition()
        {
            string propertyName = "phiPosition";
            string propertyName2 = "thetaPosition";
            Object LP;
            Object LP2;
            string SLP = "Phi: "; 

            LP = m_missileLauncher.GetType().GetProperty(propertyName).GetValue(m_missileLauncher, null);
            SLP += LP.ToString();
            LP2 = m_missileLauncher.GetType().GetProperty(propertyName2).GetValue(m_missileLauncher, null);
            SLP += " Theta: ";     //"Phi: (phiPostion value) Theta: "
            SLP += LP2.ToString();//"Phi: (phiPostion value) Theta: (thetaPosition value)"
            LauncherPosition = SLP;  //will display Phi: (phiPostion value) Theta: (thetaPosition value)
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
        public ICommand KillTargetsCommand { get; set; }
        public ICommand CreateMockCommand { get; set; }
        public ICommand CreateDCCommand { get; set; }
        public ICommand StopCommand { get; set; }

        private void CreateMock()
        {
            m_missileLauncher = MLFactory.CreateMissileLauncher(MLType.Mock);
            //GetCount();
            //GetPosition();
        }
        private void CreateDC()
        {
            m_missileLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            GetCount();
            GetPosition();
        }

        private void KillTargets()
        {
            

            if (!(m_missileLauncher is DreamCheeky))
            {
                MessageBox.Show("Error! DreamCheeky launcher has not been selected!");
            }
            else
            {
                

                if (SelectedTarget == null)
                {
                    MessageBox.Show("Error! No Targets were selected!");
                }
                else
                {
                    if (SelectedTarget.Target.IsFriend)
                    {
                        MessageBox.Show("Error! Friendly fire is not permited!");
                    }
                    else
                    {
                        IsMLRunning = true;
                        var killTask = Task.Run(() => this.Kill());
                    }
                    
                }
 
            }
        }

        private void Kill()
        {
            double x, y, z, theta, phi;
            x = SelectedTarget.Target.X;
            y = SelectedTarget.Target.Y;
            z = SelectedTarget.Target.Z;
            theta = TargetPositioning.CalculateTheta(x, y, z);
            phi = TargetPositioning.CalculatePhi(x, y);
            //m_missileLauncher.MoveTo(0,0);
            m_missileLauncher.MoveTo((phi * 22.2), (theta * 22.2));
            GetPosition();
            Fire();
            SelectedTarget.Target.IsAlive = false;
        }
        private void ClearTargets()
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
                MessageBox.Show("Error! DreamCheeky launcher has not been selected!");
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
                GetPosition();
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
                GetPosition();
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
                GetPosition();
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
                GetPosition();
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
