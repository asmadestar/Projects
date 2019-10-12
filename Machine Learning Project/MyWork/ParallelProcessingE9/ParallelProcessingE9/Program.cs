using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ParallelProcessingE9
{
    class Program
    {
        static int numThreads = 150;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Console.WriteLine("Press any Key to start.");
            //Console.ReadKey();

            //numThreads = int.Parse(Console.ReadLine());

            Stopwatch sw = new Stopwatch();
            sw.Start();

            ParallelSamplesE9 nt = new ParallelSamplesE9();

             //Executes all tasks sequentianally
             // nt.StartSequenced(numThreads, workerFunction);

             // Executes with spaning of every worker on a single thread.
             // nt.StartMultithreadedNative(numThreads, workerFunction);

             //nt.StartMultithreadedNativeV2(numThreads, workerFunction);

            nt.StartWithTpl(numThreads, workerFunction);
            sw.Stop();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0} ms", sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void workerFunction(object onFinishDelegate)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"Started thread: {Thread.CurrentThread.Name} - {Thread.CurrentThread.ManagedThreadId}");

            double r = 202020203030442;
            for (int i = 0; i < 1500000; i++)
            {
                r += 1.94536;
            }

            if (onFinishDelegate != null)
            {
                ((Action<string>)onFinishDelegate)(Thread.CurrentThread.Name);
            }

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("Stopped thread: {0}", Thread.CurrentThread.Name);
        }
    }
}
