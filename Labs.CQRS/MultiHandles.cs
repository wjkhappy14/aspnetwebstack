using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Labs.CQRS
{
    public class MultiHandles //: Handles<ItemCreatedEvent>
    {
        public void Create<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Handle<T>(T message)
        {
            using (MySqlConnection conn = new MySqlConnection())
            {
                MySql.Data.MySqlClient.MySqlHelper.ExecuteNonQuery(conn, "", new MySqlParameter { });
            }
        }

        public void Update<T>(T message)
        {
            throw new NotImplementedException();
        }
    }
}
