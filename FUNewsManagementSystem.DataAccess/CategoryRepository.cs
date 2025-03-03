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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;

        public CategoryRepository(FUNewsManagementContext context)
        {
            _context = context;
        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.OrderByDescending(o => o.CategoryId).ToList();
        }


        public Category GetCategoryById(short id)
        {
            return _context.Categories.Find(id);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            var existingCategory = _context.Categories
                .Include(c => c.NewsArticles) // Tải danh sách bài báo liên kết
                .FirstOrDefault(c => c.CategoryId == category.CategoryId);

            if (existingCategory != null)
            {
                if (category.IsActive == false) // Kiểm tra trạng thái mới
                {
                    foreach (var news in existingCategory.NewsArticles)
                    {
                        news.CategoryId = null;
                    }
                }

                // Cập nhật dữ liệu mới từ category vào existingCategory
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                _context.SaveChanges();
            }
        }


        public void DeleteCategory(short id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
        public List<Category> getAllValidCategory() => _context.Categories.Where(x => x.IsActive == true).OrderByDescending(o => o.CategoryId).ToList();
        public (List<Category>, int totalItems) findALlWithPagination(int pg, int pageSize)
        {
            var list = _context.Categories.OrderByDescending(x => x.CategoryId).ToList();

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
    }
}
