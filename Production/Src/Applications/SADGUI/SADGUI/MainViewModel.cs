//mvm.cs:

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
using TargetServerCommunicator;
using TargetServerCommunicator.Servers;

namespace SADGUI
{
    class MainViewModel : ViewModelBase, IAutoModeBase
    {
        private IAutoModeBase i_autobase_currentstate;
        private IAutoModeBase a_autoModeElimAll;
        private IAutoModeBase a_autoModeElimEnemies;
        private IAutoModeBase a_autoModeElimFriends;

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
        private string m_IP;
        private int m_Port;
        private const string m_teamname = "Scorched Earth Destroyers";
        private string m_SelectedGame;
        private IGameServer gameServer;


        private BlockingCollection<Image<Bgr, byte>> imageBlockingCollection;

        private BlockingCollection<Image<Bgr, byte>> processBuffer;

        public MainViewModel()
        {
            //this was causing the XAML designer to crash it has been moved to GetImage()
            //try
            //{
            //    m_capture = new Capture();
            //}
            //catch (Exception)
            //{

            //}
            GetImageCommand = new MyCommands(GetImage);
            LoadTargetsFromFileCommand = new MyCommands(LoadTargetsFromFile);
           // LoadTargetsFromServerCommand = new MyCommands(LoadTargetsFromServer);
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
            KillAllCommand = new MyCommands(KillAll);
            KillEnemiesCommand = new MyCommands(KillEnemies);
            KillFriendsCommand = new MyCommands(KillFriends);
            LoadServerCommand = new MyCommands(LoadServer);
            StartGameCommand = new MyCommands(StartGame);
            StopGameCommand = new MyCommands(StopGame);
            m_missileLauncherCommandQueue = new Queue<ICommand>();
            GameList = new ObservableCollection<string>();
            m_targetManager.TargetList = new ObservableCollection<Targets>();


            //m_missileLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            //GetCount();
            //GetPosition();
            move = 5;

            cts = new CancellationTokenSource();
            IsMLRunning = false;
            imageBlockingCollection = new BlockingCollection<Image<Bgr, byte>>();
            processBuffer = new BlockingCollection<Image<Bgr, byte>>();
            isRunning = false;
            this.StartCommandQueue();

            /*within implClass constructor:
            obj1 = new ClassName(this);
                private IAutoModeBase i_autobase_currentstate;
                private IAutoModeBase AutoModeElimAll;
                private IAutoModeBase AutoModeElimEnemies;
                private IAutoModeBase AutoModeElimFriends;
		    objBase = 1stDerivedClassObj;*/
            a_autoModeElimAll = new AutoModeElimAll(this);
            a_autoModeElimEnemies = new AutoModeElimEnemies(this);
            a_autoModeElimFriends = new AutoModeElimFriends(this);

            i_autobase_currentstate = a_autoModeElimEnemies;

        }

        //function to set IAutoModeBaseState state:
        private void setIAutoModeBaseState(IAutoModeBase _i_auto_currentstate)
        {
            i_autobase_currentstate = _i_auto_currentstate;
        }

        private void StopGame()
        {
            if (gameServer == null)
                return;

            if (SelectedGame == null)
                return;

            gameServer.StopRunningGame();
        }

        private void StartGame()
        {
            //First you want to get the targets
            GetTargets();
            if (gameServer == null)
                return;

            if (SelectedGame == null)
                return;

            gameServer.StartGame(SelectedGame);
            KillAll();
        }

