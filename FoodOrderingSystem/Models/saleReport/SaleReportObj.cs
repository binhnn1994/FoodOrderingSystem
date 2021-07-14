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
        public Item item { set; get; }
        public CustomerOrder customerOrder { set; get; }
        public OrderDetails orderDetails { set; get; }
    }
}
