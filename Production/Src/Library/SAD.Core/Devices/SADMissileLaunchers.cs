﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BuildDefender;

namespace SAD.Core.Devices
{
    //IMissileLauncher interface
    public interface IMissileLauncher
    {
        void MoveBy(double phi, double theta);
        void MoveTo(double phi, double theta);
        void Fire();
        void Reload();
        void PrintStatus();

    }

    //Missile Launcher Type enummeration
    public enum MLType
    {
        DreamCheeky,
        Mock
    }

    //DreamCheeky type missile launcher Adaptor
    public class DreamCheeky : IMissileLauncher
    {
        public DreamCheeky()
        {
            LauncherName = "Target Eradicator";
            TotalMissiles = 4;
            MissileCount = TotalMissiles;
            m_launcher = new MissileLauncher();
            m_thetaPosition = 0.0;
            m_phiPosition = 0.0;
            //Launcher = new MissileLauncherAdapter();
        }
        public void MoveBy(double phi, double theta)
        {
            //Launcher.MoveBy(phi, theta);
            //doubles are passed in, but the ML methods takes ints
            phi = (phi / 22.2);
            theta = (theta / 22.2);
            if (thetaPosition == 0.0)
            {
                phiPosition += phi;
                thetaPosition += theta;
                int degrees = Convert.ToInt32(theta*22.2);
                int zdegrees = Convert.ToInt32(phi*22.2);

                if (degrees < 0) //negative values specify moving left
                {                //but ML doesn't like negative values.
                    degrees *= -1;
                }
                if (zdegrees < 0) //negative values specify moving down
                {
                    zdegrees *= -1;
                }
                if (theta > 0) //theta == positive move right
                {
                    m_launcher.command_Right(degrees);
                }
                else if (theta < 0) //theta == negtive move left
                {
                    m_launcher.command_Left(degrees);
                }
                else if (theta == 0) //theta == 0, don't move
                {                    //probably shouldn't call anything
                    m_launcher.command_Left(0);
                }
                if (phi > 0) //positive == up
                {
                    m_launcher.command_Up(zdegrees);
                }
                else if (phi < 0) //negative == down
                {
                    m_launcher.command_Down(zdegrees);
                }
                else if (phi == 0) //0 == dont move
                {
                    m_launcher.command_Up(0);
                }
            }
            else if (theta < thetaPosition)
            {
                var thing = thetaPosition - theta;
                phiPosition += phi;
                thetaPosition += thing;
                int degrees = Convert.ToInt32(thing*22.2);
                int zdegrees = Convert.ToInt32(phi*22.2);

                if (degrees < 0) //negative values specify moving left
                {                //but ML doesn't like negative values.
                    degrees *= -1;
                }
                if (zdegrees < 0) //negative values specify moving down
                {
                    zdegrees *= -1;
                }
                if (thing > 0) //theta == positive move right
                {
                    m_launcher.command_Right(degrees);
                }
                else if (thing < 0) //theta == negtive move left
                {
                    m_launcher.command_Left(degrees);
                }
                else if (thing == 0) //theta == 0, don't move
                {                    //probably shouldn't call anything
                    m_launcher.command_Left(0);
                }
                if (phi > 0) //positive == up
                {
                    m_launcher.command_Up(zdegrees);
                }
                else if (phi < 0) //negative == down
                {
                    m_launcher.command_Down(zdegrees);
                }
                else if (phi == 0) //0 == dont move
                {
                    m_launcher.command_Up(0);
                }


            }
            else if (theta > thetaPosition)
            {
                var thing = theta - thetaPosition;
                phiPosition += phi;
                thetaPosition += thing;
                int degrees = Convert.ToInt32(thing*22.2);
                int zdegrees = Convert.ToInt32(phi*22.2);

                if (degrees < 0) //negative values specify moving left
                {                //but ML doesn't like negative values.
                    degrees *= -1;
                }
                if (zdegrees < 0) //negative values specify moving down
                {
                    zdegrees *= -1;
                }
                if (thing > 0) //theta == positive move right
                {
                    m_launcher.command_Right(degrees);
                }
                else if (thing < 0) //theta == negtive move left
                {
                    m_launcher.command_Left(degrees);
                }
                else if (thing == 0) //theta == 0, don't move
                {                    //probably shouldn't call anything
                    m_launcher.command_Left(0);
                }
                if (phi > 0) //positive == up
                {
                    m_launcher.command_Up(zdegrees);
                }
                else if (phi < 0) //negative == down
                {
                    m_launcher.command_Down(zdegrees);
                }
                else if (phi == 0) //0 == dont move
                {
                    m_launcher.command_Up(0);
                }
            }
            //phi = phi - phiPosition;
            //theta = theta - thetaPosition;
            
        }
        public void MoveTo(double phi, double theta)
        {
            //Launcher.MoveTo(phi, theta);
            //reset the launcher
            m_launcher.command_reset();
            phiPosition = 0.0;
            thetaPosition = 0.0;
            //call moveby
            MoveBy(phi, theta);
        }

