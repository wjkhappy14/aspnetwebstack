using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.Data;

namespace System.Web.Http.SelfHost.Controllers
{
    public class DatabaseController : ApiController
    {

        private Database Db = null;

        public DatabaseController()
        {

            Db = Database.OpenConnectionString(ConfigurationManager.ConnectionStrings["UserCenter"].ConnectionString, "System.Data.SqlClient");
            Database.ConnectionOpened += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine(args.Connection);
            };

            

        }



        [HttpGet]

        public IHttpActionResult Query(string cmd)
        {
            
        

            cmd = "Select   GetDate() as TimeNow";
            var data = Db.Query(cmd);
            return Json(data);

        }
        [HttpGet]
        public IHttpActionResult LastInsertId()
        {

          var lastId=   Db.GetLastInsertId();
            return Json(new {LastId=lastId });
        }


    }
}
