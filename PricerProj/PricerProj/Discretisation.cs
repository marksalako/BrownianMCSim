using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerProj
{
    public abstract class Discretisation
    {
        public double initialValue           { get; private set; }
        public double timeToExpiry           { get; private set; }

        public DifferentialEquation diffEqn  { get; private set; }

        public Discretisation(DifferentialEquation de,
                              double initial,
                              double expiry)
        {
            initialValue = initialValue;
            timeToExpiry = expiry;
            diffEqn      = de;
        }

        public abstract double step(double x, double t, double dt, double rand);

    }
}
