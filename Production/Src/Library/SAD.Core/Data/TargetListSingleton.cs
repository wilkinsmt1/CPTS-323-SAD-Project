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
 * This class may need modification; will be working on that. Let me know if there are any suggestions. -Anna
 */

namespace SAD.Core.Data
{
    public class TargetListSingleton
    {
        private static TargetListSingleton targetListSingletonInstance = null;

        private TargetListSingleton(){}

        public static TargetListSingleton GetInstance()
        {
            if (null == TargetListSingleton)   //error says is a type, but is used like a variable..
            {
                TargetListSingleton = new TargetListSingleton();
            }
            return TargetListSingleton;

        //if TargetListSingleton ?? (TargetListSingleton = new TargetListSingleton());
        //return TargetListSingleton;  //new instance assigned, and returned
        }
        public void WriteToConsole(string list)
        {
            Console.WriteLine(list);
        } 
      
    };
}
