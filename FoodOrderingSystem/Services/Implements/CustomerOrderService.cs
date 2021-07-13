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
        private ICustomerOrderDAO _customerOrderDAO;
        public CustomerOrderService(ICustomerOrderDAO customerOrderDAO) => _customerOrderDAO = customerOrderDAO;
        public string AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, double total)
            => _customerOrderDAO.AddCustomerOrder(customerID, toAddress, deliveryFee, note, total);
    }
}
