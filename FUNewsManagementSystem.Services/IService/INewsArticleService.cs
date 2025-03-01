    using FUNewsManagementSystem.BusinessObject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace FUNewsManagementSystem.Services.IService
    {
        public interface INewsArticleService
        {
            List<NewsArticle> GetAllNewsArticles();

            List<NewsArticle> GetAllNewsArticlesWithDetails();
            NewsArticle GetNewsArticleById(string id);
            void AddNewsArticle(NewsArticle newsArticle);
            void UpdateNewsArticle(NewsArticle newsArticle);
            void DeleteNewsArticle(string id);
            NewsArticle GetLastNewsArticle();
            bool HasArticlesInCategory(int categoryId);

            List<NewsArticle> GetAllNewsArticlesByUserId(short userId);
            List<NewsArticle> GetNewsReportByPeriod(DateTime startDate, DateTime endDate);
            string LastId();
            void save();
            DateTime? LimitEndDate();
            void disableACateGoryId(short categoryId);
            public DateTime FirstCreateDate();

        }
    }
