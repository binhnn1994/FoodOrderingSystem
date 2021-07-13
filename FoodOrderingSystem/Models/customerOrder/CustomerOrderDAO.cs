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
        public CustomerOrder GetCustomerOrderByID(string orderID)
        {
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select orderID, customerID, orderDate, status, toAddress, deliveryFee, note " +
                            "From customerOrder " +
                            "Where orderID = @orderID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderID", orderID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CustomerOrder
                                {
                                    OrderID = orderID,
                                    CustomerID = reader.GetString("customerID"),
                                    OrderDate = reader.GetDateTime("orderDate"),
                                    Status = reader.GetString("status"),
                                    ToAddress = reader.GetString("toAddress"),
                                    DeliveryFee = reader.GetDouble("deliveryFee"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public bool ConfirmUpdate(string orderID, string status)
        {
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Update customerOrder " +
                                    "Set status = @status " +
                                    "Where orderID = @orderID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderID", orderID);
                        command.Parameters.AddWithValue("@status", status);
                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IList<CustomerOrder> GetPendingCustomerOrders()
        {
            IList<CustomerOrder> customerOrders = new List<CustomerOrder>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select orderID, customerID, orderDate, status, toAddress, deliveryFee, note "
                        + "From customerOrder "
                        + "Where status = 'Pending' ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerOrders.Add( new CustomerOrder
                                {
                                    OrderID = reader.GetString("orderID"),
                                    CustomerID = reader.GetString("customerID"),
                                    OrderDate = reader.GetDateTime("orderDate"),
                                    Status = reader.GetString("status"),
                                    ToAddress = reader.GetString("toAddress"),
                                    DeliveryFee = reader.GetDouble("deliveryFee"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note")
                                });
                            }
                            return customerOrders;
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
