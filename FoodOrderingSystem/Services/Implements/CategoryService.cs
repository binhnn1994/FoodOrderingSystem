using FoodOrderingSystem.Models.category;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private ICategoryDAO _categoryDAO;
        public CategoryService(ICategoryDAO categoryDAO) => _categoryDAO = categoryDAO;
        public IEnumerable<Category> GetCategories() => _categoryDAO.GetCategories();
    }
}
