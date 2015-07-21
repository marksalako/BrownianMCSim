using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PricerProj
{
    public class DifferentialEquation
    {
        private double r;
        private double vol;

        public DifferentialEquation(double rate, double v)
        { r = rate; vol = v; }

        public double drift(double x, double t)
        {
            return r * x;
        }

        public double diffusion(double x, double t)
        {
            return vol * x;
        }
    }
}
