using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Http.SelfHost.Models
{


    /// <summary>
    /// 
    /// </summary>
    public class Topic : ObjectIdentity, IComparer<Topic>
    {
        public Topic()
        {
            Categories = new LinkedList<Category>();

        }
        public String Name { get; set; }


        public LinkedList<Category> Categories { get; private set; }

        public int Compare(Topic x, Topic y)
        {
            Comparison<Topic> c = (a, b) =>
            {
                return (int)(a.Id - b.Id);
            };
            return Wintellect.PowerCollections.Comparers.ComparerFromComparison(c).Compare(x, y);

        }
    }
}
