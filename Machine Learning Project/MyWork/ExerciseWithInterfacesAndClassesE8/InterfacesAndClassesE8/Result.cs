using System;
using System.Collections.Generic;
using System.Text;

namespace InterfacesAndClassesE8
{
    class Result
    {
        private double average;
        private double standardDeviation;
        private double variance;
        private double median;


        public double Average { get => average; set => average = value; }
        public double StandardDeviation { get => standardDeviation; set => standardDeviation = value; }
        public double Variance { get => variance; set => variance = value; }
        public double  Median { get => median; set => median = value; }
    }
}
