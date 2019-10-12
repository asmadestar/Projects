using CalculatorLib;
using System;

namespace HelloWorldVS
{
    class Program
    {
        static void Main(string[] args)
        {
            BetterCalculator cal = new BetterCalculator();
           
            int res = cal.Add(221, 42343);
            res = cal.Mul(221, 42343);
            var res3 = cal.Div(221, 42343);

            Console.WriteLine(res);

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
