using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Wintellect.PowerCollections;

namespace ParallelMergeSort
{
    /// <summary>
    /// Parallel merge sort by dividing the array into N buckets, where N is the total number of threads, each thread will either sort its bucket using QickSort or MergeSort
    /// (QuickSort shows slightly better performance) all threads are synchronized using a System.Threading.barrier. When all threads sort their partitions, half of the threads (the 
    /// odd threads will be removed) and the even threads will merge its chunck with the removed thread's chunk, then the odd threads are moved again, after Log(N) iterations,
    /// The sorted array will be in one chunk in the first thread
    /// </summary>
  public  static class ParallelSort
    {




        public static int Counter { get; set; }

        /// <summary>
        /// Parallel Merge Sort using Barrier
        /// </summary>
        /// <typeparam name="T">Array type</typeparam>
        /// <param name="array">The array object</param>
        /// <param name="comparer">Comparer object, if null the default comparer will be used</param>
        /// http://blogs.msdn.com/b/pfxteam/archive/2011/06/07/10171827.aspx
        public static void Sort<T>(T[] array, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            if (array == null)
                throw new ArgumentNullException("array");
            if (array.Length == 0)
                throw new ArgumentException("array");

            // fallback to sequntial if the number of elements is too small
            if (array.Length <= Environment.ProcessorCount * 2)
            {
                Array.Sort(array, comparer);
            }

            // the auxilary array
            T[] auxArray = new T[array.Length];
            //Array.Copy(array, auxArray, array.Length);

            int totalWorkers = 2;//  Environment.ProcessorCount; // must be power of two

            //worker tasks, -1 because the main thread will be used as a worker too
            Task[] workers = new Task[totalWorkers - 1];

            // number of iterations is determined by Log(workers), this is why th workers has to be power of 2
            int iterations = (int)Math.Log(totalWorkers, 2);

            // Number of elements for each array, if the elements number is not divisible by the workers, the remainders will be added to the first
            // worker (the main thread)
            int partitionSize = array.Length / totalWorkers;
                   
            int remainder = array.Length % totalWorkers;
           


            //Barrier used to synchronize the threads after each phase
            //The PostPhaseAction will be responsible for swapping the array with the aux array 
            Barrier barrier = new Barrier(totalWorkers, (b) =>
            {
                partitionSize <<= 1;

                var temp = auxArray;
                auxArray = array;
                array = temp;
            });

            Action<object> workAction = (obj) =>
                {
                    int index = (int)obj;

                    //calculate the partition boundary
                    int low = index * partitionSize;
                    if (index > 0)
                        low += remainder;
                    int high = (index + 1) * partitionSize - 1 + remainder;

                    //QuickSort(array, low, high, comparer);
                    Array.Sort(array, low, high - low + 1, comparer);

                    barrier.SignalAndWait();

                    for (int j = 0; j < iterations; j++)
                    {
                        //we always remove the odd workers
                        if (index % 2 == 1)
                        {
                            barrier.RemoveParticipant();
                            break;
                        }

                        int newHigh = high + partitionSize / 2;
                        index >>= 1; //update the index after removing the zombie workers
                        Merge(array, auxArray, low, high, high + 1, newHigh, comparer);
                        high = newHigh;
                        barrier.SignalAndWait();
                    }

                };


            
            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = Task.Factory.StartNew(obj => workAction(obj), i+1);   
            }

            workAction(0);

            if(iterations % 2 != 0)
                Array.Copy(auxArray, array, array.Length);

        }


        /// <summary>
        /// Merge sort an array recursively
        /// </summary>
        public static void MergeSort<T>(T[] array, T[] auxArray, int low, int high, IComparer<T> comparer)
        {
            Counter++;

            if (low >= high) return;

            int mid = (high + low) / 2;
            MergeSort<T>(auxArray, array, low, mid, comparer);
            MergeSort<T>(auxArray, array, mid + 1, high, comparer);

            Merge<T>(array, auxArray, low, mid, mid + 1, high, comparer);

        }

        /// <summary>
        /// Mrge two sorted sub arrays
        /// </summary>
        private static void Merge<T>(T[] array, T[] auxArray, int low1, int low2, int high1, int high2, IComparer<T> comparer)
        {
            int ptr1 = low1;
            int ptr2 = high1;
            int ptr = low1;
            for (; ptr <= high2; ptr++)
            {
                if (ptr1 > low2)
                    array[ptr] = auxArray[ptr2++];
                else if (ptr2 > high2)
                    array[ptr] = auxArray[ptr1++];

                else
                {
                    if (comparer.Compare(auxArray[ptr1], auxArray[ptr2]) <= 0)
                    {
                        array[ptr] = auxArray[ptr1++];
                    }
                    else
                        array[ptr] = auxArray[ptr2++];
                }
            }
        }



        /// <summary>
        ///  QuickSort method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="comparer"></param>
        public static void QuickSort<T>(T[] array, int low, int high, IComparer<T> comparer)
        {

            Counter++;
            T pivot;
            int l_hold, h_hold;

            l_hold = low;
            h_hold = high;
            pivot = array[low];

            while (low < high)
            {
                while (comparer.Compare(pivot, array[high]) <= 0 && (low < high))
                {
                    high--;
                }

                if (low != high)
                {
                    array[low] = array[high];
                    low++;
                }

                while (comparer.Compare(pivot, array[low]) >= 0 && (low < high))
                {
                    low++;
                }

                if (low != high)
                {
                    array[high] = array[low];
                    high--;
                }
            }

            array[low] = pivot;
            int mid = low;
            low = l_hold;
            high = h_hold;

            if (low < mid)
            {
                QuickSort(array, low, mid - 1, comparer);
            }

            if (high > mid)
            {
                QuickSort(array, mid + 1, high, comparer);
            }
        }
    
    }
}
