using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public interface IFeedbackDAO
    {
        public Feedback GetFeedback(string feedbackID);

        public IList<Feedback> GetFeedbackByStatus(string status);

        public bool RespondFeedback(string feedbackID);
    }
}
