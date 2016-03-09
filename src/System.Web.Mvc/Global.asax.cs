using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.ModelCommands;
using System.Web.Mvc.ModelEvents;
using System.Web.Mvc.Models;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace System.Web.Mvc
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitService();
            //build EnableSkipStrongNames
        }


        private void InitService()
        {
            ServiceBus bus = new ServiceBus();
            EventStore eventStore = new EventStore(bus);
            var repository = new Repository<InventoryItem>(eventStore);
            var commands = new CommandHandler<InventoryItem>(repository);

            bus.RegisterHandler<CreateInventoryItem>(commands.Create);
            bus.RegisterHandler<DeleteInventoryItem>(commands.Delete);

            var detail = new InventoryItemCreatedHandle();
            bus.RegisterHandler<InventoryItemCreated>(detail.Create);
            bus.RegisterHandler<ItemsDeletedFromInventory>(detail.Delete);
            bus.RegisterHandler<InventoryItemRenamed>(detail.Update);


            ServiceLocator.Bus = bus;
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}