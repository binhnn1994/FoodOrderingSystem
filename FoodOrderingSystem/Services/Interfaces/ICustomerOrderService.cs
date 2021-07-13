﻿using FoodOrderingSystem.Models.customerOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface ICustomerOrderService
    {
        public CustomerOrder GetCustomerOrderByID(string orderID);
        public bool ConfirmUpdate(string orderID, string status);
        public IList<CustomerOrder> GetPendingCustomerOrders();
    }
}
