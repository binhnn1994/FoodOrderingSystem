using FoodOrderingSystem.Models.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.Cart
{
    public class CartObj
    {
        public int quantity { set; get; }
        public Item item { set; get; }
    }
}
