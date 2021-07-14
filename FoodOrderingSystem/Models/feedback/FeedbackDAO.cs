using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public class FeedbackDAO : IFeedbackDAO
    {
        public Feedback GetFeedback(string feedbackID)
        {
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select feedbackID, customerEmail, content " +
                            "From feedback " +
                            "Where feedbackID = @feedbackID ";
                    using (var command = new MySqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@feedbackID", feedbackID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Feedback
                                {
                                    FeedbackID = reader.GetString("feedbackID"),
                                    CustomerEmail = reader.GetString("customerEmail"),
                                    Content = reader.GetString("content")
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

        public IList<Feedback> GetFeedbacks(string status)
        {
            IList<Feedback> feedbacks = new List<Feedback>();
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string sql = "Select feedbackID, customerEmail, receiveDate, content, respondDate " +
                            "From feedback " +
                            "Where status = @status " +
                            "Order by receiveDate ";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                feedbacks.Add(new Feedback
                                {
                                    FeedbackID = reader.GetString("feedbackID"),
                                    CustomerEmail = reader.GetString("customerEmail"),
                                    ReceiveDate = reader.GetDateTime("receiveDate"),
                                    Content = reader.GetString("content"),
                                    RespondDate = reader.IsDBNull(reader.GetOrdinal("receiveDate")) ? null : reader.GetDateTime("receiveDate")
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
            return feedbacks;
        }

        public bool RespondFeedback(string feedbackID)
        {
            try
            {
                DateTime respondDate = DateTime.Now;
                string status = "Closed";
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string sql = "Update feedback " +
                                    "Set status = @status, respondDate = @respondDate " +
                                    "Where feedbackID = @feedbackID ";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@feedbackID", feedbackID);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@respondDate", respondDate);
                        int result = command.ExecuteNonQuery();
                        if (result == 1) return true;
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
