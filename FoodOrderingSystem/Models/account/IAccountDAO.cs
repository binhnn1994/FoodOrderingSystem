using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models.account
{
    public interface IAccountDAO
    {
        int NumberOfAccountByRole(string roleName);
        IEnumerable<Account> ViewAccountListByRole(string roleName, int RowsOnPage, int RequestPage);
        bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, string address);
        bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, string address);
        bool InactiveAccount(string userID, string note); 
        Account GetDetailOfAccount(string userID);
        bool ActiveAccount(string userID);
        int NumberOfAccountBySearching(string searchValue, string roleName);
        IEnumerable<Account> ViewAccountListBySearching(string searchValue, string roleName, int RowsOnPage, int RequestPage);
        public Account Login(string userEmail, string password);
    }
}
