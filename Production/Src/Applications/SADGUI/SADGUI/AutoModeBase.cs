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
    //public class AutoModeBase
   abstract class AutoModeBase : MainViewModel 
    {
        //public abstract void manualMode();
        //public abstract void targetHitorNot();
        //public abstract void targetHasMissilesOrNot();
        public abstract void killSelectedTargets();
        public abstract void moveToFirstTarget();
        public abstract void moveToNextTarget();
        public abstract void friend();
        public abstract void foe();
        public abstract void noMissiles();
        public abstract void reload();
        public abstract void hasMissiles();
        public abstract void hit();
        public abstract void miss();
        public abstract void canSwapSides();
        public abstract void cannotSwapSides();

    };
}