using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace System.Web.Mvc.Controllers
{
    /// <summary>
    /// Created By：Angkor
    /// 测试语言文化区域设置信息
    /// </summary>
    public class ThreadCultureController : Controller
    {


        public ViewResult Index()
        {
            return View();
        }






        /// <summary>
        /// 获取当前线程文化信息
        /// /jp/ThreadCulture/CultureInfo/100
        /// </summary>
        /// <returns></returns>
        public JsonResult CultureInfo()
        {
           var CurrentCulture = Thread.CurrentThread.CurrentCulture;
           var CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            return Json(new
            {
                CultureDateTimeFormat = CurrentCulture.DateTimeFormat,
                ThreeLetterWindowsLanguageName=CurrentCulture.ThreeLetterWindowsLanguageName
            }, JsonRequestBehavior.AllowGet);
        }

    }
}