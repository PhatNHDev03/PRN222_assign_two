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
        SystemAccount GetProfileByEmail(string email);
        bool ValidateOldPassword(string email, string oldPassword);
        bool UpdateProfile(string email, string newEmail, string accountName, string newPassword);
        bool UpdatePassword(string email, string newPassword);
        List<SystemAccount> findAllWithArticles();
        SystemAccount findAllWithArticlesById(short id);
        SystemAccount FindAllWithArticlesByIdWithDate(short id, DateTime startDate, DateTime endDate);
        (List<SystemAccount>, int totalItems) findALlWithPagination(int pg, int pageSize);
    }
}
