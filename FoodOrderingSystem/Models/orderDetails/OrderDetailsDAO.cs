using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.orderDetails
{
    public class OrderDetailsDAO
    {
        public IList<OrderDetails> GetOrderDetails(string orderID)
        {
            IList<OrderDetails> orderDetails = new List<OrderDetails>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select orderID, itemID, quantity " +
                            "From orderDetails " +
                            "Where orderID = @orderID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderID", orderID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.Add(new OrderDetails
                                {
                                    OrderID = reader.GetString("orderID"),
                                    ItemID = reader.GetString("itemID"),
                                    Quantity = reader.GetInt32("quantity")
                                });
                            }
                            return orderDetails;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
