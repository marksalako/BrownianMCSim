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
        private BrownianMotion bm;

        public MCGenerator(double inMean, double inSigma, double inDeltaT = 1.0)
        {
            bm = new BrownianMotion(inMean, inSigma, inDeltaT);
        }

        public double simulate(int steps, double initialPrice, int seed)
        {
            Random r = new Random();
            double result = initialPrice;
            for (int i = 0; i < steps; ++i )
            {
                //Normal normal = Normal.WithMeanStdDev(0, 1);
                RandomGen randomNorm = new RandomGen(seed);

                result = bm.NextPrice(result, randomNorm.NextDouble(bm.mean, bm.sigma) );
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

    }
}
