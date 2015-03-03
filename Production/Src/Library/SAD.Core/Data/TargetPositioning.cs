using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Data
{
    class TargetPositioning
    {
        public static double convertRadsToDegs(double radAngle) //convert radians to degrees
        {
            double degrees = ((radAngle*180)/Math.PI);
            return degrees;
        }
        public static double calculateRadius(double x, double y, double z) //radius = Sqrt(x^2 + y^2)
        {
            double xSqr = Math.Pow(x,2);
            double ySqr = Math.Pow(y, 2);
            double radius = Math.Sqrt(xSqr + ySqr);
            return radius;
        }
        public static double calculateTheta(double x, double y, double z)
        {
            double radius = calculateRadius(x, y, z);
            double radians = (Math.Atan2(z, radius));
            double theta = convertRadsToDegs(radians);
            return theta;
        }
        public static double calculatePhi(double x, double y)
        {
            double radians = Math.Atan2(x, y);
            double phi = convertRadsToDegs(radians);
            if (phi > 90)
            {
                phi = (phi - 90) * -1; 
            }
            return phi;
        }
    }
}

