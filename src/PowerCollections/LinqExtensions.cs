using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintellect.PowerCollections
{
    public static class LinqExtensions
    {



        /// <summary>
        /// 引用自：http://blogs.msdn.com/b/pfxteam/archive/2012/11/16/plinq-and-int32-maxvalue.aspx
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }
        private static IEnumerable<T> YieldBatchElements<T>(
       IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }

    }
}
