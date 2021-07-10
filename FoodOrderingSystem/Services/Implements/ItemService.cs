using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class ItemService : IItemService
    {
        private IItemDAO _itemDAO;
        public ItemService(IItemDAO itemDAO) => _itemDAO = itemDAO;

        public bool CreateItem(string itemName, string categoryName, decimal unitPrice)
            => _itemDAO.CreateItem(itemName, categoryName, unitPrice);

        public bool UpdateItemInformation(string itemID, string itemName, string categoryName, decimal unitPrice)
            => _itemDAO.UpdateItemInformation(itemID, itemName, categoryName, unitPrice);

        public int NumberOfItemFilterCategory(string categoryName, string status)
            => _itemDAO.NumberOfItemFilterCategory(categoryName, status);

        public IEnumerable<Item> ViewItemListFilterCategory(string categoryName, string status, int RowsOnPage, int RequestPage)
            => _itemDAO.ViewItemListFilterCategory(categoryName, status, RowsOnPage, RequestPage);

        public Item GetDetailOfItem(string itemID) => _itemDAO.GetDetailOfItem(itemID);

        public bool ActiveItem(string itemID) => _itemDAO.ActiveItem(itemID);

        public bool InactiveItem(string itemID, string note) => _itemDAO.InactiveItem(itemID, note);

        public int NumberOfItemBySearching(string searchValue, string categoryName, string status)
            => _itemDAO.NumberOfItemBySearching(searchValue, categoryName, status);

        public IEnumerable<Item> ViewItemListBySearching(string searchValue, string categoryName, string status, int RowsOnPage, int RequestPage)
            => _itemDAO.ViewItemListBySearching(searchValue, categoryName, status, RowsOnPage, RequestPage);


    }
}
