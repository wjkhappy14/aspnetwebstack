using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections.BasicAlgorithms
{

    /// <summary>
    /// 一些通用的算法  http://www.geeksforgeeks.org/
    /// </summary>
    public class Common
    {

        public Common()
        {


        }



        /// <summary>
        /// 是否是回文
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        // Using LINQ Sort and Comare.
        // Sort word1, Sort word2 and compare both
        public static bool IsAnagramOf(string word1, string word2)
        {
            return word1.OrderBy(x => x).SequenceEqual(word2.OrderBy(x => x));
        }

        /// <summary>
        /// 交换两个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap<T>(ref T x, ref T y)
        {
            T temp;
            temp = x;
            x = y;
            y = temp;
        }

    }
}
