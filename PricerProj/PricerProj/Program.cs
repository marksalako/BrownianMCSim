using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerProj
{
    class Program
    {
        static void Main(string[] args)
        {
            int numOfPaths = 100000;

            int timeToExpiry = 90;

            double spot = 1.5;

            double mean = Math.Pow(1.05, (1.0 / 252.0)) - 1.0;

            //double stdDev = 0.1 / Math.Sqrt(252.0);

            double stdDev = 0.25;

            MCGenerator monty = new MCGenerator(0.0, 1.0, 1.0/10 );

            double[] results = monty.generatePaths(spot, numOfPaths, timeToExpiry);

            double average = results.Average();


            Console.Write(average);
            Console.Read();
        }
    }
}