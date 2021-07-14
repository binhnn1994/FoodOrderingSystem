using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public class Feedback
    {
        [Required]
        public int FeedbackID { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime ReceiveDate { get; set; }
        [Required]
        public string RequestContent { get; set; }
        public DateTime? RespondDate { get; set; }
        public string RespondContent { get; set; }

    }
}
