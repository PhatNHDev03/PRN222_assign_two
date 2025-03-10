﻿using FUNewsManagementSystem.BusinessObject;
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


        List<SystemAccount> ISystemAccountService.findAllWithArticles()
        {
            return _systemAccountRepository.FindAllWithArticles();
        }



        public SystemAccount findAllWithArticlesById(short id)
        {
            return _systemAccountRepository.FindAllWithArticlesById(id);
        }

        public List<SystemAccount> FindAllWithArticlesWithDate(DateTime startDaTe, DateTime endDate)
        {
            return _systemAccountRepository.FindAllWithArticlesWithDate(startDaTe, endDate);
        }

        public SystemAccount FindAllWithArticlesByIdWithDate(short id, DateTime startDate, DateTime endDate)
        {
            return _systemAccountRepository.FindAllWithArticlesByIdWithDate(id, startDate, endDate);
        }

        public (List<SystemAccount>, int totalItems) findALlWithPagination(int pg, int pageSize)
        {
            return _systemAccountRepository.findALlWithPagination(pg,  pageSize);
        }

        public bool ValidatePassword(string email, string password)
        {
            return _systemAccountRepository.ValidatePassword(email, password); 
        }

        public void ChangePassword(string email, string newPassword)
        {
            _systemAccountRepository.ChangePassword(email, newPassword);
        }

        
    }
}
