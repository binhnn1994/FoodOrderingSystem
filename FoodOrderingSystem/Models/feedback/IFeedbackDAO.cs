using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public interface IFeedbackDAO
    {
        public Feedback GetFeedback(string feedbackID);

        public IList<Feedback> GetFeedbacks(string status);

        public bool RespondFeedback(string feedbackID);

        int AddFeedback(string customerEmail, string requestContent);
    }
}
