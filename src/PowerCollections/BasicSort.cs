using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections
{



    /// <summary>
    /// http://blogs.msdn.com/b/pfxteam/archive/2008/01/31/7357135.aspx
    /// Recursion and Concurrency  基于泛型的合并和快速排序算法
    /// </summary>
    public class BasicSort
    {



        public void Test()
        {




        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Quicksort<T>(T[] arr, int left, int right) where T : IComparable<T>
        {
            if (right > left)
            {
                int pivot = QuickSortPartition<T>(arr, left, right);
                Quicksort(arr, left, pivot - 1);
                Quicksort(arr, pivot + 1, right);
            }
        }

        private static int QuickSortPartition<T>(T[] listToSort, int leftIndxPtr, int rightIndxPtr)
        {
            int pivotIndex = (leftIndxPtr + rightIndxPtr) / 2;
            T pivotValue = listToSort[pivotIndex];

            // Temporarily moves the pivot element to the end of the subarray, so that it does not get in the way.
            Algorithms.Swap(ref listToSort[pivotIndex], ref listToSort[rightIndxPtr]);

            // Compare remaining array elements against pivotValue = A[hi]
            for (int lpStIndx = leftIndxPtr; lpStIndx <= rightIndxPtr; lpStIndx++)
            {
                // Remember while moving small elements from right to left, few larger elements will automatically move to right side.


                var compare = System.Collections.Generic.Comparer<T>.Default.Compare(pivotValue, listToSort[lpStIndx]);
                if (compare > 0)
                {
                    Algorithms.Swap(ref listToSort[lpStIndx], ref listToSort[leftIndxPtr]);
                    leftIndxPtr++;
                }
            }

            // Now leftIndxPtr stops at the position of the pivots original location, so move the pivot back to it's earlier place.
            Algorithms.Swap(ref listToSort[leftIndxPtr], ref listToSort[rightIndxPtr]);
            return leftIndxPtr;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static void Mergesort<T>(T[] arr, int left, int right) where T : IComparable<T>
        {
            if (right > left)
            {
                int mid = (right + left) / 2;
                Parallel.Invoke(
                    () => Mergesort(arr, left, mid),
                    () => Mergesort(arr, mid + 1, right));

                PartitionAndMerge<T>(arr, left, mid + 1, right);
            }
        }



        /// <summary>
        /// 合并
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToSort"></param>
        /// <param name="startIndxPtr"></param>
        /// <param name="centerIndxPtr"></param>
        /// <param name="endIndxPtr"></param>
        private static void PartitionAndMerge<T>(T[] listToSort, int startIndxPtr, int centerIndxPtr, int endIndxPtr)
        {
            int leftIndxPtr = startIndxPtr;
            int tempArrayIndex = startIndxPtr;
            int midIndxPtr = centerIndxPtr + 1;

            int tempArrLen = listToSort.Length;//endIndxPtr - startIndxPtr;
            T[] tempArray = new T[tempArrLen + 1];

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            // Pick elements from Left Sub Array or Right Sub Array which ever elements are smaller till one of the Sub Array finished first.
            while (leftIndxPtr <= centerIndxPtr && midIndxPtr <= endIndxPtr)
            {
                // Copy to temp array from Left to Mid - Smaller than Mid.

                var compare = System.Collections.Generic.Comparer<T>.Default.Compare(listToSort[leftIndxPtr], listToSort[midIndxPtr]);
                if (compare <= 0)
                {
                    tempArray[tempArrayIndex] = listToSort[leftIndxPtr];
                    leftIndxPtr++;
                }

                // Copy to temp array from Mid to Right - Larger than Mid.
                else
                {
                    tempArray[tempArrayIndex] = listToSort[midIndxPtr];
                    midIndxPtr++;
                }

                tempArrayIndex++;
            }

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            // Pick remaining all elements from Left Sub Array, if any elements are left in above loop. 
            // Incase of Right Sub Array is larger or has more smaller elements.
            while (leftIndxPtr <= centerIndxPtr)
            {
                // Copy to temp array from Left to Mid - Smaller than Mid.
                tempArray[tempArrayIndex] = listToSort[leftIndxPtr];

                leftIndxPtr++;
                tempArrayIndex++;
            }

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            // Pick remaining all elements from Right Sub Array, if any elements are left in above loop. 
            // Incase of Right Sub Array is larger or has more smaller elements.
            while (midIndxPtr <= endIndxPtr)
            {
                // Copy to temp array from Right to Mid - Larger than Mid.
                tempArray[tempArrayIndex] = listToSort[midIndxPtr];

                midIndxPtr++;
                tempArrayIndex++;
            }

            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            // Update back All elements from temp Array to soruce array.
            for (int lpIndxPtr = startIndxPtr; lpIndxPtr <= endIndxPtr; lpIndxPtr++)
            {
                listToSort[lpIndxPtr] = tempArray[lpIndxPtr];
            }
        }

    }
}
