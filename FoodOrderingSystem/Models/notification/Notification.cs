using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.notification
{
    public class Notification
    {
        [Required]
        public int NotificationID { get; set; }
        public string OrderID { get; set; }
        public bool IsRead { get; set; }
        public string Message { get; set; }
    }
}
