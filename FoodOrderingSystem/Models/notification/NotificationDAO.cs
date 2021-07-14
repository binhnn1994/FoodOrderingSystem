using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.notification
{
    public class NotificationDAO
    {
        public IList<Notification> GetNotifications(bool isRead)
        {
            IList<Notification> notifications = new List<Notification>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select notificationID, orderID, isRead, message " +
                                    "From notification " +
                                    "Where isRead = @isRead ";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@isRead", isRead);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    NotificationID = reader.GetInt32("notificationID"),
                                    OrderID = reader.GetString("orderID"),
                                    IsRead = reader.GetBoolean("isRead"),
                                    Message = reader.GetString("message")
                                });
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return notifications;
        }


    }
}
