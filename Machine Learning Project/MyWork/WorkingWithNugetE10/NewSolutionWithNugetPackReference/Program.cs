using System;
using ClassLibraryNuget;

namespace NewSolutionWithNugetPackReference
{
    class Program
    {
        static void Main(string[] args)
        {    // creating an object of Class1
            Class1 nugetPack = new Class1();

            //calling the method class classLibraryMethod() and printing the return value on the screen
            Console.WriteLine(nugetPack.classLibraryMethod());

            Console.WriteLine("press any key to exit");
            Console.ReadLine();
        }
    }
}
