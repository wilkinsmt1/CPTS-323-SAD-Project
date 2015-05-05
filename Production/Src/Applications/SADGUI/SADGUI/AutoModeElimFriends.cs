﻿using System;
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
    class AutoModeElimFriends : AutoModeBase
    {
        Targets target;

        public ObservableCollection<TargetViewModel> TargetsCollection { get; private set; }
        private MainViewModel mvmc;
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
            return;
        }

        ////begin here: in automodeimpl 
        //public void loadTargets()
        //{
        //        moveToFirstTarget();//  
        //}

        //begin here in this class:
        public override void moveToFirstTarget()
        {
            //use first target
            if (target.IsFriend)  //target.Target.IsFriend)
            {
                friend();
            }
            else
            {
                foe();
            }
        }

        public override void moveToNextTarget()
        {
            if (numConsecutiveFreinds > 4) //|| (timer > 1)) //or > 4 in a row?
            {
                return;
            }
            else
            {
                if (target.IsFriend)
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
        public override void friend()
        {
            if (numConsecutiveFreinds > 4)
            {
                Console.WriteLine("All targets are friends: Exit game"); //exit here or not?
                //Exit();
                Environment.Exit(0);
            }
            else
            {
                moveToNextTarget();
            }
        }

        public override void foe()
        {
            if (mvmc.MissileCount.Equals(0)) //numberOfMissilesLeft < 1)  //=============================================
            {
                noMissiles();
            }
            else
            {
                hasMissiles();
            }
        }
        public override void noMissiles()
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
        public override void reload()
        {
            string name;
            //user input: press enter
            MessageBox.Show("Reload Missiles! ");
            //Console.WriteLine("Press ENTER after reloading turrets!");
            //name = Console.ReadLine();
            moveToNextTarget();
        }
        public override void hasMissiles()
        {

            mvmc.KillFriends(); //fire one turret ... or all..
            return;
            //if (hit) //how to determine this? if server says a target is hit, then it is hit.
            //{
            //    hit();
            //}
            //else
            //{
            //    miss();
            //}
        }
        public override void hit()
        {
            if (target.CanSwapSidesWhenHit == true)
            {
                canSwapSides();
            }
            else
            {
                cannotSwapSides();
            }
        }

        public override void miss()
        {
            if (mvmc.MissileCount.Equals(0))  //how to write this? 
            {
                noMissiles();
            }
            else
            {
                hasMissiles();
            }
        }

        public override void canSwapSides()
        {
            target.IsFriend = true;
            friend();
        }
        public override void cannotSwapSides()
        {
            foe();
        }
    };
}