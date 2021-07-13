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
        public Feedback GetFeedback(string feedbackID) => _feedbackDAO.GetFeedback(feedbackID);

        public IList<Feedback> GetFeedbacks(string status) => _feedbackDAO.GetFeedbacks(status);

        public bool RespondFeedback(string feedbackID) => _feedbackDAO.RespondFeedback(feedbackID);
    }
}
