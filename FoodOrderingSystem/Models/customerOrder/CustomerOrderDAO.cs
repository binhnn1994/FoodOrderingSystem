using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public class CustomerOrderDAO : ICustomerOrderDAO
    {
        public bool AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, decimal total)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into customerOrder (orderID, customerID, orderDate, status, toAddress, deliveryFee, note, total) " +
                    "Values (Left(SHA(RAND()),10), @customerID, @orderDate, 'pending', @toAddress, @deliveryFee, @note, @total)";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@customerID", customerID);
                    command.Parameters.AddWithValue("@orderDate", DateTime.Now);
                    command.Parameters.AddWithValue("@toAddress", toAddress);
                    command.Parameters.AddWithValue("@deliveryFee", deliveryFee);
                    command.Parameters.AddWithValue("@note", note);
                    command.Parameters.AddWithValue("@total", total);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        result = true;
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
