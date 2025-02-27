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
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsArticleService(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        public List<NewsArticle> GetAllNewsArticlesWithDetails()
        {
            return _newsArticleRepository.GetAllNewsArticlesWithDetails();
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return _newsArticleRepository.GetNewsArticleById(id);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.AddNewsArticle(newsArticle);
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _newsArticleRepository.UpdateNewsArticle(newsArticle);
        }

        public void DeleteNewsArticle(string id)
        {
            _newsArticleRepository.DeleteNewsArticle(id);
        }
        public NewsArticle GetLastNewsArticle()
        {
            return _newsArticleRepository.GetLastNewsArticle();
        }
        public bool HasArticlesInCategory(int categoryId)
        {
            return _newsArticleRepository.HasArticlesInCategory(categoryId);
        }


        List<NewsArticle> INewsArticleService.GetAllNewsArticlesByUserId(short userId) => _newsArticleRepository.GetAllNewsArticlesByUserId(userId);

        public List<NewsArticle> GetNewsReportByPeriod(DateTime startDate, DateTime endDate)
        {
            return _newsArticleRepository.GetNewsReportByPeriod(startDate, endDate);
        }

        public string LastId() => _newsArticleRepository.LastId();

        public void save() => _newsArticleRepository.save();

        public DateTime? LimitEndDate() => _newsArticleRepository.LimitEndDate();

        public void disableACateGoryId(short categoryId)
        {
            _newsArticleRepository.disableACateGoryId(categoryId);
        }

        public DateTime FirstCreateDate()=>_newsArticleRepository.FirstCreateDate();
       
    }
}
