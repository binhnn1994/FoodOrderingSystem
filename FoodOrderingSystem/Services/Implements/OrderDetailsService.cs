using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private IOrderDetailsService _orderDetailsService;
        public OrderDetailsService(IOrderDetailsService orderDetailsService) => _orderDetailsService = orderDetailsService;

        public bool AddOrderDetail(string orderID, string itemID, int quantity)
            => _orderDetailsService.AddOrderDetail(orderID, itemID, quantity);
    }
}
