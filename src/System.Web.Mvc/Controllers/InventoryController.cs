using Labs.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.ModelCommands;
using System.Web.Mvc.Models;

namespace System.Web.Mvc.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ServiceBus _bus = ServiceLocator.Bus;
        public InventoryController()
        {

        }

        public ActionResult Index()
        {
            CreateInventoryItem itemCmd = new CreateInventoryItem();
            itemCmd.Id = ItemIdentity.Default_Id;
            _bus.Send(itemCmd);
            return Json(new { CreateCommand = itemCmd }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            CreateInventoryItem itemCmd = new CreateInventoryItem();
            _bus.Send(itemCmd);
            return Json(new { CreateCommand = itemCmd }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Bus()
        {
            return Json(new { Bus = _bus }, JsonRequestBehavior.AllowGet);
        }
    }
}