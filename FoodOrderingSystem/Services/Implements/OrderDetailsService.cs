using FoodOrderingSystem.Models.orderDetails;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private IOrderDetailsDAO _orderDetailsDAO;
        public OrderDetailsService(IOrderDetailsDAO orderDetailsDAO) => _orderDetailsDAO = orderDetailsDAO;

        public bool AddOrderDetail(string orderID, string itemID, int quantity)
            => _orderDetailsDAO.AddOrderDetail(orderID, itemID, quantity);

        public IList<OrderDetails> GetOrderDetails(string orderID) => _orderDetailsDAO.GetOrderDetails(orderID);
    }
}
