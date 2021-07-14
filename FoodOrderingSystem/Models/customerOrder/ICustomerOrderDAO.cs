using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public interface ICustomerOrderDAO
    {
        public CustomerOrder GetCustomerOrderByID(string orderID);
        public bool ConfirmUpdate(string orderID, string status);
        public IList<CustomerOrder> GetPendingCustomerOrders();
        string AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, double total);
        public IList<CustomerOrder> GetOrderListByID(string customerID);
    }
}
