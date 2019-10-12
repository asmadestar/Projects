using System;

namespace InterfacesAndClassesE8
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            double[] data = { 1, 4, 7, 2, 19, 3, 56, 8, 11, 9, 13 };
            ClassForIMyInterface2 objectMeanVariance = new ClassForIMyInterface2();
            ClassForIMyInterface1 objectSum = new ClassForIMyInterface1();

            Result results = new Result();
            objectMeanVariance.Train(data);
            results = (Result)objectMeanVariance.GetResult();
            Console.WriteLine(" Variance,Standard Deviation and Average of the values in this Array.\n");
            Console.WriteLine("Average of Given Data is " + Math.Round(results.Average), 2);
            Console.WriteLine("Standard Deviation of Given Data is " + Math.Round(results.StandardDeviation));
            Console.WriteLine("Variance of Given Data is " + Math.Round(results.Variance));


            Console.WriteLine(" Sum of all the values in data and test if it is greater than 100 or not");
            objectSum.Train(data);
            int result = (int)objectSum.GetResult();
            Console.WriteLine("Result \n" + result);
            Console.ReadLine();
        }
    }
}
