using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.orderDetails
{
    public class OrderDetailsDAO : IOrderDetailsDAO
    {
        public bool AddOrderDetail(string orderID, string itemID, int quantity)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into orderDetails (orderID, itemID, quantity) " +
                    "Values (@orderID, @itemID, @quantity)";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@orderID", orderID);
                    command.Parameters.AddWithValue("@itemID", itemID);
                    command.Parameters.AddWithValue("@quantity", quantity);
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

        public IList<dynamic> GetOrderDetails(string orderID)
        {
            IList<dynamic> orderDetails = new List<dynamic>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "SELECT D.orderID, I.itemName, D.quantity, I.unitPrice " +
                        "FROM orderDetails D Inner join item I on D.itemID = I.itemID " +
                        "WHERE D.orderID = @orderID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderID", orderID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orderDetails.Add(new
                                {
                                    OrderID = reader.GetString(0),
                                    Item = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    UnitPrice = reader.GetDouble(3)

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
