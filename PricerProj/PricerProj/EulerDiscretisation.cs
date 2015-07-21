using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricerProj
{
    class EulerDiscretisation : Discretisation
    {
        public EulerDiscretisation(DifferentialEquation de,
                                   double initial,
                                   double toExpiry)
            : base(de, initial,toExpiry)
        { }

        public override double step(double x, double t, double dt, double rand)
        {
            return x;
        }

    }
}
