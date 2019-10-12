using System;

namespace CalculatorLibrary
{
    public class CalculatorModel
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Sub(int a, int b)
        {
            return a - b;
        }

        public int Mult(int a, int b)
        {
            return a*b;
        }

        public int Div(int a, int b)
        {
            return a/b;
        }
    }
}
