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
    interface ICommandSender
    {
        void Send<T>(T command) where T : Command;
    }
}
