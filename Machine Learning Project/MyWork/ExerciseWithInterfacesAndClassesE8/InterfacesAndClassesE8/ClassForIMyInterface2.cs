using System;
using System.Collections.Generic;
using System.Text;

namespace InterfacesAndClassesE8
{
    public class ClassForIMyInterface2 : IMyInterface
    {
        Result result = new Result();

        public object GetResult()
        {
            return result;
        }

        public void Train(double[] data)
        {

            result.Variance = variance(data);
            result.Average = average(data);
            result.StandardDeviation = standardDeviation(result.Variance);
           // result.Median = Median(data);


        }

        public double average(double[] data)
        {
            int sum = 0;

            if (data.Length > 1)
            {


                foreach (int value in data)
                {
                    sum += value;
                }


                return sum / data.Length;
            }
            else { return data[0]; }
        }

        public double standardDeviation(double variance)
        {
            return Math.Sqrt(variance);
        }

        public double variance(double[] data)
        {
            if (data.Length > 1)
            {


                double avg = average(data);

                // Now figure out how far each point is from the mean
                // So we subtract from the number the average
                // Then raise it to the power of 2
                double sumOfSquares = 0.0;

                foreach (int i in data)
                {
                    sumOfSquares += Math.Pow((i - avg), 2.0);
                }

                // Finally divide it by n - 1 (for standard deviation variance)
                // Or use length without subtracting one ( for population standard deviation variance)
                return sumOfSquares / (double)(data.Length - 1);
            }
            else { return 0.0; }


            /*public double Median(double[] data)
            {
               
                
                    int size = data.Length - 1;
                    return ((data[size / 2] == data[(size / 2) + 1]) / 2);
                
            }
            */
        }
    }
}
