using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.customerOrder
{
    public interface ICustomerOrderDAO
    {
        bool AddCustomerOrder(string customerID, string toAddress, double deliveryFee, string note, decimal total);
    }
}
