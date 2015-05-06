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
    /*class AutoModeImpl : AutoModeBase
    {
        private MainViewModel mvmc;
        private int numConsecutiveFreinds = 0;

        ////begin here:
        public void loadTargets()
        {
            mvmc.LoadTargetsFromServer(); //load server or get data for next target
        }

        //TM:
        private void LoadTargetsAuto()
        {

            mvmc.LoadTargetsFromServer(); //get data for next target
            if (((TargetPositioning.CalculatePhi(x, y)) || (TargetPositioning.CalculateTheta(x, y))) < null)
            {
                Console.WriteLine("Negative Coordinates: Use Manual Mode!");
                manualMode();
            }
            else
            {
                moveToFirstTarget();
            }
        }
        public override void killSelectedTargets()
        {
            Console.WriteLine("Select: 1)Kill Enemies 2)Kill Friends 3)Kill All 4)Exit");
            int number = 0;

            Console.WriteLine("Kill Targets:");
            Console.WriteLine("1 - Friends");
            Console.WriteLine("2 - Foes");
            Console.WriteLine("3 - All");
            Console.WriteLine("4 - Exit Game");

            number = Convert.ToInt32(Console.ReadLine());

            switch (number)
            {
                case 1:
                    Console.WriteLine("Friends, Traitors! Walk the plank!");
                    mvm.KillFriends();
                    break;
                case 2:
                    Console.WriteLine("Scallywags, Walk the plank!");
                    mvm.KillEnemies();
                    break;
                case 3:
                    Console.WriteLine("Arrrggg, Walk the plank!");
                    mvm.KillAll();
                    break;
                case 4:
                    Console.WriteLine("Exiting Game!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("That is not a valid input!");
                    killSelectedTargets();
                    break;
            }
            //Console.Read(); //system pause
            return;
        }

        //other fns:
        private MainViewModel mvm;
        private int numConsecutiveFreinds = 0;
        //public override void manualMode(){
        //    //manual mode not implemented: use 'manual' gui controls
        //    return;
        //}
        //public override void targetHitorNot(){
        //    return;
        //}
        //public override void targetHasMissilesOrNot(){
        //    return;
        //}
        public override void killSelectedTargets()
        {
            Console.WriteLine("Select: 1)Kill Enemies 2)Kill Friends 3)Kill All 4)Exit");
            int number = 0;

            Console.WriteLine("Kill Targets:");
            Console.WriteLine("1 - Friends");
            Console.WriteLine("2 - Foes");
            Console.WriteLine("3 - All");
            Console.WriteLine("4 - Exit Game");

            number = Convert.ToInt32(Console.ReadLine());

            switch (number)
            {
                case 1:
                    Console.WriteLine("Friends, Traitors! Walk the plank!");
                    mvm.KillFriends();
                    break;
                case 2:
                    Console.WriteLine("Scallywags, Walk the plank!");
                    mvm.KillEnemies();
                    break;
                case 3:
                    Console.WriteLine("Arrrggg, Walk the plank!");
                    mvm.KillAll();
                    break;
                case 4:
                    Console.WriteLine("Exiting Game!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("That is not a valid input!");
                    killSelectedTargets();
                    break;
            }
            //Console.Read(); //system pause
            return;
        }

        ////begin here:
        //public void loadTargets()
        //{
        //    mvm.LoadTargetsFromServer(); //load server or get data for next target
        //}
        //
        ////TM:
        //private void LoadTargetsAuto()
        //{
        //
        //    mvm.LoadTargetsFromServer(); //get data for next target
        //    if (((TargetPositioning.CalculatePhi(x, y)) || (TargetPositioning.CalculateTheta(x, y))) < null)
        //    {
        //        Console.WriteLine("Negative Coordinates: Use Manual Mode!");
        //        manualMode();
        //    }
        //    else
        //    {
        //        moveToFirstTarget();
        //    }
        //}

        private void moveToFirstTarget()
        {
            //use first target
            if (TargetCollection.target.Target.IsFriend)
            {
                friend();
            }
            else
            {
                foe();
            }
        }

        private void moveToNextTarget()
        {
            if ((numFriends > 10) || (timer > 1)) //or > 4 in a row?
            {
                return;
            }
            else
            {
                if (friend)
                {
                    numConsecutiveFreinds += 1;
                    friend();
                }
                else
                {
                    numConsecutiveFreinds = 0;
                    foe();
                }
            }
        }
        private void friend()
        {
            if (numConsecutiveFreinds > 4)
            {
                Console.WriteLine("All targets are friends; exit game"); //exit here or not?
                exit(0);
            }
            else
            {
                moveToNextTarget();
            }
        }

        private void foe()
        {
            if (mvm.numberOfMissilesLeft < 0)
            {
                noMissiles();
            }
            else
            {
                hasMissiles();
            }
        }
        private void noMissiles()
        {
            reload();
        }
        //foreach (var target in TargetsCollection)
        //{
        //    m_missileLauncherCommandQueue.Enqueue(new MyCommands(() =>
        //    {
        //        KillTheTargets(target.Target);
        //    }));
        //}
        private void reload()
        {
            string name;
            //user input: press enter
            Console.WriteLine("Press ENTER after reloading turrets!");
            name = Console.ReadLine();
            moveToNextTarget();
        }
        private void hasMissiles()
        {

            mvm.KillTheTargets(target.Target); //fire one turret
            //if (hit) //how to determine this? if server says a target is hit, then it is hit.
            //{
            //    hit();
            //}
            //else
            //{
            //    miss();
            //}
        }
        private void hit()
        {
            if (mvm.canSwapSides == true)
            {
                canSwapSides();
            }
            else
            {
                cannotSwapSides();
            }
        }

        private void miss()
        {
            if (numMissiles < null)
            {
                noMissiles();
            }
            else
            {
                hasMissiles();
            }
        }

        private void canSwapSides()
        {
            friend = true;
            friend();
        }
        private void cannotSwapSides()
        {
            foe();
        }
    }*/
}