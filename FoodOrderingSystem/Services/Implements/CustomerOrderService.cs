using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private ICustomerOrderService _customerOrderService;
        public CustomerOrderService(ICustomerOrderService customerOrderService) => _customerOrderService = customerOrderService;
        public string AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, double total)
            => _customerOrderService.AddCustomerOrder(customerID, toAddress, deliveryFee, note, total);
    }
}
