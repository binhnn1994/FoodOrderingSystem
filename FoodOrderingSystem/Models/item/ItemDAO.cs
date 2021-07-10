using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.item
{
    public class ItemDAO : IItemDAO
    {
        public bool CreateItem(string itemName, string categoryName, decimal unitPrice)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "Insert Into item (itemID, itemName, status, categoryName, unitPrice) " +
                    "Values (Left(SHA(RAND()),12), @itemName, 'Active', @categoryName, @unitPrice)";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@categoryName", categoryName);
                    command.Parameters.AddWithValue("@unitPrice", unitPrice);
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

        public bool UpdateItemInformation(string itemID, string itemName, string categoryName, decimal unitPrice)
        {
            var result = false;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = "UPDATE item " +
                    "SET itemName = @itemName, categoryName = @categoryName, unitPrice = @unitPrice " +
                    "WHERE itemID = @itemID";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@itemID", itemID);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@categoryName", categoryName);
                    command.Parameters.AddWithValue("@unitPrice", unitPrice);
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

        public Item GetDetailOfItem(string itemID)
        {
            Item item = null;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                             "From item " +
                             "Where itemID = @itemID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemID", itemID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                item = new Item
                                {
                                    itemID = reader.GetString(0),
                                    itemName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    status = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    categoryName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    unitPrice = reader.GetDecimal(4),
                                    note = reader.IsDBNull(5) ? null : reader.GetString(5)
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
            return item;
        }

        public bool ActiveItem(string itemID)
        {
            bool result = false;
            try
            {
                if (GetDetailOfItem(itemID).status.Equals("Active")) return false;
                if (GetDetailOfItem(itemID).note == null || GetDetailOfItem(itemID).note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE item " +
                        "SET status= 'Active', note = null " +
                        "WHERE itemID = @itemID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@itemID", itemID);
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

        public bool InactiveItem(string itemID, string note)
        {
            bool result = false;
            try
            {
                if (GetDetailOfItem(itemID).status.Equals("Inactive")) return false;
                if (note == null || note.Trim().Equals("")) return false;
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "UPDATE item " +
                        "SET status= 'Inactive', note=@note " +
                        "WHERE itemID = @itemID";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@note", note);
                        command.Parameters.AddWithValue("@itemID", itemID);
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

        public int NumberOfItemFilterCategory(string categoryName, string status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = null;
                if (categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                                "From item ";
                else if (categoryName.Trim().ToLower().Equals("all") && !status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where status = @status ";
                else if (!categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where categoryName = @categoryName ";
                else
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where categoryName = @categoryName and status = @status ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    if (!categoryName.Trim().ToLower().Equals("all"))
                        command.Parameters.AddWithValue("@categoryName", categoryName);
                    if (!status.Trim().ToLower().Equals("all"))
                        command.Parameters.AddWithValue("@status", status);
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

        public IEnumerable<Item> ViewItemListFilterCategory(string categoryName, string status, int RowsOnPage, int RequestPage)
        {
            var items = new List<Item>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = null;
                    if (categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "LIMIT @offset, @limit";
                    else if (categoryName.Trim().ToLower().Equals("all") && !status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where status = @status " +
                            "LIMIT @offset, @limit";
                    else if (!categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where categoryName = @categoryName " +
                            "LIMIT @offset, @limit";
                    else
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where categoryName = @categoryName and status = @status " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        if (!categoryName.Trim().ToLower().Equals("all"))
                            command.Parameters.AddWithValue("@categoryName", categoryName);
                        if (!status.Trim().ToLower().Equals("all"))
                            command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new Item
                                {
                                    itemID = reader.GetString(0),
                                    itemName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    status = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    categoryName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    unitPrice = reader.GetDecimal(4),
                                    note = reader.IsDBNull(5) ? null : reader.GetString(5)
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
            return items;
        }

        public int NumberOfItemBySearching(string searchValue, string categoryName, string status)
        {
            int count = 0;
            using (var connection = new MySqlConnection(DBUtils.ConnectionString))
            {
                connection.Open();
                string Sql = null;
                if (categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                                "From item " +
                                "Where MATCH (itemName) AGAINST (@searchValue in natural language mode)";
                else if (categoryName.Trim().ToLower().Equals("all") && !status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and status = @status ";
                else if (!categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and categoryName = @categoryName ";
                else
                    Sql = "Select COUNT(itemID) " +
                          "From item " +
                          "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and categoryName = @categoryName and status = @status ";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    command.Parameters.AddWithValue("@searchValue", searchValue);
                    if (!categoryName.Trim().ToLower().Equals("all"))
                        command.Parameters.AddWithValue("@categoryName", categoryName);
                    if (!status.Trim().ToLower().Equals("all"))
                        command.Parameters.AddWithValue("@status", status);
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

        public IEnumerable<Item> ViewItemListBySearching(string searchValue, string categoryName, string status, int RowsOnPage, int RequestPage)
        {
            var items = new List<Item>();
            int offset = ((int)(RequestPage - 1)) * RowsOnPage;
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = null;
                    if (categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) " + 
                            "LIMIT @offset, @limit";
                    else if (categoryName.Trim().ToLower().Equals("all") && !status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and status = @status " +
                            "LIMIT @offset, @limit";
                    else if (!categoryName.Trim().ToLower().Equals("all") && status.Trim().ToLower().Equals("all"))
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and categoryName = @categoryName " +
                            "LIMIT @offset, @limit";
                    else
                        Sql = "Select itemID, itemName, status, categoryName, unitPrice, note " +
                            "From item " +
                            "Where MATCH (itemName) AGAINST (@searchValue in natural language mode) and categoryName = @categoryName and status = @status " +
                            "LIMIT @offset, @limit";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@limit", RowsOnPage);
                        command.Parameters.AddWithValue("@searchValue", searchValue);
                        if (!categoryName.Trim().ToLower().Equals("all"))
                            command.Parameters.AddWithValue("@categoryName", categoryName);
                        if (!status.Trim().ToLower().Equals("all"))
                            command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new Item
                                {
                                    itemID = reader.GetString(0),
                                    itemName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    status = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    categoryName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    unitPrice = reader.GetDecimal(4),
                                    note = reader.IsDBNull(5) ? null : reader.GetString(5)
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
            return items;
        }
    }
}
