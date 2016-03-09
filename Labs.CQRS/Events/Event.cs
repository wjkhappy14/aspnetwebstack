using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs.CQRS
{


    /// <summary>
    /// 
    /// </summary>
    public class Event : Message
    {
        public int Version { get; set; }
    }
}
