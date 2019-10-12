using System;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.IO;

namespace ConfigurationReadingE7
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()

           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var setting1 = configuration["Setting1"];
            Console.WriteLine(setting1);
            Console.WriteLine(configuration["Setting2"]);
            Console.ReadLine();
        }
    }
}
