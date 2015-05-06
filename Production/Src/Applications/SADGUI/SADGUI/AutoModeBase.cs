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
   interface IAutoModeBase //: MainViewModel 
    {
        //public abstract void manualMode();
        //public abstract void targetHitorNot();
        //public abstract void targetHasMissilesOrNot();
       void killSelectedTargets();
        void moveToFirstTarget();
        void moveToNextTarget();
        void friend();
        void foe();
        void noMissiles();
        void reload();
        void hasMissiles();
        void hit();
        void miss();
        void canSwapSides();
        void cannotSwapSides();

    };
}