        /// <summary>
        /// Loads the server based on the team name, IP, and Port
        /// Uses code found in SADClient Program.cs
        /// </summary>
        private void LoadServer()
        {
            //MessageBox.Show("Team name is: " + TeamName);
            //MessageBox.Show("IP is: " + IP);
            //MessageBox.Show("Port is: " + Port);

            //This throws an exception because I cannot connect to the server yet,
            //so I will comment it out for now

            ////Code from SADClient Program.cs
            //create a game server

            gameServer = GameServerFactory.Create(GameServerType.WebClient, TeamName, IP, Port);
            gameServer.StopRunningGame();
            //get the game list, Game combobox on the main window is bound to GameList

            var games = gameServer.RetrieveGameList();
            foreach (var game in games)
            {
                GameList.Add(game);
            }
            
            //var targets = gameServer.RetrieveTargetList(m_SelectedGame);
            //ConvertTargets(targets);

        }
        private void GetTargets()
        {
            if (gameServer == null)
                return;

            if (SelectedGame == null)
                return;

            var targets = gameServer.RetrieveTargetList(SelectedGame);
            foreach (var target in targets)
            {
                // Translate the GameServerCommunicatorTarget into your own...
                // Then add to your own collection of targets bound by the View.
                //Targets.Add(target);
                ConvertTargets(target);
            }
            TargetsList = m_targetManager.GetTargetList();
            //Add a target viewmodel
            AddTarget();
        }
        /// <summary>
        /// Converts the GameServers targets into our TargetManager targets.
        /// </summary>
        /// <param name="target"></param>
        private void ConvertTargets(TargetServerCommunicator.Data.Target target)
        {
            m_targetManager.TargetList.Add(new Targets() // add all the extracted data to the list
            {
                TargetName = target.name,
                X = target.x, //have to do this all at the same time
                Y = target.y, //so it is in the same Targets object
                Z = target.z,
                IsFriend = Convert.ToBoolean(target.status),
                Points = (int)target.points,
                FlashRate = 0,
                SpawnRate = (int)target.spawnRate,
                CanSwapSidesWhenHit = target.canChangeSides,
                Status = "Still At Large",
                IsAlive = true
            });
            

        }


