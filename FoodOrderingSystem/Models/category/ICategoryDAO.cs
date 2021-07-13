using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.category
{
    public interface ICategoryDAO
    {
        IEnumerable<Category> GetCategories();
    }
}
