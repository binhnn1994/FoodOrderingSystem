using FoodOrderingSystem.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public class FeedbackDAO : IFeedbackDAO
    {
        public int AddFeedback(string customerEmail, string requestContent)
        {
            int result = 0;
            try
            {
                MySqlConnection connection = new MySqlConnection(DBUtils.ConnectionString);
                MySqlCommand command = new MySqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                AddFeedbackExecute();
                void AddFeedbackExecute()
                {
                    command.CommandText = "CreateFeedback";
                    command.Parameters.AddWithValue("@customerEmail_Input", customerEmail);
                    command.Parameters.AddWithValue("@requestContent_Input", requestContent);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Fail. " + ex.Message);
            }
            return result;
        }

        public Feedback GetFeedbackByID(string feedbackID)
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
