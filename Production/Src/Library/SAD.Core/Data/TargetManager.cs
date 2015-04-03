using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TargetListSingleton class:
 * To get an instance of this class, use this:  TargetListSingleton targetListSingletonInstance = TargetListSingleton.GetInstance();  
 * Singleton Info: p127 text; A singleton is used to allow only one instance of the class, and contain a global access point. 
 * There should only be one list of targets; this will prevent multiple lists.
 * This class may need modification; will be working on that. Let me know if there are any suggestions. -Anna
 */

namespace SAD.Core.Data
{
    public class TargetManager
    {
        //int a;
        private static TargetManager targetListInstance; //Private constant instance
        public ObservableCollection<Targets> TargetList { get;  set; }
        //private string TargetStatus;

        private TargetManager() //Private constructor
        {
            
        }

        public static TargetManager GetInstance() //Public method getInstance
        {
            if (targetListInstance == null)
            {
                targetListInstance = new TargetManager();
            }
            return targetListInstance;

        //if TargetListSingleton ?? (TargetListSingleton = new TargetListSingleton());
        //return TargetListSingleton;  //new instance assigned, and returned
        }

        //public void changeStatus(string targetName)
        //{
        //    Targets result = TargetList.Find(i => i.TargetName.ToUpper() == targetName);
        //    result.Status = "He's dead jim.";
        //}

        //public double[] getCoordinates(string targetName)
        //{
        //    Targets result = TargetList.Find(i => i.TargetName.ToUpper() == targetName);
        //    double x, y, z, theta, phi;
            
        //    x = result.X;
        //    y = result.Y;
        //    z = result.Z;
        //    theta = TargetPositioning.calculateTheta(x , y, z);
        //    phi = TargetPositioning.calculatePhi(x, y);
        //    double[] ptArray = new double[2]{phi,theta};
        //    return ptArray;
        //}
    }
}
