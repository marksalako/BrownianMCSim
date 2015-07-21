using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerProj
{
    public class BrownianMotion
    {
        public  double mean   { get; private set; }
        public  double sigma  { get; private set; }
        public  double deltaT { get; private set; }

        public BrownianMotion(double inMean, double inSigma, double inDeltaT )
        {
            mean    = inMean;
            sigma   = inSigma;
            deltaT  = inDeltaT;
        }

        private double PriceChange(double currentPrice, 
                                   double randomMove)
        {
            return currentPrice * mean * deltaT +
                   sigma * currentPrice * randomMove;
        }

        public double NextPrice(double currentPrice, double randomMove)
        {
            return Math.Max(currentPrice + PriceChange(currentPrice, randomMove), 0.0);
        }

    }
}
