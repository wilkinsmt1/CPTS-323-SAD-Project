using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAD.Core.Data
{
    public class TargetPositioning
    {
        //function:     GetNewTheta
        //arguments:    double x, y, z coordinates        //the two new functions are not giving the correct output yet. may change units to int later.
        //returns:      theta
        //conversion:   according to the wikipage given:
        public double GetNewTheta(double y = 0.1, double x = 0.1, double z = 0.1) //phi, theta, in this order
        {
            double t = 0.0;          
            x = x * x;
            y = y * y;
            t = z;
            z = z * z;
            z = x + y + z;
            z = Math.Sqrt(z); // sqrt(x^2 + y^2 + z^2)
            z = t / z;
            z = Math.Acos(z);            
            return (z * (180 / (Math.PI))); //returns new value for theta
        }

        //function:     GetNewPhi
        //arguments:    double x, y, z coordinates
        //returns:      phi
        //conversion:   according to the wikipage given:
        public double GetNewPhi(double y = 0.1, double x = 0.1, double z = 0.1) //phi, theta, in this order
        {
            x = y / x;
            x = Math.Atan(x);
            return (x * (180 / (Math.PI)));   //returns new value for phi
        }



        /* Function:    CalculateTheta
         * Arguments:   double x-coordinate, double y-coordinate
         * Returns:     double theta (degrees)
         * Example:     theta = tp.CalculateTheta(3.0);                                     // TargetPositioning tp = new TargetPositioning();
         * Note:        The camera should be positioned directly in line with the target;
         *              The missile launcher will be to the left or right of the camera;
         *              Input will be: 
         *                  double x = distance between the camera and the missile launcher.
         *                  double y = distance between the camera and the target.
         */
        public double CalculateTheta(double y = 10.0, double x = 10.0) //changed to phi, theta
        {
            double theta;                                                                   // Angle on the horizontal
            double div;
            div = y / x;
            theta = (Math.Atan(div));     //16.699244234 degrees; 
            theta = theta * (180 / (Math.PI));
            //convert double angle to int seconds:
            return theta;  //returning double, not int
        }

        /* Function:    CalculatePhi
         * Arguments:   double x-coordinate, double y-coordinate
         * Returns:     double phi (degrees)
         * Example:     phi = tp.CalculatePhi(3.0);                                         // TargetPositioning tp = new TargetPositioning();
         * Note:        The camera should be positioned directly in line with the target;
         *              The missile launcher will be to the left or right of the camera;
         *              Input will be: 
         *                  double x = distance between the camera and the missile launcher.
         *                  double y = distance between the camera and the target.
         */
        public double CalculatePhi(double y = 10.0, double x = 10.0)   //changed to phi, theta  //move, shoot , commands work.: next gui and camera?
        {
            int exp = 2;
            double phi = 0.0;                                                               // Angle above the horizontal
            double hyp = 0.0;
            double yDivByHyp = 0.0;
            double xSqPlusYsquared = 0.0;
            xSqPlusYsquared = (Math.Pow(x, exp)) + (Math.Pow(y, exp));                      // x^2 + y^2
            hyp = (Math.Sqrt(xSqPlusYsquared));                                             // Sqrt(x^2 + y^2)
            yDivByHyp = y / hyp;
            phi = Math.Atan(yDivByHyp);
            phi = phi * (180 / (Math.PI));
            phi = 90 - phi; // will check this again
            //convert double angle to int seconds:
            return phi; //returning a double, not int. is double necessary, or is int better??
        }

    }
}

