using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Data
{
    public class Targets // This is the Targets class for the list of Targets objects
    {                    // The class has name, x, y, z, isfriend, points, flashrate, spawn rate
                         // and swap values as auto properties!
        public string TargetName { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool IsFriend { get; set; }
        public int Points { get; set; }
        public int FlashRate { get; set; }
        public int SpawnRate { get; set; }
        public bool CanSwapSidesWhenHit { get; set; }
    }
}
