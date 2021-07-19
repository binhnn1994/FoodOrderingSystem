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
        public IEnumerable<Account> ViewAccountListByRole(string roleName, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, phoneNumber, address, status, note " +
                            "From account " +
                            "Where roleName = @roleName " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        command.Parameters.AddWithValue("@roleName", roleName);
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
                                    address = reader.GetString(5),
                                    status = reader.GetString(6),
                                    note = reader.IsDBNull(7) ? null : reader.GetString(7)
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

        public int NumberOfAccountByRole(string roleName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account " +
                                "Where roleName = @roleName";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
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

        public bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, string address)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into account (userID, userEmail, roleName, hashedPassword, fullname, phoneNumber, address, status) " +
                    "Values (Left(SHA(RAND()),12), @userEmail, 'Staff', @password, @fullname, @phoneNumber, @address, 'Active')";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@fullname", fullname);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@address", address);
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

        public bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, string address)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "UPDATE account " +
                    "SET fullname = @fullname, phoneNumber = @phoneNumber, address = @address " +
                    "WHERE userID = @userID";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userID", userID);
                    command.Parameters.AddWithValue("@fullname", fullname);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@address", address);
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

        public bool InactiveAccount(string userID, string note)
        {
            bool result = false;
            try
            {
                if (GetDetailOfAccount(userID).status.Equals("Inactive")) return false;
                if (note == null || note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                        "SET status= 'Inactive', note=@note " +
                        "WHERE userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@note", note);
                        command.Parameters.AddWithValue("@userID", userID);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool ActiveAccount(string userID)
        {
            bool result = false;
            try
            {
                if (GetDetailOfAccount(userID).status.Equals("Active")) return false;
                if (GetDetailOfAccount(userID).note == null || GetDetailOfAccount(userID).note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE account " +
                        "SET status= 'Active', note = null " +
                        "WHERE userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        int rowEffects = command.ExecuteNonQuery();
                        if (rowEffects > 0)
                        {
                            result = true;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public Account GetDetailOfAccount(string userID)
        {
            Account account = null;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, phoneNumber, address, status, note " +
                                    "From account " +
                                    "Where userID = @userID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                account = new Account
                                {
                                    userID = reader.GetString(0),
                                    userEmail = reader.GetString(1),
                                    roleName = reader.GetString(2),
                                    fullname = reader.GetString(3),
                                    phoneNumber = reader.GetString(4),
                                    address = reader.GetString(5),
                                    status = reader.GetString(6),
                                    note = reader.IsDBNull(7) ? null : reader.GetString(7)
                                };
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public int NumberOfAccountBySearching(string searchValue, string roleName)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Select COUNT(userID) " +
                                "From account " +
                                "Where roleName = @roleName and MATCH (fullname) AGAINST (@searchValue in natural language mode)";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@roleName", roleName);
                    command.Parameters.AddWithValue("@searchValue", searchValue);
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

        public IEnumerable<Account> ViewAccountListBySearching(string searchValue, string roleName, int RowsOnPage, int RequestPage)
        {
            var accounts = new List<Account>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, phoneNumber, address, status, note " +
                            "From account " +
                            "Where roleName = @roleName and MATCH (fullname) AGAINST (@searchValue in natural language mode) " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        command.Parameters.AddWithValue("@roleName", roleName);
                        command.Parameters.AddWithValue("@searchValue", searchValue);
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
                                    address = reader.GetString(5),
                                    status = reader.GetString(6),
                                    note = reader.IsDBNull(7) ? null : reader.GetString(7)
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

        public Account Login(string userEmail, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select userID, userEmail, roleName, fullname, phoneNumber, address, status, note " +
                            "From account " +
                            "Where userEmail = @userEmail AND hashedPassword = @password AND status = 'Active'";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@userEmail", userEmail);
                        command.Parameters.AddWithValue("@password", password);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) return new Account
                            {
                                userID = reader.GetString("userID"),
                                userEmail = userEmail,
                                hashedPassword = password,
                                fullname = reader.GetString("fullname"),
                                roleName = reader.GetString("roleName")
                            };
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public bool Register(string userEmail, string password, string fullname, string phoneNumber, string address)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into account (userID, userEmail, roleName, hashedPassword, fullname, phoneNumber, address, status) " +
                    "Values (Left(SHA(RAND()),12), @userEmail, 'Customer', @password, @fullname, @phoneNumber, @address, 'Active')";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@fullname", fullname);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@address", address);
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
