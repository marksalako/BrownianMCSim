using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using System.Threading;
using System.Collections.Concurrent;

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

        private static Object _lock = new Object();
        private static Object _lock2 = new Object();

        //simulate sim;
        //simulateHist simHist;

        public ConcurrentBag<double> generatePaths(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            ConcurrentBag<double> toReturn = new ConcurrentBag<double>{};
            //var indices                    = Enumerable.Range(0, numberOfPaths);

            var rnd = new Random(42);
            ConcurrentStack<int> seeds = new ConcurrentStack<int> {};
            for (int i = 0; i < numberOfPaths; ++i)
                seeds.Push(rnd.Next(1, numberOfPaths - 1));

            int steps = Convert.ToInt32 (Math.Floor(timeToExpiry / bm.deltaT));


            Parallel.ForEach(seeds, 
                             //new ParallelOptions { MaxDegreeOfParallelism = 2 },
                             seed =>
                {
                    Thread.Sleep(1);
                    
                    simulate mySim = new simulate(simulator.simulate);

                    double res = new double();
                    res = mySim(steps, initialPrice, seed, bm);

                    toReturn.Add(res);
                }
            );

            return toReturn;
        }

        public ConcurrentBag<double[]> generatePathsHist(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            int timeSteps = Convert.ToInt32(Math.Floor(timeToExpiry / bm.deltaT));

            ConcurrentBag<double[]> toReturn = new ConcurrentBag<double[]> {};
            var indices             = Enumerable.Range(0, numberOfPaths);

            var rnd = new Random(42);
            List<int> seeds = new List<int> { numberOfPaths };
            for (int i = 0; i < numberOfPaths; ++i)
                seeds.Add(rnd.Next(1, numberOfPaths));

            Parallel.ForEach(indices,
                             new ParallelOptions { MaxDegreeOfParallelism = 1 },
                             ind =>
            {
                int seed;
                seed = seeds[ind];
                
                Thread.Sleep(10);

                simulateHist mySim = new simulateHist(simulator.simulateHist);
                var res = new double[timeSteps];

                res = mySim(timeSteps, initialPrice, seeds[ind], bm);

                toReturn.Add(res);
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
