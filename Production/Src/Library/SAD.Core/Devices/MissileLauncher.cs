using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Devices
{

    public interface IMissileLauncher
    {
        void MoveBy(double phi, double theta);
        void MoveTo(double phi, double theta);
        void Fire();
        void Reload();
        void PrintStatus();

    }

    public enum MLType
    {
        DreamCheeky,
        Mock
    }
    public class DreamCheeky : IMissileLauncher
    {
        public void MoveBy(double phi, double theta)
        {
            Console.WriteLine("Moving.");
        }
        public void MoveTo(double phi, double theta)
        {
            Console.WriteLine("Moving.");
        }

        public void Fire()
        {
            Console.WriteLine("Firing.");
        }

        public void Reload()
        {
            Console.WriteLine("Reloading.");
        }

        public void PrintStatus()
        {
            Console.WriteLine("No status yet.");
        }

        public int MissileCount { get; set; }
        public string LauncherName { get; set; }
        private static int TotalMissiles = 4;
    }

    public class Mock : IMissileLauncher
    {
        public void MoveBy(double phi, double theta)
        {
            Console.WriteLine("Moving.");
        }

        public void MoveTo(double phi, double theta)
        {
            Console.WriteLine("Moving.");
        }

        public void Fire()
        {
            Console.WriteLine("Firing.");
        }

        public void Reload()
        {
            Console.WriteLine("Reloading.");
        }

        public void PrintStatus()
        {
            Console.WriteLine("Name: Mock Launcher\n Status: Married with children.");
        }
    }

    //public class MissileLauncherAdapter : IMissileLauncher
    //{
    //    public void MoveBy(double phi, double theta)
    //    {
    //        Console.WriteLine("Moving.");
    //    }
    //    public void MoveTo(double phi, double theta)
    //    {
    //        Console.WriteLine("Moving.");
    //    }

    //    public void Fire()
    //    {
    //        Console.WriteLine("Firing.");
    //    }

    //    public void Reload()
    //    {
    //        Console.WriteLine("Reloading.");
    //    }

    //    public void PrintStatus()
    //    {
    //        Console.WriteLine("No status yet.");
    //    }
    //}

}
