using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class ServiceLocator
    {
        public static ServiceBus Bus { get; set; }

    }
}