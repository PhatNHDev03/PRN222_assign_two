﻿using FUNewsManagementSystem.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.DataAccess.IRepository
{
    public interface ISystemAccountRepository
    {
      
        SystemAccount GetSystemAccountByShort(short id);
        SystemAccount GetSystemAccountByUsernameAndPassword(string accountEmail, string accountPassword);
        List<SystemAccount> FindAll();
        short GetLastId();
  
        SystemAccount GetSystemAccountById(short id);
        void SaveSystemAccount(SystemAccount systemAccount);
        void UpdateSystemAccount(SystemAccount systemAccount);
        void DeleteByObject(SystemAccount systemAccount);
        SystemAccount IsExistEmail(string accountEmail);
        List<SystemAccount> FindAllWithArticles();
        SystemAccount FindAllWithArticlesById(short id);
    }

}
