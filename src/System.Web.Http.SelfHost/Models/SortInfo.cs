using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Models
{
    public class SortInfo<T>
    {
        public SortInfo()
        {
        }
        public string Name { get; set; }
        public int Count { get; set; }
        public TimeSpan Elapsed { get; set; }
        public T[] Array { get; set; }

    }
}