        public void Fire()
        {
            if (MissileCount < 1)
            {
                Console.WriteLine("I just can’t do it cap’tin, we just don’t have tha power!!");
            }
            else
            {
                Console.WriteLine("I'll give 'er all we got cap'tin!!");
                //Launcher.Fire();
                m_launcher.command_Fire();
                MissileCount--;

            }  
        }

        public void Reload()
        {
            //Console.WriteLine("Yaaarrrggghhh!!! I be realodin' the missiles right away cap'tin!");
            MissileCount = TotalMissiles;
        }

        public void PrintStatus()
        {
            Console.WriteLine("Launcher Name: {0}", LauncherName);
            Console.WriteLine("Missiles: {0} of 4 remain, cap'tin! Arrrrggghhhh!", MissileCount);
        }

        public int MissileCount { get; set; }
        public string LauncherName { get; set; }
        public double phiPosition { get; set; }
        public double thetaPosition { get; set; }
        private static int TotalMissiles;
        private static double m_phiPosition;
        private static double m_thetaPosition;
       // private Controller controller;
        //public IMissileLauncher Launcher { get; set; }
        private MissileLauncher m_launcher; //This is an object from the code in MissileLauncher.cs
    }

    //Mock type missile launcher
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
            Console.WriteLine("Name: Mock Launcher\nStatus: Married with children.");
        }
    }

    /* Missile Launcher controller
     * Instead of working with IMissileLauncher Launcher I think it has
     * to work with DreamCheeky type launcher instead, but it doesn't yet.
     */
    //public class Controller
    //{
    //    //These methods call the adapter methods.
    //    public void Fire()
    //    {
    //        Launcher.Fire();
    //    }
    //    public void MoveTo(double phi, double theta)
    //    {
    //        Launcher.MoveTo(phi,theta);
    //    }
    //    public void MoveBy(double phi, double theta)
    //    {
    //        Launcher.MoveBy(phi, theta);
    //    }
    //    public IMissileLauncher Launcher { get; set; }
    //}

    //This adapts the MissileLauncher.cs code to be used with an IMissileLauncher object
    //public class MissileLauncherAdapter : IMissileLauncher
    //{
    //    MissileLauncher m_launcher; //This is an object from the code in MissileLauncher.cs

    //    public MissileLauncherAdapter()
    //    {
    //        m_launcher = new MissileLauncher();
    //    }
    //    public void MoveBy(double phi, double theta)
    //    {
    //        //doubles are passed in, but the ML methods takes ints
    //        int degrees = Convert.ToInt32(theta);
    //        int zdegrees = Convert.ToInt32(phi);

    //        if (degrees < 0) //negative values specify moving left
    //        {                //but ML doesn't like negative values.
    //            degrees *= -1;
    //        }
    //        if (zdegrees < 0) //negative values specify moving down
    //        {
    //            zdegrees *= -1;
    //        }
    //        if (theta > 0) //theta == positive move right
    //        {
    //            m_launcher.command_Right(degrees);
    //        }
    //        else if (theta < 0) //theta == negtive move left
    //        {
    //            m_launcher.command_Left(degrees);
    //        }
    //        else if (theta == 0) //theta == 0, don't move
    //        {                    //probably shouldn't call anything
    //            m_launcher.command_Left(0);
    //        }
    //        if (phi > 0) //positive == up
    //        {
    //            m_launcher.command_Up(zdegrees);
    //        }
    //        else if (phi < 0) //negative == down
    //        {
    //            m_launcher.command_Down(zdegrees);
    //        }
    //        else if (phi == 0) //0 == dont move
    //        {
    //            m_launcher.command_Up(0);
    //        }
    //    }
    //    /* MoveTo moves a certain amount of degrees from a default position.
    //     * So the launcher should go to a default position then MoveBy the degrees.
    //     * Going to the default position with ML reset is pretty slow, so it probably
    //     * should be changed to something else maybe.
    //     */
    //    public void MoveTo(double phi, double theta)
    //    {
    //        //reset the launcher
    //        m_launcher.command_reset();
    //        //call moveby
    //        MoveBy(phi,theta);
    //    }

    //    public void Fire() //calls the ML fire method
    //    {
    //        m_launcher.command_Fire();
    //    }

    //    public void Reload() //ML doesn't have a reload command
    //    {
    //        //do nothing
    //    }

    //    public void PrintStatus() //ML doesn't have a print command
    //    {
    //        //do nothing
    //    }
    //}
}
