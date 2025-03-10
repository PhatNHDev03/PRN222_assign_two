﻿using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.BusinessObject.Pagination;
using FUNewsManagementSystem.DataAccess.IRepository;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.DataAccess
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementContext _context;

        public SystemAccountRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public SystemAccount GetSystemAccountByUsernameAndPassword(string accountEmail, string accountPassword)
        {
            return _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == accountEmail && a.AccountPassword == accountPassword);
        }

        public List<SystemAccount> FindAll() => _context.SystemAccounts.OrderByDescending(o => o.AccountId).ToList();

        public short GetLastId() => _context.SystemAccounts.OrderByDescending(o => o.AccountId).First().AccountId;

        public SystemAccount GetSystemAccountById(short id)
            => _context.SystemAccounts.FirstOrDefault(i => i.AccountId == id);

        public void SaveSystemAccount(SystemAccount systemAccount)
        {
            _context.SystemAccounts.Add(systemAccount);
            _context.SaveChanges();
        }

        public void UpdateSystemAccount(SystemAccount systemAccount)
        {
            _context.SystemAccounts.Update(systemAccount);
            _context.SaveChanges();
        }

        public void DeleteByObject(SystemAccount systemAccount)
        {
            var listArticlesUpdater = _context.NewsArticles.Where(x => x.UpdatedById == systemAccount.AccountId).ToList();
            foreach (var article in listArticlesUpdater)
            {
                article.UpdatedById = article.CreatedById;
            }
            _context.SaveChanges();

            var listNewArticlesFound = _context.NewsArticles.Where(x => x.CreatedById == systemAccount.AccountId).ToList();
            var getNewArticlesIdList = listNewArticlesFound.Select(x => x.NewsArticleId).ToList();

            var ListNewTagFound = _context.Set<Dictionary<string, object>>("NewsTag")
              .Where(r => getNewArticlesIdList.Contains(EF.Property<string>(r, "NewsArticleId")))
              .ToList();

            if (ListNewTagFound != null)
            {
                _context.RemoveRange(ListNewTagFound);
                _context.SaveChanges();
            }

            if (listNewArticlesFound != null)
            {
                _context.RemoveRange(listNewArticlesFound);
                _context.SaveChanges();
            }

            _context.SystemAccounts.Remove(systemAccount);
            _context.SaveChanges();
        }

        public SystemAccount GetSystemAccountByShort(short id)
    => _context.SystemAccounts.FirstOrDefault(i => i.AccountId == id);
        public SystemAccount IsExistEmail(string accountEmail)
            => _context.SystemAccounts.FirstOrDefault(x => x.AccountEmail.Equals(accountEmail));

        public List<SystemAccount> FindAllWithArticles()
            => _context.SystemAccounts.Include(x => x.NewsArticles).OrderByDescending(o => o.AccountId).ToList();

        public List<SystemAccount> FindAllWithArticlesWithDate(DateTime startDate, DateTime endDate)
        {
            var check = _context.SystemAccounts.Include(x => x.NewsArticles).OrderByDescending(o => o.AccountId).Include(x => x.NewsArticles
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)).ToList();
            return check;
        }

        public SystemAccount FindAllWithArticlesById(short id)
            => _context.SystemAccounts.Include(x => x.NewsArticles).FirstOrDefault(x => x.AccountId == id);
        public SystemAccount FindAllWithArticlesByIdWithDate(short id, DateTime startDate, DateTime endDate)
           => _context.SystemAccounts
                .Include(x => x.NewsArticles.Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate))
                .FirstOrDefault(x => x.AccountId == id);
        public (List<SystemAccount>, int totalItems) findALlWithPagination(int pg, int pageSize)
        {
            var list = _context.SystemAccounts.OrderByDescending(x=>x.AccountId).ToList();
        
            if (pg == 1) { 
                pg=1;
            }
            int resCount = list.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg-1)*pageSize;
            var data = list.Skip(recSkip).Take(pager.Pagesize).ToList();
            return (data,resCount);
        }

        //Kiểm tra pass
        public bool ValidatePassword(string email, string password)
        {
            var account = _context.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);
            Console.WriteLine($"Validating password for email: {email}, provided password: {password}, stored password: {account?.AccountPassword}");
            if (account == null)
            {
                return false; // Account not found
            }

            return account.AccountPassword == password; // Check if the password matches
        }
        public void ChangePassword(string email, string newPassword)
        {
            var account = IsExistEmail(email);
            if (account != null)
            {
                // Hash the new password and update the account
                account.AccountPassword = newPassword; 
                _context.SystemAccounts.Update(account);
                _context.SaveChanges();
            }
        }
    }

}
