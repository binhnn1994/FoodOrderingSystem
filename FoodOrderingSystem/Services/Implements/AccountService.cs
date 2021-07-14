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
        private IAccountDAO _accountDAO;
        public AccountService(IAccountDAO accountDao) => _accountDAO = accountDao;
        public IEnumerable<Account> ViewAccountListByRole(string roleName, int RowsOnPage, int RequestPage) 
            => _accountDAO.ViewAccountListByRole(roleName, RowsOnPage, RequestPage);
        public int NumberOfAccountByRole(string roleName) => _accountDAO.NumberOfAccountByRole(roleName);
        public bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, string address)
            => _accountDAO.CreateStaff(userEmail, password, fullname, phoneNumber, address);
        public bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, string address)
            => _accountDAO.UpdateStaffInformation(userID, fullname, phoneNumber, address);
        public bool InactiveAccount(string userID, string note) => _accountDAO.InactiveAccount(userID, note);
        public Account GetDetailOfAccount(string userID) => _accountDAO.GetDetailOfAccount(userID);
        public bool ActiveAccount(string userID) => _accountDAO.ActiveAccount(userID);
        public IEnumerable<Account> ViewAccountListBySearching(string searchValue, string roleName, int RowsOnPage, int RequestPage)
            => _accountDAO.ViewAccountListBySearching(searchValue, roleName, RowsOnPage, RequestPage);
        public int NumberOfAccountBySearching(string searchValue, string roleName) 
            => _accountDAO.NumberOfAccountBySearching(searchValue, roleName);
        public Account Login(string userEmail, string password) => _accountDAO.Login(userEmail, password);
        public bool Register(string userEmail, string password, string fullname, string phoneNumber, string address) => _accountDAO.Register(userEmail, password, fullname, phoneNumber, address);
    }
}
