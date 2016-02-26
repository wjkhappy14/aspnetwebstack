using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCLabs.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return Json(new {
                Now =DateTime.Now.ToString(),
                EnvironmentVariables =Environment.GetEnvironmentVariables() },
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}