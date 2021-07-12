using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.orderDetails
{
    public class OrderDetails
    {
        public string orderID { get; set; }
        public string itemID { get; set; }
        public int quantity { get; set; }
    }
}
