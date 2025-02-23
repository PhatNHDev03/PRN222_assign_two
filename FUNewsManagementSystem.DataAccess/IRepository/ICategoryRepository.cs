using FUNewsManagementSystem.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.DataAccess.IRepository
{
    public interface ICategoryRepository
    {
        public List<Category> GetAllCategories();
        public Category GetCategoryById(short id);
        public void AddCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(short id);
        public List<Category> getAllValidCategory();
    }
}
