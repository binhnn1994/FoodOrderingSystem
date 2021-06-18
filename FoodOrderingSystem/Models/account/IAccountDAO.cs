using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.account
{
    public interface IAccountDAO
    {
        int NumberOfStaffs();
        IEnumerable<Account> ViewStaffsList(int RowsOnPage, int RequestPage);
    }
}
