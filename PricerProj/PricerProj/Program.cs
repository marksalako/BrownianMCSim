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
            Console.WriteLine("-----Call Option Pricer-----");
            Console.WriteLine("Please enter the following values...");

            Console.Write("Spot: ");
            double spot = Convert.ToDouble(Console.ReadLine());

            Console.Write("Strike: ");
            double strike = Convert.ToDouble(Console.ReadLine());

            Console.Write("Time to Expiry(in days): ");
            int timeToExpiry = Convert.ToInt32(Console.ReadLine());

            Console.Write("Interest rate (eg 0.03): ");
            double interest = Convert.ToDouble(Console.ReadLine());

            Console.Write("Volatility (eg 0.25): ");
            double vol = Convert.ToDouble(Console.ReadLine());


            int numOfPaths = 100000;


            double mean = Math.Pow((1.0 + interest), (1.0 / 252)) - 1.0;

            double stdDev = vol / Math.Sqrt(252);

            MCGenerator monty = new MCGenerator(mean, stdDev, 1.0);

            double[] results = monty.generatePaths(spot, numOfPaths, timeToExpiry);

            double average = results.Average();

            //Console.Write(average);
            Console.Write("Call price: ");
            Console.WriteLine(Math.Max(average - strike, 0.0));
            Console.WriteLine("Max path-end: {0}", results.Max().ToString());
            Console.WriteLine("Min path-end: {0}", results.Min().ToString());
            Console.WriteLine("Standard Error: {0}", stdDev / Math.Sqrt(numOfPaths));

            Console.Read();
        }
    }
}