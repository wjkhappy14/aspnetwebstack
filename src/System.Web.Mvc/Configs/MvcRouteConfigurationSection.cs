using MvcXmlRouting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.Configs
{
    public class MvcRouteConfigurationSection:ConfigurationSection
    {

        [ConfigurationProperty("ignore", IsRequired = false)]
        public IgnoreCollection Ignore
        {
            get
            {
                return (IgnoreCollection)(this["ignore"]);
            }
        }

        [ConfigurationProperty("map", IsRequired = false)]
        public RoutingCollection Map
        {
            get
            {
                return (RoutingCollection)(this["map"]);
            }
        }
    }
}