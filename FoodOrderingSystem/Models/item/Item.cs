using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.item
{
    public class Item
    {
        public string itemID { get; set; }
        public string itemName { get; set; }
        public string status { get; set; }
        public string categoryName { get; set; }
        public decimal unitPrice { get; set; }
        public string note { get; set; }
        public string image { get; set; }
        public string description { get; set; }
    }
}
