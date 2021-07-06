using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.account
{
    public class Account
    {
        public string userID { get; set; }
        public string userEmail { get; set; }
        public string roleName { get; set; }
        public string hashedPassword { get; set; }
        public string fullname { get; set; }
        public string phoneNumber { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public string note { get; set; }
    }
}
