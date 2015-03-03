using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAD.Core.Devices;
using SAD.Core.IO;
using SAD.Core.Data; //testing

//if running from cmd line, need 3 args (program name, theta, phi). 
//if running from windows console, need 2 args (phi, theta; then set 3rd value to zero)
//theta (left/right) must be between 0-3000. 
//phi   (  up/down ) must be between 0-480
//it seems to not be accurate over time, and is not linear, but has some curve(s) in the graph
//also, calibrating more often could help, depending on how far away the targets are. 
//The ML seems to have errors occasionally, and goes to completely wrong coordinates.

namespace SADCL
{
    class Program
    {
        static void Main(string[] args) //added argc, for number of arguments in cmd line
        {
            string filePath = "";
            string command = "";
            double phi = 0.0;
            double theta = 0.0;
            char[] delimiterChar = { ' ' };
            var DCLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            //Targets tgt;
            //bool friendly = tgt.GetType();

            TargetPositioning tp = new TargetPositioning();
            Console.WriteLine(tp.GetNewTheta(1, 2, 3));
            Console.WriteLine(tp.GetNewTheta(1, 2, 3)); 

            FileReader iniReader = null;
            Console.WriteLine("The system has loaded.");
            Console.WriteLine("Argh! Ready ta fire Captain! Argh! Argghh! Arggggghhhhh!");
            Console.WriteLine("Your Options are FIRE, MOVE <0-480> <0-3000> <0>, MOVEBY, EXIT, Arrggh! \n");
            Console.WriteLine("RELOAD, SCOUNDRELS, FRIEND, KILL, STATUS, \n");
            Console.WriteLine("OR would YE rather WALK the PLANK? Argghhh!");
            

            while (command.ToUpper() != "EXIT")
            {
                Console.Write(">");
                command = Console.ReadLine();
                command = command.ToUpper();
                if (command.StartsWith("FIRE"))
                {
                    DCLauncher.Fire();
                }
                else if (command.StartsWith("MOVE "))
                {
                    var values = command.Split(delimiterChar);
                    try
                    {
                        if (values.Length < 3) 
                        {
                            throw new Exception("Errorargggghh!! Ye di'not enter a valid phi or theta");
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                        return;
                    }
                    phi = Convert.ToDouble(values[1]); 
                    theta = Convert.ToDouble(values[2]);
                    DCLauncher.MoveTo(phi,theta);
                }
                else if (command.StartsWith("MOVEBY"))
                {
                    var values = command.Split(delimiterChar);
                    try
                    {
                        if (values.Length < 3)
                        {
                            throw new Exception("Errorargggghh!! Ye di'not enter a valid phi or theta");
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                        return;
                    }

                    phi = Convert.ToDouble(values[1]);
                    theta = Convert.ToDouble(values[2]);
                    DCLauncher.MoveBy(phi, theta);
                }
                else if (command.StartsWith("RELOAD"))
                {
                    DCLauncher.Reload();
                }
                else if (command.StartsWith("LOAD"))
                {
                    var values = command.Split(delimiterChar);
                    
                    filePath = values[1];
                    var fileExists = System.IO.File.Exists(filePath);
                    if (!fileExists)
                    {
                        Console.WriteLine("Yarrgh! No file exists! Arrrrgh!!");
                    }
                    else
                    {
                        Console.WriteLine("Argghh! Argghh! Thar be new targets in sight, cap'tin.... Argh!");
                        iniReader = FRFactory.CreateReader(FRType.INIReader, filePath);
                    }
                }
                else if (command.StartsWith("SCOUNDRELS"))
                {
                    iniReader.Scoundrels();
                }
                else if (command.StartsWith("FRIEND"))
                {
                    iniReader.Friends();
                }
                else if (command.StartsWith("KILL"))
                {
                    /* This command should check if the target specified is a friend
                     * if it is a friend:
                     * 1. it should move the turret relative to the coordinates
                     * 2. it should then fire a missile.
                     * 3. it should then remove the target from the target tracker
                     * if it is a friend:
                     * print: “Sorry Cap'tin, we don’t permit friendly fire, yargh!”
                     */
                }
                else if (command.StartsWith("STATUS"))
                {
                    DCLauncher.PrintStatus();
                }
                else
                {
                    Console.WriteLine("Errorarggh!, di'not enter a valid command! Yarrrgggghhhh!!!");
                }
            }



             ////create missile launcher using factory:
             //var dClaLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
             //var mLauncher = MLFactory.CreateMissileLauncher(MLType.Mock);
             ////create file reader using factory:
             //var iniReader = FRFactory.CreateReader(FRType.INIReader, filePath);
             //var jsonReader = FRFactory.CreateReader(FRType.JSONReader, filePath);
             //var xmlReader = FRFactory.CreateReader(FRType.XMLReader, filePath);
             ////using adapter to control missile launcher:
             //var controller = new Controller();
             //controller.Launcher = new MissileLauncherAdapter();
             ////fire
             //controller.Fire();
             ////moveto
             //controller.MoveTo(0.0, 0.0);
             ////moveby
             //controller.MoveBy(0.0, 0.0);
             ////Reload command:
             //dClaLauncher.Reload();
             //mLauncher.Reload();
             ////Status command:
             //dClaLauncher.PrintStatus();
             //mLauncher.PrintStatus();
        }
    }
}
