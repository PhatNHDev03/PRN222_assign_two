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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        public Category GetCategoryById(short id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.UpdateCategory(category);
        }

        public void DeleteCategory(short id)
        {
            _categoryRepository.DeleteCategory(id);
        }
        public List<Category> getAllValidCategory() => _categoryRepository.getAllValidCategory();
    }
}
