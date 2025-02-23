using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.DataAccess;
using FUNewsManagementSystem.DataAccess.IRepository;
using FUNewsManagementSystem.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        public SystemAccountService(ISystemAccountRepository systemAccountRepository)
        {
            _systemAccountRepository = systemAccountRepository;

        }
        public SystemAccount ValidateLogin(string AccountEmail, string accountPassword)
        {
            return _systemAccountRepository.GetSystemAccountByUsernameAndPassword(AccountEmail, accountPassword);
        }
        public void SaveSystemAccount(SystemAccount systemAccount)
        {
            _systemAccountRepository.SaveSystemAccount(systemAccount);
        }


        public List<SystemAccount> findAllSystem()
        => _systemAccountRepository.FindAll();

        public SystemAccount GetSystemAccountByShort(short id)
        => _systemAccountRepository.GetSystemAccountByShort(id);



        public void UpdateSystemAccount(SystemAccount systemAccount)
        => _systemAccountRepository.UpdateSystemAccount(systemAccount);
        public short getLastId() => _systemAccountRepository.GetLastId();
        public void deleteByObject(SystemAccount systemAccount) => _systemAccountRepository.DeleteByObject(systemAccount);

        public SystemAccount IsExistEmail(string accountEmail) => _systemAccountRepository.IsExistEmail(accountEmail);

        //Profile
        public SystemAccount GetProfileByEmail(string email)
        {
            return _systemAccountRepository.IsExistEmail(email);
        }

        public bool ValidateOldPassword(string email, string oldPassword)
        {
            var account = _systemAccountRepository.IsExistEmail(email);
            return account != null && account.AccountPassword == oldPassword;
        }

        public bool UpdateProfile(string email, string newEmail, string accountName, string newPassword)
        {
            var account = _systemAccountRepository.IsExistEmail(email);
            if (account == null) return false;

            Console.WriteLine($"Before Update - Email: {account.AccountEmail}, Password: {account.AccountPassword}");

            if (!string.IsNullOrEmpty(newEmail)) account.AccountEmail = newEmail;
            if (!string.IsNullOrEmpty(accountName)) account.AccountName = accountName;
            if (!string.IsNullOrEmpty(newPassword))
            {
                Console.WriteLine($"Updating Password: {newPassword}");
                account.AccountPassword = newPassword;
            }

            _systemAccountRepository.UpdateSystemAccount(account);
            Console.WriteLine($"After Update - Email: {account.AccountEmail}, Password: {account.AccountPassword}");
            return true;
        }
        public bool UpdatePassword(string email, string newPassword)
        {
            var account = _systemAccountRepository.IsExistEmail(email);
            if (account == null) return false;

            account.AccountPassword = newPassword; 
            _systemAccountRepository.UpdateSystemAccount(account);
            return true;
        }


        List<SystemAccount> ISystemAccountService.findAllWithArticles()
        {
            return _systemAccountRepository.FindAllWithArticles();
        }



        public SystemAccount findAllWithArticlesById(short id)
        {
            return _systemAccountRepository.FindAllWithArticlesById(id);
        }
    }
}
