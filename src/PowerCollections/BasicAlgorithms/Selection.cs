using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections.BasicAlgorithms
{
 

    /*

  ============================================================================================================================================================================================================================

  For finding the kth smallest number in a list or array; such a number is called the kth order statistic. 
  This includes the cases of finding the minimum, maximum, and median elements.

  Input 4   3   7   8   2   9   5   1   6   0 (Unsorted Array)

  Find 4th biggest

  Use Selection Algorithm (Note it is differnt from selection sort) http://en.wikipedia.org/wiki/Selection_algorithm
  http://www.geeksforgeeks.org/k-largestor-smallest-elements-in-an-array/
  http://www.geeksforgeeks.org/forums/topic/kth-largest-element/

  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Few Selection Algorithm Types:

  1. Partial Selection Sort.
  2. Quick Select.
  3. Online Select.
  4. Quikc Select.
  5. Using Heap.
  6. Using Prioirty Queue.

  Build a heap/priority queue.  O(n)
  Pop top element.  O(log n)
  Pop top element.  O(log n)
  Pop top element.  O(log n)

  Total = O(n) + 3 O(log n) = O(n) 

  Worst Case : O(n) (linear time) , O (1) for Sorted Data.

  Best Case of this Algo : Finding the minimum (or maximum) element by iterating through the list, keeping track of the running minimum – the minimum so far – (or maximum) and can be seen as related to the selection sort. 
  Hardest Case : Finding the median, and this necessarily takes n/2 storage. 

  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

  Testing :

  1. NULL array and pass size as 0.
  2. Array with duplicates.
  3. Array which is already sorted or sorted in reverse order and we are finding 1st biggest or Nth biggest.
  4. Find something in between. Like given an array like {-6, -6, 2, 2, 2, 10, 10, 10, -6, -6, 2, 2, 2, 2, 2}, try finding different Nth biggest.
  5. Try also with array having all distinct elements. Something like {14, 13, 12, 11, 10,  9, 8, 7, 6, 5, 4, 3, 2, 1, 15}.

  ============================================================================================================================================================================================================================
  */
    partial class SelectionAlgorithm
    {
        public void SelectionAlgorithmKthSmallestTest()
        {
            int[] ArrayElements = { 40, 30, 70, 80, 20, 90, 50, 10, 60, 100 };
            // Fails for 7,8
            int kthSmallest = 10;
            //int resultVal = QuickSelect(ArrayElements, 0, (ArrayElements.Length - 1), kthSmallest);
            int resultVal = QuickSelect(ArrayElements, kthSmallest);
            
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // 2. Partition-based selection - Quick Select.
        // Best and Average Case O(n) Worst Case O(n^2)
        //Invoke this function by passing arrayElements, 0, size-1 and kthBiggest.
        int QuickSelect(int[] arrayElements, int kthSmallest)
        {
            int startPos = 0;
            int endPos = arrayElements.Length - 1;

            while (endPos > startPos)
            {
                int pivotIndx = QuickSelectPartition(arrayElements, startPos, endPos);

                if (pivotIndx == kthSmallest)
                {
                    break;
                }

                if (pivotIndx > kthSmallest)
                {
                    endPos = pivotIndx - 1;
                }
                else
                {
                    startPos = pivotIndx + 1;
                }
            }

            return arrayElements[kthSmallest - 1];
        }

        int QuickSelectPartition(int[] listToSort, int leftIndxPtr, int rightIndxPtr)
        {
            int pivotIndx = (leftIndxPtr + rightIndxPtr) / 2;
            int pivotValue = listToSort[pivotIndx];

            // Temporarily moves the pivot element to the end of the subarray, so that it does not get in the way.
            Common.Swap(ref listToSort[pivotIndx], ref listToSort[rightIndxPtr]);

            // Compare remaining array elements against pivotValue = A[hi]
            for (int lpStIndx = leftIndxPtr; lpStIndx <= rightIndxPtr; lpStIndx++)
            {
                // Remember while moving small elements from right to left, few larger elements will automatically move to right side.
                if (listToSort[lpStIndx] < pivotValue)
                {
                    Common.Swap(ref listToSort[lpStIndx], ref listToSort[leftIndxPtr]);
                    leftIndxPtr++;
                }
            }

            // Now leftIndxPtr stops at the position of the pivots original location, so move the pivot back to it's earlier place.
            Common.Swap(ref listToSort[leftIndxPtr], ref listToSort[rightIndxPtr]);
            return leftIndxPtr;
        }
        //private int QuickSelectPartition(int[] arrayElements, int leftPos, int rightPos, int pivotIndx)
        //{
        //    int pivotElement = arrayElements[pivotIndx];

        //    int beginIndx = leftPos;

        //    Common.Swap(ref arrayElements[pivotIndx], ref arrayElements[rightPos]);

        //    for (int lpCnt = leftPos; lpCnt < rightPos; lpCnt++)
        //    {
        //        if (arrayElements[lpCnt] < pivotElement)
        //        {
        //            Common.Swap(ref arrayElements[beginIndx], ref arrayElements[lpCnt]);
        //            beginIndx++;
        //        }
        //    }

        //    Common.Swap(ref arrayElements[beginIndx], ref arrayElements[rightPos]);

        //    return beginIndx;
        //}


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //1. O(KN) Partial Selection Sort : Yields a simple selection algorithm.
        //This is asymptotically inefficient, but can be sufficiently efficient if K is small, and is easy to implement. 
        public int SelectKthSmallest(int[] arrayElements, int kthSmallest)
        {
            // Note : Changes original list.
            for (int kthLpCnt = 0; kthLpCnt < kthSmallest; kthLpCnt++)
            {
                int minIndex = kthLpCnt;
                int minValue = arrayElements[kthLpCnt];

                for (int linearLpCnt = kthLpCnt + 1; linearLpCnt < arrayElements.Length; linearLpCnt++)
                {
                    if (arrayElements[linearLpCnt] < minValue)
                    {
                        minIndex = linearLpCnt;
                        minValue = arrayElements[linearLpCnt];

                        Common.Swap(ref arrayElements[kthLpCnt], ref arrayElements[minIndex]);

                    }
                }
            }
            return arrayElements[kthSmallest];
        }
    }
}
