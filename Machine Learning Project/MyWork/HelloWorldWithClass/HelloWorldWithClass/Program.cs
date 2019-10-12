using CalculatorLibrary;
using System;

namespace HelloWorldWithClass
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World for new version of E4!");
            CalculatorModel cal = new CalculatorModel();

            int a = 10;
            int b = 2;

            int addres = cal.Add(a, b);
            int subres = cal.Sub(a, b);
             int multres = cal.Mult(a, b);
            int divres = cal.Div(a, b);

            Console.WriteLine("The result of adding " + a + " and " + b + " is = " + addres);
            Console.WriteLine("The result of subtracting " + a + " from " + b + " is = " + subres);
            Console.WriteLine("The result of multiplying " + a + " by " + b + " is = " + multres);
            Console.WriteLine("The result of dividing " + a + " by " + b + " is = " + divres);


            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
