using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAD.Core.Devices;
using SAD.Core.IO;

namespace SAD.Core.Devices
{
    //Factory that creates Missile Launchers
    public sealed class MLFactory
    {
        /// <summary>
        /// Creates a new missile launcher based on what is passed in
        /// </summary>
        /// <param name="type">The type of missile launcher to be created.</param>
        /// <returns>Returns the newly created missile launcher.</returns>
        public static IMissileLauncher CreateMissileLauncher(MLType type)
        {
            IMissileLauncher ML = null;
            switch (type)
            {
                case MLType.DreamCheeky:
                    ML = new DreamCheeky();
                    break;
                case MLType.Mock:
                    ML = new Mock();
                    break;
            }
            return ML;
        }
    }

    //Factory that creates File readers
    public sealed class FRFactory
    {
        /* This hasn't been tested with the INIReader code and
         * JSON and XML support hasn't been added.
         * So obviously it needs work.
         */
        public static FileReader CreateReader(FRType type, string path)
        {
            FileReader reader = null;
            switch (type)
            {
                case FRType.INIReader:
                    reader = new INIReader(path);
                    break;
                case FRType.JSONReader:
                    Console.WriteLine("Not Implemented yet, sorry.");
                    //reader = new JSONReader();
                    break;
                case FRType.XMLReader:
                    Console.WriteLine("Not Implemented yet, sorry.");
                    //reader = new XMLReader();
                    break;
            }
            return reader;

        }

        //private static string path = "";
        
    }

    
}
