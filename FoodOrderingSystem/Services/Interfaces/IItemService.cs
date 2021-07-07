﻿using FoodOrderingSystem.Models.item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IItemService
    {
        int NumberOfItemFilterCategory(string categoryName, string status);
        IEnumerable<Item> ViewItemListFilterCategory(string categoryName, string status, int RowsOnPage, int RequestPage);
        bool CreateItem(string itemName, string categoryName, decimal unitPrice, int availableQuantity, int foodCoin);
        bool UpdateItemInformation(string itemID, string itemName, string categoryName, decimal unitPrice, int availableQuantity, int foodCoin);
        bool InactiveItem(string itemID, string note);
        Item GetDetailOfItem(string itemID);
        bool ActiveItem(string itemID);
        int NumberOfItemBySearching(string searchValue, string categoryName, string status);
        IEnumerable<Item> ViewItemListBySearching(string searchValue, string categoryName, string status, int RowsOnPage, int RequestPage);
    }
}
