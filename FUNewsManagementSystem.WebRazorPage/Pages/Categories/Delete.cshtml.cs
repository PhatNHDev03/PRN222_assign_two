using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.DataAccess.IRepository;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleRepository _newsArticleRepository; // Inject ?? ki?m tra NewsArticles

        public DeleteModel(ICategoryService categoryService, INewsArticleRepository newsArticleRepository)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _newsArticleRepository = newsArticleRepository ?? throw new ArgumentNullException(nameof(newsArticleRepository));
        }

        [BindProperty]
        public short CategoryId { get; set; }

        public Category Category { get; set; }

        public IActionResult OnGet(short id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            Category = _categoryService.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound();
            }

            CategoryId = Category.CategoryId;
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            if (CategoryId <= 0)
            {
                return new JsonResult(new { success = false, message = "Invalid Category ID." });
            }

            try
            {
                // L?y category ?? ki?m tra
                var category = _categoryService.GetCategoryById(CategoryId);
                if (category == null)
                {
                    return new JsonResult(new { success = false, message = "Category not found." });
                }

                // Ki?m tra xem category có NewsArticle nào liên quan không
                if (_newsArticleRepository.HasArticlesInCategory(CategoryId))
                {
                    return new JsonResult(new { success = false, message = "Cannot delete this category because it contains associated news articles." });
                }

                // Ki?m tra và x? lý các category con (InverseParentCategory)
                if (category.InverseParentCategory != null && category.InverseParentCategory.Any())
                {
                    foreach (var childCategory in category.InverseParentCategory.ToList())
                    {
                        childCategory.ParentCategoryId = null; // ??t ParentCategoryId c?a các con v? null
                    }
                }

                // Xóa category
                _categoryService.DeleteCategory(CategoryId);
                return new JsonResult(new { success = true, message = "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error deleting category: {ex.Message}" });
            }
        }
    }
}