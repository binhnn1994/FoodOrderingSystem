using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public class CustomerOrder
    {
        public string orderID { get; set; }
        public string customerID { get; set; }
        public DateTime orderDate { get; set; }
        public string status { get; set; }
        public string toAddress { get; set; }
        public decimal deliveryFee { get; set; }
        public string note { get; set; }
        public decimal total { get; set; }
    }
}
