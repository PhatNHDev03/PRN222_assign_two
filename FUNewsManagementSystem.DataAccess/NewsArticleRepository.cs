using FUNewsManagementSystem.BusinessObject;
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
                .Include(u => u.CreatedBy)
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
            if (string.IsNullOrEmpty(id))
            {
                return null; 
            }

            return _context.NewsArticles.Include(n => n.Tags).Include(n => n.Category)
                .FirstOrDefault(n => n.NewsArticleId == id);
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var newsArticle = _context.NewsArticles
                        .Include(na => na.Tags) // Bao gồm các tag liên quan để EF quản lý mối quan hệ
                        .FirstOrDefault(na => na.NewsArticleId == id);

                    if (newsArticle != null)
                    {
                        // Xóa tất cả các liên kết giữa NewsArticle và Tag (sẽ xóa các bản ghi trong bảng NewsTag)
                        newsArticle.Tags.Clear(); // Xóa tất cả các tag liên quan, EF sẽ tự động xóa các bản ghi trong bảng NewsTag

                        // Xóa bài viết
                        _context.NewsArticles.Remove(newsArticle);

                        // Lưu thay đổi
                        _context.SaveChanges();

                        // Commit transaction
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    transaction.Rollback();
                    throw new Exception($"Error deleting news article: {ex.Message}", ex);
                }
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
            var allArticles = _context.NewsArticles.ToList(); // Lấy toàn bộ dữ liệu vào bộ nhớ
            var lastArticle = allArticles
                .OrderByDescending(n => n.NewsArticleId != null && int.TryParse(n.NewsArticleId, out int id) ? id : 0)
                .FirstOrDefault();
            return lastArticle?.NewsArticleId ?? "0"; // Trả về "0" nếu không có bản ghi
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
           return  _context.NewsArticles.Where(x => x.CreatedById == userId).
            Include(x => x.Tags).Include(c => c.Category).ToList();
        }
        public DateTime FirstCreateDate ()=> (DateTime) _context.NewsArticles
            .OrderBy(x => x.CreatedDate).FirstOrDefault().CreatedDate;   
    }
}
