using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public interface IFeedbackDAO
    {
        bool AddFeedback(int feedbackID, string customerEmail, DateTime receiveDate, string status, string content, DateTime respondDate);
    }
}
