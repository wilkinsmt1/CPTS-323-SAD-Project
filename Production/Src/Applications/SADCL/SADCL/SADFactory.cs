using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAD.Core.Devices;
using SAD.Core.IO;

namespace SADCL
{
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

    public sealed class FRFactory
    {
        public static FileReader CreateReader(FRType type)
        {
            FileReader reader = null;
            switch (type)
            {
                case FRType.INIReader:
                    reader = new INIReader(path);
                    break;
                case FRType.JSONReader:
                    reader = new JSONReader();
                    break;
                case FRType.XMLReader:
                    reader = new XMLReader();
                    break;
            }
            return reader;

        }

        private static string path = "";
        
    }

    
}
