using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public class CustomerOrder
    {
        [Required]
        public string OrderID { get; set; }
        [Required]
        public string CustomerID { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string ToAddress { get; set; }
        public double DeliveryFee { get; set; }
        public string Note { get; set; }
        public decimal Total { get; set; }
    }
}
