using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAD.Core.Devices;
using SAD.Core.IO;

namespace SADCL
{
    public class MLFactory
    {
        public MissileLauncher CreateMissileLauncher()
        {
            MissileLauncher ML = new MissileLauncher();
            return ML;
        }

        public FileReader CreateReader()
        {
            FileReader reader = null;
            reader = new INIReader(path);
            return reader;

        }

        public FileReader CreateIniReader(string path)
        {
            return new INIReader(path);
        }

        private string path = "";
    }
}
