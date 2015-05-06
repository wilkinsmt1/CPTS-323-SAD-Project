using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Data
{
   public class TargetPositioning
    {
        private static double theta;
        private static double phi;
        public static double CalculateTheta(double x, double y)
        {
            //Theta = arc cos(z/sqrt(x^2+y^2+z^2))
            var denominator = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            var num = x/denominator; 
            theta = Math.Acos(num);
            //convert from rads to degrees
            theta = (theta*57.2957795);
            //theta = Math.Round(theta, 2);
            if (y < 0)
            {
                theta *= -1;
            }
            return theta;
        }

        public static double CalculatePhi(double y, double z)
        {
            //Phi = arc tan(y/x)
            phi = Math.Atan2(z,y);
            //convert from rads to degrees
            phi = (phi*57.2957795);
            phi = Math.Round(phi, 2);
            return phi;
        }
    }
}

