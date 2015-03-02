using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Test
{
    class UnitTest1
    {
        static void TestMethod2(string[] args)
        {


            double theta;
            double phi;
            MissileLauncher ml = new IMissileLauncher();
            TargetPositioningAngles tpa = new TargetPositioningAngles(); //TargetPositioningAngles class, with spherical coordinates (rho, theta, phi)
            Targets tar = new Targets();
            MLFactory mlf = new MLFactory();
            TargetPositioningAngles tp = new TargetPositioningAngles(); //TargetPositioning class, get theta, phi
            UsbLibrary.Win32Usb usb = new UsbLibrary.Win32Usb();

            theta = tp.CalculateRho(2.0, 3.0, 4.0);
            phi = tp.CalculatePhi(2.0, 3.0, 4.0);
            Console.WriteLine("Theta = " + theta);   //example: y=3; xadj=10
            Console.WriteLine("Phi   = " + phi);
            Console.Read();
            //ML.MoveTo();
            tar.internX(1);
            tar.internY(1);
            tar.internZ(1);
            ml.Fire();//change
        }
    }
}
