﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAD.Core.Data;
using SAD.Core.Devices;
using SAD.Core.IO;

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
            string targetName = "";
            char[] delimiterChar = { ' ' };
            var DCLauncher = MLFactory.CreateMissileLauncher(MLType.DreamCheeky);
            FileReader iniReader = null;
            Console.WriteLine("The system has loaded.");
            Console.WriteLine("Argh! Ready ta fire Captain! Argh! Argghh! Arggggghhhhh!");

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
                        
                    }
                    phi = Convert.ToDouble(values[1]);
                    theta = Convert.ToDouble(values[2]);
                    DCLauncher.MoveTo((phi*22.2), (theta*22.2));
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
                        
                    }

                    phi = Convert.ToDouble(values[1]);
                    theta = Convert.ToDouble(values[2]);
                    DCLauncher.MoveBy((phi*22.2), (theta*22.2));
                }
                else if (command.StartsWith("RELOAD"))
                {
                    DCLauncher.Reload();
                }
                else if (command.StartsWith("LOAD"))
                {
                    var values = command.Split(delimiterChar);
                    try
                    {
                        if (values.Length < 2)
                        {
                            throw new Exception("Errorargh! Ye din't specify a file!");
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        Console.WriteLine(ex.Message);
                        
                    }
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
                    if (iniReader == null)
                    {
                        Console.WriteLine("Arrrggh! Ye din't load a file yet, you scallywag!");
                    }
                    else
                    {
                        iniReader.Scoundrels();
                    }
                }
                else if (command.StartsWith("FRIEND"))
                {
                    if (iniReader == null)
                    {
                        Console.WriteLine("Arrrggh! Ye din't load a file yet, you scallywag!");
                    }
                    else
                    {
                        iniReader.Friends();
                    }
                }
                else if (command.StartsWith("KILL"))
                {

                    if (iniReader == null)
                    {
                        Console.WriteLine("Arrrggh! Ye din't load a file yet, you scallywag!");
                    }
                    else
                    {

                        var values = command.Split(delimiterChar);
                        try
                        {
                            if (values.Length < 2)
                            {
                                throw new Exception("Errorargh! Ye din't specify a file!");
                            }
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex.Message);
                            
                        }
                        targetName = values[1];
                        bool friend = iniReader.isFriend(targetName);
                        if (friend)
                        {
                            Console.WriteLine("Sorry Captain, we don’t permit friendly fire, yar");
                        }
                        else
                        {
                            TargetManager targetManager = TargetManager.GetInstance();
                            double[] phitheta = targetManager.getCoordinates(targetName);
                            phi = phitheta[0];
                            theta = phitheta[1];
                            DCLauncher.MoveTo((phi * 22.2), (theta * 22.2));
                            //Console.ReadLine();
                            DCLauncher.Fire();

                            targetManager.changeStatus(targetName);
                        }
                    }
                    

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
