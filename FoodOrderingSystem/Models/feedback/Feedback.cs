using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public class Feedback
    {
        public int feedbackID { get; set; }
        public string customerEmail { get; set; }
        public DateTime receiveDate { get; set; }
        public string status { get; set; }
        public string content { get; set; }
        public DateTime respondDate { get; set; }
        
        
    }
}
