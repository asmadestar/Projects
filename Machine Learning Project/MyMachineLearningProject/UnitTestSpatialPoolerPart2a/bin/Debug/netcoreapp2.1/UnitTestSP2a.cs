using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NeoCortexApi;
using NeoCortexApi.Entities;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;

/// <summary>
/// Following description is from numenta:
/// The input overlap between two binary vectors is defined as their dot product.
/// In order to normalize this value we divide by the minimum number of active
/// inputs (in either vector). This means we are considering the sparser vector as
/// reference. Two identical binary vectors will have an input overlap of 1,
/// whereas two completely different vectors (one is the logical NOT of the other)
/// will yield an overlap of 0. In this section we will see how the input overlap
/// of two binary vectors decrease as we add noise to one of them.
/// </summary>

namespace UnitTestSpatialPoolerPart2a
{
    [TestClass]
    public class UnitTestSP2a

    {    /// <summary>
        /// This method is used to implement the C# version of part 2a python spatial pooler tutorial
        /// We will show with this implementation how the input overlap decreases as we add noise to one of the 
        /// input vectors
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            // 2 input binary vectors

            int[] inputX1 = new int[1000];

            int[] inputX2 = new int[1000];

            //feeding inputX1 with random binary numbers 

            for (int j = 0; j < 1000; j++)
            {
                var random = new Random();
                inputX1[j] = random.Next(0, 2);
            }


            // copying inputX2 to inputX1
            for (int j = 0; j < 1000; j++)
            {

                inputX2[j] = inputX1[j];
            }

            // Array containing noise levels in percentage
            float[] Noise = new float[] { 0, 5, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            //arrays that will be used to write noise levels and input overlap to text files
            double[] noiseLevelArray = new double[Noise.Length];
            double[] percentOverlapArray = new double[Noise.Length];


            for (int i = 0; i < Noise.Length; i++)
            {
                //inputX1 = inputX2 before the loop. inputX1 is reference vector to which noise is added 
                inputX2 = corruptVector(ref inputX1, Noise[i]);

                //noise level in array (not necessary because we could use the array Noise directly with WriteArrayToFile method)
                noiseLevelArray[i] = Noise[i];

                //array in which input overlap is stored
                percentOverlapArray[i] = percentOverlap(inputX1, inputX2);

            }


            // Writing data to text files
            WriteArrayToFile("noiseLevel.txt", noiseLevelArray);
            //WriteArrayToFile("percentInputOverlap.txt", percentOverlapArray);

            //calling this method to launch create-plots-Part2a.py script to plot line graph from data in these 2 files
            runPythonCodePart2a("noiseLevel.txt", "percentInputOverlap.txt");

        }





        /// <summary>
        /// This method enables to run python script in this C# code
        /// It will send the 2 parameters as arguments to the script 
        /// which will plot them
        /// </summary>
        /// <param name="filename1">first text file name</param>
        /// <param name="filename2">second text file name </param>
        public static void runPythonCodePart2a(string filename1, string filename2)
        {

            var pythonPath = Environment.GetEnvironmentVariable("PYTHON");
            pythonPath = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe";
            if (string.IsNullOrEmpty(pythonPath))
                throw new InvalidOperationException("Python not installed. Environment variable 'Path' not set.");

            ProcessStartInfo pInf = new ProcessStartInfo();

            string pyFileToExecute = "create-plots-Part2a.py";

            pInf.FileName = pythonPath;

            pInf.ArgumentList.Add(pyFileToExecute);

            /* See arguments of .py file to know what to add here as argument list.
             * 'python create-histogram.py <title> <Number of Points on X>, <name of data> <data file1>,.., <name of data> <data file N>"
             *  arg 1 : Title ; arg 2 : Number of points on X-axis ; arg 3 : name of data ; arg 4 : datafile1.txt,..,
             */

            pInf.ArgumentList.Add("input Overlap ");
            pInf.ArgumentList.Add("data1");
            pInf.ArgumentList.Add(filename1);
            pInf.ArgumentList.Add("data2");
            pInf.ArgumentList.Add(filename2);

            Process proc = Process.Start(pInf);


        }


