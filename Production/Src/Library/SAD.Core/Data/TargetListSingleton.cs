using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * TargetListSingleton class:
 * To get an instance of this class, use this:  TargetListSingleton targetListSingletonInstance = TargetListSingleton.GetInstance();  
 * Singleton Info: p127 text; A singleton is used to allow only one instance of the class, and contain a global access point. 
 * There should only be one list of targets; this will prevent multiple lists.
 * 
 */
namespace SAD.Core.Data
{

    public class TargetListSingleton //: Targets   //class
    {
        public static TargetListSingleton targetListInstance; // = null

        private TargetListSingleton() { }

        public static TargetListSingleton GetInstance()
        {
            if (targetListInstance == null)
            {
                List<Targets> targetList = new List<Targets>();
                //targetListInstance = new TargetListSingleton(); //if no instance, then create an instance
            }
            return targetListInstance;
        }
    };
}
