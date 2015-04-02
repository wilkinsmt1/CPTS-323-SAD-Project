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

//added:
using System.Web.Script.Serialization;
using System.IO;

namespace SADGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summa
    /// ry>
    public class TargetList
    {
        public string Description { get; set; }
        public List<LauncherTarget> Targets { get; set; }
    }

    public class LauncherTarget
    {
        public string Target { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool Friend { get; set; }
        public int Points { get; set; }
        public int FlashRate { get; set; }
        public int SpawnRate { get; set; }
        public bool CanSwapSidesWhenHit { get; set; }
    }
    
    public class JSONparser
    {
        private string path;
        public JSONparser(string filePath)
        {
            path = filePath;
        }

        public string ReadFile(string data = null)
        {
            string jsonStr = data;
            string retJsonData = "";

            if (data == null)
            {
                jsonStr = File.ReadAllText(path);
            }

            TargetList targets = new JavaScriptSerializer().Deserialize<TargetList>(jsonStr);
            Console.WriteLine(targets.Description);
            foreach (LauncherTarget tgt in targets.Targets)
            {
                retJsonData += String.Format("\n{0} \nName={1} \nX={2} \nY={3} \nZ={4} \nFriend={5} \nPoints={6} ",
                                    tgt.Target, tgt.Name, tgt.X, tgt.Y, tgt.Z, tgt.Friend, tgt.Points);
                retJsonData += String.Format("FlashRate={0} \nSpawnRate={1} \nCanSwapSidesWhenHit={2}\n",
                                    tgt.FlashRate, tgt.SpawnRate, tgt.CanSwapSidesWhenHit);
            }

            return retJsonData;

            /*LauncherTarget newTgt = new LauncherTarget();
            newTgt.Target = "Target"; newTgt.Name = "TargetFour";
            newTgt.X = 77; newTgt.Y = 88; newTgt.Z = 99;
            newTgt.Friend = false; newTgt.Points = 55; newTgt.FlashRate = -1;
            newTgt.SpawnRate = 4; newTgt.CanSwapSidesWhenHit = false;

            targets.Targets.Add(newTgt);
            jsonStr = new JavaScriptSerializer().Serialize(targets);
            File.WriteAllText(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\cmd.json"), jsonStr);*/
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string file, fileExt, data = null;
            string dir = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\.."));
            string tgtDisplay;

            file = dir + @"\cmd.json";
            fileExt = System.IO.Path.GetExtension(file);

            JSONparser parser = new JSONparser(file);
            tgtDisplay = parser.ReadFile(data);
            TextBox1.Text = tgtDisplay;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