        /// <summary>
        /// This method is writing data which are arrays to text files
        /// </summary>
        /// <param name="fileName">name of the file to which data will writen</param>
        /// <param name="data">data in array we wish to write to a text file</param>
        private static void WriteArrayToFile(string fileName, double[] data)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                StringBuilder sb = new StringBuilder();

                int j = 0;
                foreach (var item in data)
                {
                    sb.Append(item);

                    // no '|' at the end 
                    if (++j < data.Length)
                        sb.Append("|");
                }

                sw.Write(sb.ToString());
            }


        }

        /// <summary>
        /// This method adds noise to vectors
        /// adding noise to a binary vector means flipping a certain number of its bits
        /// 5 % noise to a binary vector of 100 bits means flipping randomly 5 bits
        /// </summary>
        /// <param name="vector">this is a one-dimension array fed with binary numbers</param>
        /// <param name="noiseLevel">it is the percentage of noise we would like to add to vector (array)</param>
        /// <returns>returns a corrupted vector</returns>

        public static int[] corruptVector(ref int[] vector, float noiseLevel)
        {
            Random rn = new Random();
            int size = vector.Length;
            //size += 1;
            int[] a = new int[size];
            var percent = ((double)size / 100) * noiseLevel;
            //percent += 1;
            Array.Copy(vector, 0, a, 0, size - 1);
            Random random = new Random();
            HashSet<int> randomNumbers = new HashSet<int>();

            for (int i = 0; i < percent; i++)
                while (!randomNumbers.Add(random.Next(0, Convert.ToInt32(percent)))) ;

            foreach (int i in randomNumbers)
            {


                if (a[i] == 1)
                {
                    a[i] = 0;
                }
                else
                {
                    a[i] = 1;
                }
            }

            return a;
        }


        /// <summary>
        /// this method calcultes the percent overlap of 2 binary vectors
        /// It is their dot product divided by the minimun number of 1 in the vectors
        /// </summary>
        /// <param name="x1"> binary vector 1 (array </param>
        /// <param name="x2">binary vector 2</param>
        /// <returns>the percent overlap between x1 and x2</returns>
        public static float percentOverlap(int[] x1, int[] x2)
        {
            
            int nonZeroX1 = count_nonzero(x1);
            int nonZeroX2 = count_nonzero(x2);

            float minX1X2 = Math.Min(nonZeroX1, nonZeroX2);
            float percentOverlap = 0;

            if (minX1X2 > 0)
            {
                percentOverlap = dotProductMethod(x1, x2) / minX1X2;

            }

            return percentOverlap;
        }

        /// <summary>
        /// the method below calculates the dot product
        /// The dot product of two vectors a = [a1, a2, …, an] and b = [b1, b2, …, bn] is defined as: 
        ///a* b = a1b1 + a2b2 +…+anbn
        /// </summary>
        /// <param name="x1"> binary vector 1</param>
        /// <param name="x2">binary vector 2</param>
        /// <returns>the dot product of x1 and x2</returns>

        public static float dotProductMethod(int[] x1, int[] x2)
        {


            float dotProductResult = 0;

            for (int i = 0, j = 0; i < x1.Length; i++, j++)

            {

                dotProductResult += x1[i] * x2[j];

            }

            return dotProductResult;
        }

        /// <summary>
        /// This method will take a binary vector and counts its number of active columns
        /// </summary>
        /// <param name="x">binary vector</param>
        /// <returns>the number of active columns of x (returns number of "1" contained in x)</returns>
        public static int count_nonzero(int[] x)
        {
            int numberOfNonZeros = 0;
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] == 1)
                {
                    numberOfNonZeros += 1;
                }

            }

            return numberOfNonZeros;
        }


       

    }
}

   
