﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.feedback
{
    public class Feedback
    {
        [Required]
        public string FeedbackID { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public DateTime ReceiveDate { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime? RespondDate { get; set; }

    }
}