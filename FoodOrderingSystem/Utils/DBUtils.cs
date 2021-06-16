using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FoodOrderingSystem.Utils
{
    public class DBUtils
    {
        public static string ConnectionString { get; set; }

        public static bool IsServerConnected()
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException)
                {
                    return false;
                }
            }
        }
    }
}
