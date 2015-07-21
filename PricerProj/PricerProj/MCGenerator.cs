using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;

namespace PricerProj
{
    public class MCGenerator
    {
        private IDescretized bm;

        public MCGenerator(double inMean, double inSigma, double inDeltaT = 1.0)
        {
            bm = new BrownianMotion(inMean, inSigma, inDeltaT);
        }

        public double simulate(int steps, double initialPrice, int seed)
        {
            Random r = new Random();
            double result = initialPrice;
            RandomGen randomNorm = new RandomGen(seed);
            
            for (int i = 0; i < steps; ++i )
            {
                result = bm.NextPrice(result, randomNorm.NextDouble( bm.mean, bm.sigma ) );
            }

            return result;
        }

        public double[] simulateHist(int steps, double initialPrice, int seed)
        {
            Random r             = new Random();
            double[] result      = new double[steps];
            RandomGen randomNorm = new RandomGen(seed);

            result[0] = initialPrice;
            for (int i = 1; i < steps; ++i)
            {
                result[i] = bm.NextPrice(result[i-1], randomNorm.NextDouble(bm.mean, bm.sigma));
            }
            return result;
        }


        public double[] generatePaths(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            double[] toReturn = new double[numberOfPaths];
            var indices       = Enumerable.Range(0, numberOfPaths);

            Parallel.ForEach(indices, ind =>
                {
                    toReturn[ind] = simulate( Convert.ToInt32 (Math.Floor(timeToExpiry / bm.deltaT)),
                                              initialPrice,
                                              ind);
                }
            );

            return toReturn;
        }

        public double[][] generatePathsHist(double initialPrice, int numberOfPaths, double timeToExpiry)
        {
            int timeSteps = Convert.ToInt32(Math.Floor(timeToExpiry / bm.deltaT));

            double[][] toReturn = new double[numberOfPaths][];
            var indices = Enumerable.Range(0, numberOfPaths);

            Parallel.ForEach(indices, ind =>
            {
                toReturn[ind] = simulateHist(timeSteps,
                                             initialPrice,
                                             ind);
            }
            );

            return toReturn;
        }

    }
}
