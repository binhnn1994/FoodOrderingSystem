using FoodOrderingSystem.Models.account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Interfaces
{
    public interface IAccountService
    {
        int NumberOfAccountByRole(string roleName);
        IEnumerable<Account> ViewAccountListByRole(string roleName, int RowsOnPage, int RequestPage);
        bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, DateTime dateOfBirth, string address);
        bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, DateTime dateOfBirth, string address);
        bool InactiveAccount(string userID, string note);
        Account GetDetailOfAccount(string userID);
        bool ActiveAccount(string userID);
    }
}
