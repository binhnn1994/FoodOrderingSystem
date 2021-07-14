using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public class CustomerOrderDAO : ICustomerOrderDAO
    {
        public CustomerOrder GetCustomerOrderByID(string orderID)
        {
            string result = null;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select orderID, customerID, orderDate, status, toAddress, deliveryFee, note, total " +
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
                                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    Total = reader.GetDouble("total")
                                };
                            }
                connection.Close();
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
                    string Sql = "Select orderID, customerID, orderDate, status, toAddress, deliveryFee, note, total "
                        + "From customerOrder "
                        + "Where status = 'Pending' ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerOrders.Add(new CustomerOrder
                                {
                                    OrderID = reader.GetString("orderID"),
                                    CustomerID = reader.GetString("customerID"),
                                    OrderDate = reader.GetDateTime("orderDate"),
                                    Status = reader.GetString("status"),
                                    ToAddress = reader.GetString("toAddress"),
                                    DeliveryFee = reader.GetDouble("deliveryFee"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    Total = reader.GetDouble("total")
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

        public string AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, double total)
        {
            string result = null;
            try
            {
                MySqlConnection connection = new MySqlConnection(DBUtils.ConnectionString);
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                AddMediaExecute();
                void AddMediaExecute()
                {
                    command.CommandText = "CreateOrder";
                    command.Parameters.AddWithValue("@customerID_Input", customerID);
                    command.Parameters.AddWithValue("@toAddress_Input", toAddress);
                    command.Parameters.AddWithValue("@deliveryFee_Input", deliveryFee);
                    command.Parameters.AddWithValue("@note_Input", note);
                    command.Parameters.AddWithValue("@total", total);
                    command.Parameters.Add("@orderID_Output", MySqlDbType.String).Direction
                        = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    result = (string)command.Parameters["@orderID_Output"].Value;
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail. " + ex.Message);
            }
            return result;
        }

        public IList<CustomerOrder> GetOrderListByID(string customerID)
        {
            IList<CustomerOrder> customerOrders = new List<CustomerOrder>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select orderID, customerID, orderDate, status, toAddress, deliveryFee, note, total "
                        + "From customerOrder "
                        + "Where customerID = @customerID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@customerID", customerID);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerOrders.Add(new CustomerOrder
                                {
                                    OrderID = reader.GetString("orderID"),
                                    CustomerID = reader.GetString("customerID"),
                                    OrderDate = reader.GetDateTime("orderDate"),
                                    Status = reader.GetString("status"),
                                    ToAddress = reader.GetString("toAddress"),
                                    DeliveryFee = reader.GetDouble("deliveryFee"),
                                    Note = reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString("note"),
                                    Total = reader.GetDouble("total")
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
    
