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
    }
}
