using Labs.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Labs.CQRS
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventHandles
    {

        public virtual void Create(ItemCreatedEvent message)
        {
            string insertText = "INSERT INTO inventory(id,version) VALUES(@id,@version)";
            using (var conn = new MySqlConnection())
            {
                var mysqlConn = System.Configuration.ConfigurationManager.ConnectionStrings["mysqlConn"].ConnectionString;
                conn.ConnectionString = mysqlConn;
                conn.Open();
                MySqlHelper.ExecuteNonQuery(
                    conn,
                    insertText,
                    new MySqlParameter[] { new MySqlParameter("@id", message.Id),
                    new MySqlParameter("@version", message.Version)
                    }
                    );
                conn.Close();
            }
        }
        public virtual void Delete(ItemDeletedEvent message)
        { }
        public virtual void Update(ItemUpdatedEvent message) { }

    }
}
