using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.Data;

namespace System.Web.Http.SelfHost.WebMatrix
{
    public class DynamicDataReader
    {
        private static Database Db;


        public DynamicDataReader()
        {



        }
        static DynamicDataReader()
        {
            Db = Database.OpenConnectionString(ConfigurationManager.ConnectionStrings["UserCenter"].ConnectionString, "System.Data.SqlClient");
            Database.ConnectionOpened += (sender, args) =>
            {
                Diagnostics.Debug.WriteLine(args.Connection);
            };
        }

        public Database GetDB()
        {
            return Db;
        }

        public IEnumerable<dynamic> Query(string sqlText)
        {

            var result = Db.Query(sqlText);
            Db.Close();
             return result;

        }


    }
}
