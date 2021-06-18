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
        public IEnumerable<Account> ViewAccountsList(int RowsOnPage, int RequestPage) =>
            _accountDao.ViewAccountsList(RowsOnPage, RequestPage);
        public int NumberOfAccounts() => _accountDao.NumberOfAccounts();
    }
}
