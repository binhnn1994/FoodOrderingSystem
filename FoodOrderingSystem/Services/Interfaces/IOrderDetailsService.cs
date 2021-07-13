using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        bool AddOrderDetail(string orderID, string itemID, int quantity);
    }
}
