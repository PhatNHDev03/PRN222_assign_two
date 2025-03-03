using FUNewsManagementSystem.BusinessObject;
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
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsArticleRepository(FUNewsManagementContext context)
        {
            _context = context;
        }
        public List<NewsArticle> GetAllNewsArticles()
        {
            var r = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Include(n => n.CreatedBy)
                .OrderByDescending(o => o.NewsArticleId)
                .ToList();
            return r;
        }

        public List<NewsArticle> GetAllNewsArticlesWithDetails()
        {
            var r = _context.NewsArticles.Where(a => a.NewsStatus == true)
                .Include(n => n.Category).Include(n => n.Tags).Include(u => u.CreatedBy)

              .OrderByDescending(o => o.NewsArticleId).ToList();
            return r;
        }

        public NewsArticle GetNewsArticleById(string id)
        {
            return _context.NewsArticles.Include(n => n.Tags).Include(n => n.Category)
                .FirstOrDefault(n => n.NewsArticleId == id);
        }
        public List<NewsArticle> getAllNewByStaffId(short id)
        {
            var r = _context.NewsArticles.Where(a => a.NewsStatus == true && a.CreatedById == id)
              .Include(n => n.Category).Include(n => n.Tags).Include(u => u.CreatedBy)

            .OrderByDescending(o => o.NewsArticleId).ToList();
            return r;
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {


            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            var existingArticle = _context.NewsArticles.Find(newsArticle.NewsArticleId);
            if (existingArticle != null)
            {
                _context.Entry(existingArticle).CurrentValues.SetValues(newsArticle);
                _context.SaveChanges();
            }
        }


        public void DeleteNewsArticle(string id)
        {
            var newsArticle = _context.NewsArticles.Include(x => x.Tags).FirstOrDefault(x => x.NewsArticleId == id);


            if (newsArticle != null)
            {
                newsArticle.Tags = null;
                _context.SaveChanges();
                _context.NewsArticles.Remove(newsArticle);
                _context.SaveChanges();
            }
        }
        public NewsArticle GetLastNewsArticle()
        {
            return _context.NewsArticles
                .OrderByDescending(n => n.NewsArticleId)
                .FirstOrDefault();
        }
        public string LastId()
        {
            var list = _context.NewsArticles.ToList();
            int max = 0;
            foreach (var item in list)
            {
                if (int.Parse(item.NewsArticleId.Trim()) >= max)
                {
                    max = int.Parse(item.NewsArticleId.Trim());
                }
            }
            return max + "";
        }
        public List<NewsArticle> GetArticlesByCategory(int categoryId)
        {
            return _context.NewsArticles.Where(n => n.CategoryId == categoryId).ToList();
        }



        public DateTime? LimitEndDate()
        {

            var latestArticle = _context.NewsArticles
                .OrderByDescending(o => o.CreatedDate)
                .FirstOrDefault();


            if (latestArticle == null || !latestArticle.CreatedDate.HasValue)
            {
                return null;
            }

            return latestArticle.CreatedDate.Value.AddDays(1);
        }

        public List<NewsArticle> GetNewsReportByPeriod(DateTime startDate, DateTime endDate)
        {

            if (endDate < startDate)
            {
                return new List<NewsArticle>(); // Trả về danh sách rỗng nếu ngày kết thúc nhỏ hơn ngày bắt đầu
            }

            return _context.NewsArticles
                .Where(n => n.CreatedDate.HasValue && startDate <= n.CreatedDate.Value && n.CreatedDate.Value <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
        public void save() => _context.SaveChanges();


        public void disableACateGoryId(short categoryId)
        {
            var listContaintCateId = _context.NewsArticles.Where(i => i.CategoryId == categoryId).ToList();
            listContaintCateId.ForEach(t => t.CategoryId = null);
            _context.SaveChanges();
        }

        public bool HasArticlesInCategory(int categoryId)
        {
            return _context.NewsArticles.Where(n => n.CategoryId == categoryId).ToList().Any();
        }

        public List<NewsArticle> GetAllNewsArticlesByUserId(short userId)
        {
            return _context.NewsArticles.Where(x => x.CreatedById == userId).
             Include(x => x.Tags).Include(c => c.Category).ToList();
        }
        public DateTime FirstCreateDate() => (DateTime)_context.NewsArticles
            .OrderBy(x => x.CreatedDate).FirstOrDefault().CreatedDate;

        public string GetUpdaterName(short? updatedById)
        {
            if (!updatedById.HasValue)
            {
                return "No Updater";
            }

            return _context.SystemAccounts
                .Where(sa => sa.AccountId == updatedById.Value)
                .Select(sa => sa.AccountName)
                .FirstOrDefault() ?? "No Updater";
        }
        public (List<NewsArticle>, int totalItems) findALlWithPagination(int pg, int pageSize)
        {
            var list = _context.NewsArticles.Include(x => x.Tags).Include(x=>x.CreatedBy).Include(x=>x.Category)
                .AsEnumerable() // Chuyển dữ liệu về bộ nhớ trước khi parse
                  .OrderByDescending(x => int.Parse(x.NewsArticleId))
                 .ToList();
            if (pg == 1)
            {
                pg = 1;
            }
            int resCount = list.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = list.Skip(recSkip).Take(pager.Pagesize).ToList();
            return (data, resCount);
        }

        public NewsArticle getNewShowDetailById(string id)
        {
            var r = _context.NewsArticles.Where(a=>a.NewsArticleId.Equals(id))
              .Include(n => n.Category).Include(n => n.Tags).Include(u => u.CreatedBy)

            .OrderByDescending(o => o.NewsArticleId).FirstOrDefault();
            return r;
        }
    }
}
