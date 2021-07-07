using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Services.Implements
{
    public class AccountService : IAccountService
    {
        private IAccountDAO _accountDao;
        public AccountService(IAccountDAO accountDao) => _accountDao = accountDao;
        public IEnumerable<Account> ViewAccountListByRole(string roleName, int RowsOnPage, int RequestPage) 
            => _accountDao.ViewAccountListByRole(roleName, RowsOnPage, RequestPage);
        public int NumberOfAccountByRole(string roleName) => _accountDao.NumberOfAccountByRole(roleName);
        public bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, DateTime dateOfBirth, string address)
            => _accountDao.CreateStaff(userEmail, password, fullname, phoneNumber, dateOfBirth, address);
        public bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, DateTime dateOfBirth, string address)
            => _accountDao.UpdateStaffInformation(userID, fullname, phoneNumber, dateOfBirth, address);
        public bool InactiveAccount(string userID, string note) => _accountDao.InactiveAccount(userID, note);
        public Account GetDetailOfAccount(string userID) => _accountDao.GetDetailOfAccount(userID);
        public bool ActiveAccount(string userID) => _accountDao.ActiveAccount(userID);
    }
}
