using FoodOrderingSystem.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IAccountService
    {
        int NumberOfStaffs();
        IEnumerable<Account> ViewStaffsList(int RowsOnPage, int RequestPage);
    }
}
