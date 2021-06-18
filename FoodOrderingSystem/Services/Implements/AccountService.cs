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
        public IEnumerable<Account> ViewStaffsList(int RowsOnPage, int RequestPage) =>
            _accountDao.ViewStaffsList(RowsOnPage, RequestPage);
        public int NumberOfStaffs() => _accountDao.NumberOfStaffs();
        public bool CreateStaff(string userEmail, string password, string fullname, string phoneNumber, DateTime dateOfBirth, string address)
            => _accountDao.CreateStaff(userEmail, password, fullname, phoneNumber, dateOfBirth, address);
        public bool UpdateStaffInformation(string userID, string fullname, string phoneNumber, DateTime dateOfBirth, string address)
            => _accountDao.UpdateStaffInformation(userID, fullname, phoneNumber, dateOfBirth, address);
    }
}
