using FoodOrderingSystem.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IAccountService
    {
        int NumberOfAccounts();
        IEnumerable<Account> ViewAccountsList(int RowsOnPage, int RequestPage);
    }
}
