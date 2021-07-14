using FoodOrderingSystem.Models.customerOrder;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly ICustomerOrderDAO _customerOrderDAO;
        public CustomerOrderService(ICustomerOrderDAO customerOrderDAO) => _customerOrderDAO = customerOrderDAO;

        public bool ConfirmUpdate(string orderID, string status) => _customerOrderDAO.ConfirmUpdate(orderID, status);

        public CustomerOrder GetCustomerOrderByID(string orderID) => _customerOrderDAO.GetCustomerOrderByID(orderID);
        public IList<CustomerOrder> GetPendingCustomerOrders() => _customerOrderDAO.GetPendingCustomerOrders();
        public string AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, double total)
            => _customerOrderDAO.AddCustomerOrder(customerID, toAddress, deliveryFee, note, total);
    }
}
