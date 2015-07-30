using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using System.Threading;

namespace PricerProj
{

    delegate double   simulate      (int steps, double initialPrice, int seed, IDescretized desc);
    delegate double[] simulateHist  (int steps, double initialPrice, int seed, IDescretized desc);

    public class MCGenerator
    {
        private IDescretized bm;

        public MCGenerator(double inMean, double inSigma, double inDeltaT = 1.0)
        {
            bm = new BrownianMotion(inMean, inSigma, inDeltaT);
        }

        //public double simulate(int steps, double initialPrice, int seed)
        //{
        //    double result        = initialPrice;
        //    RandomGen randomNorm = new RandomGen(seed);

        //    for (int i = 0; i < steps; ++i )
        //    {
        //        result = bm.NextPrice(result, randomNorm.NextDouble( bm.mean, bm.sigma ) );
        //    }

        //    return result;
        //}

        //public double[] simulateHist(int steps, double initialPrice, int seed)
        //{
        //    Random r             = new Random();
        //    double[] result      = new double[steps];
        //    RandomGen randomNorm = new RandomGen(seed);

        //    result[0] = initialPrice;
        //    for (int i = 1; i < steps; ++i)
        //    {
        //        result[i] = bm.NextPrice(result[i-1], randomNorm.NextDouble(bm.mean, bm.sigma));
        //    }
        //    return result;
        //}

        private static Object _lock = new Object();

        //simulate sim;
        //simulateHist simHist;

        public List<double> generatePaths(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            List<double> toReturn   = new List<double>(numberOfPaths);
            var indices             = Enumerable.Range(0, numberOfPaths);

            var rnd = new Random(42);
            List<int> seeds = new List<int> { numberOfPaths };
            for (int i = 0; i < numberOfPaths; ++i)
                seeds.Add(rnd.Next(1, numberOfPaths - 1));

            int steps = Convert.ToInt32 (Math.Floor(timeToExpiry / bm.deltaT));


            Parallel.ForEach(indices, ind =>
                {
                    int seed = seeds[ind];

                    //Thread.Sleep(10);

                    simulate mySim = new simulate(simulator.simulate);

                    //sim = (stps, init, sd, brm) =>
                    //{
                    //    double result = init;
                    //    RandomGen randomNorm = new RandomGen(sd);

                    //    for (int i = 0; i < stps; ++i)
                    //    {
                    //        result = brm.NextPrice(result, randomNorm.NextDouble(brm.mean, brm.sigma));
                    //    }

                    //    return result;
                    //};

                    double res = mySim(steps, initialPrice, seed, bm);

                    //Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "-" + ind + "-" + seeds[ind] + "-" + res);

                    lock (_lock)
                    {
                        toReturn.Add (res);
                    }
                }
            );

            return toReturn;
        }

        public double[][] generatePathsHist(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            int timeSteps = Convert.ToInt32(Math.Floor(timeToExpiry / bm.deltaT));

            double[][] toReturn = new double[numberOfPaths][];
            var indices         = Enumerable.Range(0, numberOfPaths);

            var rnd = new Random(42);
            List<int> seeds = new List<int> { numberOfPaths };
            for (int i = 0; i < numberOfPaths; ++i)
                seeds.Add(rnd.Next(1, numberOfPaths));

            Parallel.ForEach(indices, ind =>
            {
                int seed = seeds[ind];

                Thread.Sleep(100);

                simulateHist mySim = new simulateHist(simulator.simulateHist);

                //simHist = (stps, init, sd, brm) =>
                //{
                //    double[] result = new double[stps];
                //    RandomGen randomNorm = new RandomGen(sd);

                //    result[0] = initialPrice;
                //    for (int i = 1; i < stps; ++i)
                //    {
                //        result[i] = brm.NextPrice(result[i - 1], randomNorm.NextDouble(brm.mean, brm.sigma));
                //    }
                //    return result;
                //};

                toReturn[ind] = mySim(timeSteps, initialPrice, seeds[ind], bm);
            }
            );

            return toReturn;
        }

    }
    public class simulator
    {
        public static double simulate(int steps, double initialPrice, int seed, IDescretized bm)
        {
            double result = initialPrice;
            RandomGen randomNorm = new RandomGen(seed);

            for (int i = 0; i < steps; ++i)
            {
                result = bm.NextPrice(result, randomNorm.NextDouble(bm.mean, bm.sigma));
            }

            return result;
        }

        public static double[] simulateHist(int steps, double initialPrice, int seed, IDescretized bm)
        {
            Random r = new Random();
            double[] result = new double[steps];
            RandomGen randomNorm = new RandomGen(seed);

            result[0] = initialPrice;
            for (int i = 1; i < steps; ++i)
            {
                result[i] = bm.NextPrice(result[i - 1], randomNorm.NextDouble(bm.mean, bm.sigma));
            }
            return result;
        }

    }

}
