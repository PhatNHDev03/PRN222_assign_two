using FUNewsManagementSystem.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.IService
{
    public interface ISystemAccountService
    {
        void SaveSystemAccount(SystemAccount systemAccount);
        void UpdateSystemAccount(SystemAccount systemAccount);

        SystemAccount GetSystemAccountByShort(short id);
        List<SystemAccount> findAllSystem();
        short getLastId();

        void deleteByObject(SystemAccount systemAccount);
        SystemAccount ValidateLogin(string AccountEmail, string accountPassword);
        SystemAccount IsExistEmail(string accountEmail);

        // Profile
        public List<SystemAccount> FindAllWithArticlesWithDate(DateTime startDaTe, DateTime endDate);
        List<SystemAccount> findAllWithArticles();
        SystemAccount findAllWithArticlesById(short id);
        SystemAccount FindAllWithArticlesByIdWithDate(short id, DateTime startDate, DateTime endDate);
        (List<SystemAccount>, int totalItems) findALlWithPagination(int pg, int pageSize);
        bool ValidatePassword(string email, string password);
        void ChangePassword(string email, string newPassword);
    }
}
