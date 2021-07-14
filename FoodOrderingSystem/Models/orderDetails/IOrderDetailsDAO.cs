using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.orderDetails
{
    public interface IOrderDetailsDAO
    {
        bool AddOrderDetail(string orderID, string itemID, int quantity);
        public IList<dynamic> GetOrderDetails(string orderID);
    }
}