        private void GetImage()
        {
            if (m_capture == null)
            {
                m_capture = new Capture();
            }
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
        public string TeamName { get { return m_teamname; }}
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

        public string SelectedGame
        {
            get { return m_SelectedGame; }
            set
            {
                if (value == m_SelectedGame)
                {
                    return;
                }
                m_SelectedGame = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> GameList { get; set; }
        public ICommand LoadTargetsFromFileCommand { get; set; }
        public ICommand LoadTargetsFromServerCommand { get; set; }
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
        public ICommand KillEnemiesCommand { get; set; }
        public ICommand KillFriendsCommand { get; set; }
        public ICommand KillAllCommand { get; set; }
        public ICommand LoadServerCommand { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand StopGameCommand { get; set; }

        public string IP
        {
            get { return m_IP; }
			set
			{
				if(value == m_IP)
				{
					return;
				}
				m_IP = value;
				OnPropertyChanged();
			}
        }

        public int Port
        {
            get { return m_Port; }
			set
			{
				if(value == m_Port)
				{
					return;
				}
				m_Port = value;
				OnPropertyChanged();
			}
        }

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
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
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
                            double x, y, z, theta, phi;
                            x = SelectedTarget.Target.X;
                            y = SelectedTarget.Target.Y;
                            z = SelectedTarget.Target.Z;
                            theta = TargetPositioning.CalculateTheta(x, y);
                            phi = TargetPositioning.CalculatePhi(y, z);
                            //m_missileLauncher.MoveTo(0,0);
                            m_missileLauncher.MoveTo((phi * 22.2), (theta * 22.2));
                            GetPosition();
                            Fire();
                            SelectedTarget.Target.IsAlive = false;
                        }
                    }
                }
            }));
        }

        internal void KillAll()
        {
            foreach (var target in TargetsCollection)
            {
                m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
                {
                    KillTheTargets(target.Target);
                }));
            }
        }

        internal void KillEnemies()
        {
            foreach (var target in TargetsCollection)
            {
                if (!target.Target.IsFriend)
                {
                    m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
                    {
                       KillTheTargets(target.Target);
                    }));
                }
            }
            
        }

        internal void KillFriends()
        {
            foreach (var target in TargetsCollection)
            {
                if (target.Target.IsFriend)
                {
                    m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
                    {
                        KillTheTargets(target.Target);
                    }));
                }
            }
        }
        private void KillTheTargets(Targets target)
        {
            double x, y, z, theta, phi;
            x = target.X;
            y = target.Y;
            z = target.Z;
            theta = TargetPositioning.CalculateTheta(y, x);
            phi = TargetPositioning.CalculatePhi(y, z);
            //m_missileLauncher.MoveTo(0,0);
           m_missileLauncher.MoveBy((phi * 22.2), (theta * 22.2));
            //m_missileLauncher.MoveTo(0,0);
           // MessageBox.Show("phi: " + phi);
           // MessageBox.Show("theta:" + theta);
            //m_missileLauncher.MoveBy((phi * 22.2), (theta * 22.2));
            GetPosition();
            m_missileLauncher.Fire();
            GetCount();
            //target.IsAlive = false;
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
       //private void LoadTargetsFromServer()
       //{
       //      string remoteUri;
       //      string dir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
       //      string file = dir + @"\webini.txt";
       //
        //    var openInputDialog = new Ookii.Dialogs.InputDialog();
        //    openInputDialog.WindowTitle = "Load Targets";
        //    openInputDialog.MainInstruction = "Enter Server URL";

        //    if (File.Exists(file))
        //    {
        //        File.Delete(file);
        //    }

        //    using (WebClient webClient = new WebClient())
        //    {
        //        try
        //        {
        //            if (openInputDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                remoteUri = openInputDialog.Input;
        //                MessageBox.Show("We loaded: " + remoteUri);
        //                webClient.DownloadFile(remoteUri, file);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("Target file not found on Server.");
        //        }
        //        finally
        //        {
        //            if (File.Exists(file))
        //            {
        //                var iniReader = FRFactory.CreateReader(FRType.INIReader, file);
        //                TargetsList = m_targetManager.GetTargetList();
        //                AddTarget();
        //            }
        //        }
        //    }
        //}
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
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
            {
                //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
                if (m_missileLauncher is DreamCheeky)
                {
                    m_missileLauncher.MoveByButtons((0), (move * 22.2));
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
            }));
        }
        private void MoveLeft()
        {
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
            {
                //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
                if (m_missileLauncher is DreamCheeky)
                {
                    m_missileLauncher.MoveByButtons((0), ((move * -1) * 22.2));
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
            }));
            

        }
        private void MoveUp()
        {
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
            {
                //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
                if (m_missileLauncher is DreamCheeky)
                {
                    m_missileLauncher.MoveByButtons((move * 22.2), (0));
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
            }));
            

        }
        private void MoveDown()
        {
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
            {
                //m_missileLauncher is already type DreamCheeky, call the MoveBy method.
                if (m_missileLauncher is DreamCheeky)
                {
                    m_missileLauncher.MoveByButtons(((move * -1) * 22.2), (0));
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
            }));

        }
        private void Fire()
        {
            m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
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

            }));

        }

        /// <summary>
        /// The commands for the missile launcher to run.
        /// </summary>
        private Queue<ICommand> m_missileLauncherCommandQueue;

        /// <summary>
        /// Is the command queue running?
        /// Setting to false will stop the command queue.
        /// </summary>
        private bool m_commandQueueRunning;

        /// <summary>
        /// The command loop.
        /// </summary>
        private async void RunCommandQueue()
        {
            while (m_commandQueueRunning)
            {
                await Task.Delay(50);
                if (m_missileLauncherCommandQueue.Count > 0)
                {
                    var command = m_missileLauncherCommandQueue.Dequeue();
                    command.Execute(null);
                }
            }
        }

        /// <summary>
        /// Start the missile launcher command loop
        /// </summary>
        private void StartCommandQueue()
        {
            m_commandQueueRunning = true;
            var task = Task.Run(() => RunCommandQueue());
        }


        //for interface, these functions must be implemented: 
        public void killSelectedTargets()
        {
            return;
        }
        public void moveToFirstTarget()
        {
            return;
        }
        public void moveToNextTarget()
        {
            return;
        }
        public void friend()
        {
            return;
        }
        public void foe()
        {
            return;
        }
        public void noMissiles()
        {
            return;
        }
        public void reload()
        {
            return;
    }
        public void hasMissiles()
        {
            return;
        }
        public void hit()
        {
            return;
        }
        public void miss()
        {
            return;
        }
        public void canSwapSides()
        {
            return;
        }
        public void cannotSwapSides()
        {
            return;
        }

    } //end mvm class

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
