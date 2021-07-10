using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.item
{
    public interface IItemDAO
    {
        int NumberOfItemFilterCategory(string categoryName, string status);
        IEnumerable<Item> ViewItemListFilterCategory(string categoryName, string status, int RowsOnPage, int RequestPage);
        bool CreateItem(string itemName, string categoryName, decimal unitPrice);
        bool UpdateItemInformation(string itemID, string itemName, string categoryName, decimal unitPrice);
        bool InactiveItem(string itemID, string note);
        Item GetDetailOfItem(string itemID);
        bool ActiveItem(string itemID);
        int NumberOfItemBySearching(string searchValue, string categoryName, string status);
        IEnumerable<Item> ViewItemListBySearching(string searchValue, string categoryName, string status, int RowsOnPage, int RequestPage);
    }
}
