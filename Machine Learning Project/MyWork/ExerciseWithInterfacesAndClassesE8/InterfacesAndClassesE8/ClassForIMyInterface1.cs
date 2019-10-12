using System;
using System.Collections.Generic;
using System.Text;

namespace InterfacesAndClassesE8
{
    public class ClassForIMyInterface1 : IMyInterface
    {

        int result;


        public object GetResult()
        {

            return result;
        }

        /*method calculate sum of all values specified in data and set result
         to 1 if the sum is  greater than 100 and 0 if sum is less than 100*/

        public void Train(double[] data)
        {
            
            double sum = 0;

            foreach (int val in data)
            {
                sum += val;

            }
            // Test on variale sum. 
            if (sum > 100)
                result = 1;
            else
                result = 0;

        }
    }
}
