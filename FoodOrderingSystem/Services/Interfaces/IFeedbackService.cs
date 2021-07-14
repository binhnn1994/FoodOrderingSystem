using FoodOrderingSystem.Models.feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IFeedbackService
    {
        public Feedback GetFeedback(string feedbackID);

        public IList<Feedback> GetFeedbackByStatus(string status);

        public bool RespondFeedback(string feedbackID);

        int AddFeedback(string customerEmail, string requestContent);
    }
}
