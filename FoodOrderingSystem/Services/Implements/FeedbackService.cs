using FoodOrderingSystem.Models.feedback;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private IFeedbackDAO _feedbackDAO;
        public FeedbackService(IFeedbackDAO feedbackDAO) => _feedbackDAO = feedbackDAO;
        public Feedback GetFeedback(string feedbackID) => _feedbackDAO.GetFeedbackByID(feedbackID);

        public IList<Feedback> GetFeedbackByStatus(string status) => _feedbackDAO.GetFeedbackByStatus(status);

        public bool RespondFeedback(string feedbackID) => _feedbackDAO.RespondFeedback(feedbackID);

        public int AddFeedback(string customerEmail, string requestContent) => _feedbackDAO.AddFeedback(customerEmail, requestContent);
    }
}
