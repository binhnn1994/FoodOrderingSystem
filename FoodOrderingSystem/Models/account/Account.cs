using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.account
{
    public class Account
    {
        public string userID { get; set; }
        [EmailAddress]
        public string userEmail { get; set; }
        public string roleName { get; set; }
        public string hashedPassword { get; set; }
        [Required]
        public string fullname { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public string note { get; set; }
    }
}
