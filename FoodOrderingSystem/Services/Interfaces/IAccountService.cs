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
        bool CreateStaff(string userEmail, string password, string fullName, string phoneNumber, DateTime dateOfBirth, string address);
    }
}
