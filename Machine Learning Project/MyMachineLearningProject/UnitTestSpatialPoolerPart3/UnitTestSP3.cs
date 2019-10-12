using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeoCortexApi;
using NeoCortexApi.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace UnitTestSpatialPoolerPart3
{
    [TestClass]
    public class UnitTestSP3

    {   /// <summary> 
            /// From Numenta
        /// After training, a SP can become less sensitive to 
       ///Noise.For this purpose, we train the SP by
        ///turning learning on, and by exposing it to a variety of random binary vectors.We will expose the SP
         ///to a repetition of input patterns in order to make it
        ///learn and distinguish them once learning is over.
         ///This will result in robustness to noise in the
        ///inputs. In this section we will reproduce
        ///the plot in the last section after the SP has
        ///learned a series of inputs.
        ///We will present 10 random vectors to the SP,
        ///and repeat these 30 times.
        ///Later you can try changing the number of times
        ///we do this to see how it changes the last plot. 
       /// you could also modify the number of examples to see how the SP behaves. Is there a
       /// relationship between the number of examples

        /// </summary>
        [TestMethod]

        
        public void TestMethod1()
        {   
            //number of binary vectors that will be used to train the SP
            int numExamples = 10;

            //2 dimensional input vector. Each row will be used as one-dimensional vector
            int[,] inputVectors = new int[numExamples, 1000];
            
            //2 dimensional output binary vector 
            int[,] outputColumns = new int[numExamples, 2048];

            //Feeding here the input vector with random binary numbers
            for (int i = 0; i < numExamples; i++)
            {
                for (int k = 0; k < 1000; k++)
                {
                    var random = new Random();
                    inputVectors[i, k] = random.Next(0, 2);
                }
            }

            //Spatial Pooler initialization and configuration 
            var parameters = Helpers.GetDefaultParams();
            parameters.setInputDimensions(new int[] { 1000 });
            parameters.setColumnDimensions(new int[] { 2048 });

            var sp = new SpatialPooler();
            var mem = new Connections();
            parameters.apply(mem);
            sp.init(mem);

            //One dimensional input vector initialized with binary numbers
            int[] inputVector = Helpers.GetRandomVector(1000,
            parameters.Get<Random>(KEY.RANDOM));

             //Array that will be expose to SP to active some of its columns 
            int[] activeCols = new int[2048];
            
            //List for active columns scores
            List<int> activeColScores = new List<int>();

            //Learning is turned off
            sp.compute(mem, inputVector, activeCols, false);

            //overlaps score of the active columns
            var overlaps = sp.calculateOverlap(mem, inputVector);

            //Sorting reversely overlaps arrays then writing it to the text file overlapBeforeTraining.txt
            overlaps = reverseSort(overlaps);
            WriteIntArrayToFile("overlapBeforeTraining.txt", overlaps);

            //we put the overlap score of each active column in a list called activeColScores
            for (int i = 0; i < 2048; i++)
            {
                if (activeCols[i] != 0)
                {
                     activeColScores.Add(overlaps[i]);
                }
            }

          
            int numberOfActivCols = 0;
            //counting the number of active columns
            foreach (int i in activeColScores)
            { 
                numberOfActivCols = numberOfActivCols + 1;
            }
           
            // Array to take the number of active columns to write it to file for plotting purpose
            int[] numberOfActiveColumns = new int[] { 1 };
            numberOfActiveColumns[0] = numberOfActivCols;

            
            int[] inputVectorsRowk = new int[] { };

            int[] outputColumnsRowk = new int[] { };

            //number of times the vectors are exposed to the SP 
            int epochs = 1;
            

            //the "numExamples" input binary vectors are exposed to the SP "epochs" times to train it
            for (int i = 0; i < epochs; i++)
            {
                for (int k = 0; k < numExamples ; k++)

                {
                    inputVectorsRowk = GetRow(inputVectors, k);
                    outputColumnsRowk = GetRow(outputColumns, k);
                    sp.compute(mem, inputVectorsRowk, outputColumnsRowk, true);
                }
            }

     
            
            overlaps = sp.calculateOverlap(mem, inputVectorsRowk);

            overlaps = reverseSort(overlaps);

          WriteIntArrayToFile("overlapAfterTraining.txt", overlaps);

          WriteIntArrayToFile("numberOfActCols.txt", numberOfActiveColumns);
            
           //overlap before and after training , numberOfActCols needed for graphs 
           runPythonCode2("overlapBeforeTraining.txt", "overlapAfterTraining.txt", "numberOfActCols.txt");

            int[,] inputVectorsCorrupted = new int[numExamples, 1000];
            int[,] outputColumnsCorrupted = new int[numExamples, 2048];


            float[] Noise = new float[] { 0, 5, 10, 15, 20, 30, 40, 50, 60, 70, 80, 90, 100 };


            double[] percentOverlapInputs = new double[Noise.Length];
            double[] percentOverlapOutputs = new double[Noise.Length];

            int[] inputVectorsCorruptedRow0 = GetRow(inputVectorsCorrupted, 0);
            int[] inputVectorsRow0 = GetRow(inputVectors, 0);
            int[] outputColumnsRow0 = GetRow(outputColumns, 0);
            int[] outputColumnsCorruptedRow0 = GetRow(outputColumnsCorrupted, 0);

            resetVector(inputVectorsRow0, inputVectorsCorruptedRow0);

            for (int i = 0; i < Noise.Length; i++)
            {
                inputVectorsCorruptedRow0 = corruptVector(ref inputVectorsRow0, Noise[i]);

                sp.compute(mem, inputVectorsRow0, outputColumnsRow0, false);
                sp.compute(mem, inputVectorsCorruptedRow0, outputColumnsCorruptedRow0, false);

                percentOverlapInputs[i] = percentOverlap(inputVectorsRow0, inputVectorsCorruptedRow0);
                percentOverlapOutputs[i] = percentOverlap(outputColumnsRow0, outputColumnsCorruptedRow0);


            }
             
           WriteDoubleArrayToFile("percentOverlapInputs.txt", percentOverlapInputs);
           WriteDoubleArrayToFile("percentOverlapOutputs.txt", percentOverlapOutputs);

            runPythonCode1("percentOverlapInputs.txt", "percentOverlapOutputs.txt");

        }

        /// <summary>
        /// this method cpoies x1 vector to x2 vector
        /// </summary>
        /// <param name="x1">binary vector that will be copied</param>
        /// <param name="x2">binary vector to which x1 is copied </param>
        public static void resetVector(int[] x1, int[] x2)
        {
            for (int i = 0; i < x1.Length; i++)
            {
                x2[i] = x1[i];
            }
        }


        /// <summary>
        /// This method enables to run python script in this C# code
        /// It will send the 2 parameters as arguments to the script 
        /// which will plot them
        /// </summary>
        /// <param name="filename1">first text file name</param>
        /// <param name="filename2">second text file name </param>
        public static void runPythonCode1 (string filename1, string filename2)
            {

                var pythonPath = Environment.GetEnvironmentVariable("PYTHON");
                pythonPath = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe";
                if (string.IsNullOrEmpty(pythonPath))
                    throw new InvalidOperationException("Python not installed. Environment variable 'Path' not set.");

                ProcessStartInfo pInf = new ProcessStartInfo();

                string pyFileToExecute = "create-plots-Part3-fig4b.py";

                pInf.FileName = pythonPath;

                pInf.ArgumentList.Add(pyFileToExecute);

                pInf.ArgumentList.Add("overlap in and out ");
                pInf.ArgumentList.Add("data1");
                pInf.ArgumentList.Add(filename1);
                pInf.ArgumentList.Add("data2");
                pInf.ArgumentList.Add(filename2);

                Process proc = Process.Start(pInf);


            }



        
        /// <summary>
        /// method to launch python script to plot figure a of part 3
        /// </summary>
        /// <param name="filename1">contains overlap data before training</param>
        /// <param name="filename2">contains overlap data after training</param>
        /// <param name="filename3">contains number of active columns</param>
        public static void runPythonCode2(string filename1, string filename2, string filename3)
        {

            var pythonPath = Environment.GetEnvironmentVariable("PYTHON");
            pythonPath = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe";
            if (string.IsNullOrEmpty(pythonPath))
                throw new InvalidOperationException("Python not installed. Environment variable 'Path' not set.");

            ProcessStartInfo pInf = new ProcessStartInfo();

            string pyFileToExecute = "create-plots-Part3-fig4a.py";

            pInf.FileName = pythonPath;

            pInf.ArgumentList.Add(pyFileToExecute);

            pInf.ArgumentList.Add("overlap in and out");
            pInf.ArgumentList.Add("data1");
            pInf.ArgumentList.Add(filename1);
            pInf.ArgumentList.Add("data2");
            pInf.ArgumentList.Add(filename2);
            pInf.ArgumentList.Add("data3");
            pInf.ArgumentList.Add(filename3);



            Process proc = Process.Start(pInf);





        }


        /// <summary>
        /// This method is writing data which are arrays to text files
        /// </summary>
        /// <param name="fileName">name of the file to which data will be writen</param>
        /// <param name="data">data in array we wish to write to a text file</param>

        private static void WriteIntArrayToFile(string fileName, int[] data)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                StringBuilder sb = new StringBuilder();

                int j = 0;
                foreach (var item in data)
                {
                    sb.Append(item);

                     
                    if (++j < data.Length)
                        sb.Append("|");
                }

                sw.Write(sb.ToString());
            }


        }

        /// <summary>
        /// This method is writing data which are double arrays to text files
        /// </summary>
        /// <param name="fileName"> file name where data will be written</param>
        /// <param name="data">it is an array of type double whose data will be saved in text file</param>

        private static void WriteDoubleArrayToFile(string fileName, double[] data)
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    StringBuilder sb = new StringBuilder();

                    int j = 0;
                    foreach (var item in data)
                    {
                        sb.Append(item);

                  
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

            /// <summary>
            /// this method takes an array and sorts its elements in reverse order
            /// </summary>
            /// <param name="x"> vector or one-dimension array whose elements will be sorted reversely</param>
            /// <returns>an array whose elements have been sorted reversely</returns>
            public static int[] reverseSort(int[] x)
            {
                Array.Sort(x);
                Array.Reverse(x);
                return x;
            }

            /// <summary>
            /// this method takes a 2-dimensional array and returns rows 
            /// </summary>
            /// <typeparam name="T">special type</typeparam>
            /// <param name="matrix">this is a 2 dimensional array</param>
            /// <param name="row">index of rows of the matrix we wish to retrieve</param>
            /// <returns>returns a row of the matrix whose index is given</returns>

            public static T[] GetRow<T>(T[,] matrix, int row)
            {
                var columns = matrix.GetLength(1);
                var array = new T[columns];
                for (int i = 0; i < columns; ++i)
                    array[i] = matrix[row, i];
                return array;
            }



        }
}


