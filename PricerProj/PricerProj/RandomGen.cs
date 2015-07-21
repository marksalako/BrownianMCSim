using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerProj
{
    public class RandomGen
    {
        Random _rng         = new Random();
        double? _spareValue = null;

        public RandomGen(int seed)
        {
            Random _rng = new Random(seed * 42);
        }

        /// <summary>
        /// Get the next sample point from the gaussian distribution.
        /// </summary>
        public double NextDouble()
        {
            if (null != _spareValue)
            {
                double tmp = _spareValue.Value;
                _spareValue = null;
                return tmp;
            }

            double v1, v2, sqr;

            do
            {
                v1 = 2.0 * _rng.NextDouble() - 1.0;
                v2 = 2.0 * _rng.NextDouble() - 1.0;
                sqr = v1 * v1 + v2 * v2;
            }
            while (sqr > 1.0 || sqr == 0);

            double fac = Math.Sqrt(-2.0 * Math.Log(sqr) / sqr);

            _spareValue = v1 * fac;
            return v2 * fac;
        }

        ///// <summary>
        ///// Get the next sample point from the gaussian distribution.
        ///// </summary>
        public double NextDouble(double mu, double sigma)
        {
            var t = mu + (NextDouble() * sigma);
            //Console.WriteLine(t);
            return t;
        }

    }

    
}
