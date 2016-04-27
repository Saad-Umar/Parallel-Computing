using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadedMatrixAddition
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number rows");
            int numberOfRows = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter the number columns");
            int numberOfColumns = Int32.Parse(Console.ReadLine());
            int[,] matrix = new int[numberOfRows, numberOfColumns];

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    Console.WriteLine("Enter a number");
                    matrix[i, j] = Int32.Parse(Console.ReadLine());
                }
            }

            Console.WriteLine("Enter number of threads");
            int numberOfThreads = Int32.Parse(Console.ReadLine());
            int multiplier = numberOfRows / numberOfThreads;
            bool extraLoadOnLastThread = ((numberOfRows % numberOfThreads) == 0) ? false : true;
            ThreadStart lastThreadHandle;

            for (int i = 0; i < numberOfRows; i++)
            {
                if (!extraLoadOnLastThread)
                {
                    int[] row = new int[numberOfColumns];
                    for (int j = 0; j < numberOfColumns; j++)
                    {
                        row[j] = matrix[i, j];
                    }
                    //Thread thread = new Thread(new ThreadStart(worker(row , row)));
                    //new Thread(() => worker(row, row));
                    //thread.Start();

                    bool newThreadNeeded = ((i % multiplier) == 0) ? true : false;

                    if (newThreadNeeded)
                    {
                        ThreadStart starter = delegate { worker(row, row, ref matrix, i); };
                        lastThreadHandle = starter;
                        new Thread(starter).Start();

                    }
                    else
                    {
                        lastThreadHandle = delegate { worker(row, row, ref matrix, i); };
                        new Thread(lastThreadHandle).Start();
                    }
                }
                else if (extraLoadOnLastThread && (i <= numberOfColumns - (numberOfRows % numberOfThreads)))
                {
                    int[] row = new int[numberOfColumns];
                    for (int j = 0; j < numberOfColumns; j++)
                    {
                        row[j] = matrix[i, j];
                    }
                    lastThreadHandle = delegate { worker(row, row,ref matrix, i); };
                    new Thread(lastThreadHandle).Start();
             
                }

            }
            Console.WriteLine("Added Matrix:");
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    Console.WriteLine(matrix[i,j]);
                }
            }
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadLine();
        }

        public static void worker(int[] row1, int[] row2, ref int[,] matrix, int rowNumber)
        {
   
            //int[] resultant = new int[row1.Length];
            for (int i = 0; i < row1.Length; i++)
            {
                try
                {
                    matrix[rowNumber, i] = row1[i] + row2[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

    }


}

