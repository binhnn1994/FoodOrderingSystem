using FoodOrderingSystem.Models.customerOrder;
using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Models.orderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.saleReport
{
    public class SaleReportObj
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public float UnitPrice { get; set; }
        public float TotalSales { get; set; }
    }
}
