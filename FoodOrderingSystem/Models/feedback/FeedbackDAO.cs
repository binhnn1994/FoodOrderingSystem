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
        public bool AddFeedback(int feedbackID, string customerEmail, DateTime receiveDate, string status, string content, DateTime respondDate)
        {
            throw new NotImplementedException();
        }

        public Feedback GetFeedback(string feedbackID)
        {
            try
            {
                using (var connection = new MySqlConnection(DBUtils.ConnectionString))
                {
                    connection.Open();
                    string Sql = "Select feedbackID, customerEmail, status, receiveDate, requestContent, respondDate, respondContent" +
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
                                    FeedbackID = reader.GetInt32("feedbackID"),
                                    CustomerEmail = reader.GetString("customerEmail"),
                                    Status = reader.GetString("status"),
                                    ReceiveDate = reader.GetDateTime("receiveDate"),
                                    RequestContent = reader.GetString("requestContent"),
                                    RespondDate = reader.IsDBNull(reader.GetOrdinal("respondDate")) ? null : reader.GetDateTime("respondDate"),
                                    RespondContent = reader.GetString("respondContent")
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
                    string sql = "Select feedbackID, customerEmail, status, receiveDate, requestContent, respondDate, respondContent" +
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
                                    FeedbackID = reader.GetInt32("feedbackID"),
                                    CustomerEmail = reader.GetString("customerEmail"),
                                    Status = reader.GetString("status"),
                                    ReceiveDate = reader.GetDateTime("receiveDate"),
                                    RequestContent = reader.GetString("requestContent"),
                                    RespondDate = reader.IsDBNull(reader.GetOrdinal("respondDate")) ? null : reader.GetDateTime("respondDate"),
                                    RespondContent = reader.GetString("respondContent")
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
