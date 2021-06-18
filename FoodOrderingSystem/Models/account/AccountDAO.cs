using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.account
{
    public class AccountDAO : IAccountDAO
    {
        public IEnumerable<Account> ViewStaffsList(int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, phoneNumber, dateOfBirth, address, status " +
                            "From account " +
                            "Where roleName = 'Staff' " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    phoneNumber = reader.GetString(4),
                                    dateOfBirth = reader.GetDateTime(5),
                                    address = reader.GetString(6),
                                    status = reader.GetString(7),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return accounts;
        }

        public int NumberOfStaffs()
        {
            int count = 0;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account " +
                                "Where roleName = 'Staff'";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
                connection.Close();
            }
            return count;
        }

        public bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, DateTime dateOfBirth, string address)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into account (userID, userEmail, roleName, hashedPassword, fullname, phoneNumber, dateOfBirth, address, status) " +
                    "Values (Left(SHA(RAND()),12), @userEmail, 'Staff', SHA(@password), @fullName, @phoneNumber, @dateOfBirth, @address, 'Active')";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@fullName", fullname);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@address", address);
                    int rowEffects = command.ExecuteNonQuery();
                    if (rowEffects > 0)
                    {
                        Debug.WriteLine(rowEffects);
                        result = true;
                    }
                }
                connection.Close();
            }
            return result;
        }
    }
}
