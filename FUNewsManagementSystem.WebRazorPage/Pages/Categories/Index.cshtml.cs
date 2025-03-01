using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public List<Category> Categories { get; set; }

        public void OnGet()
        {
            Categories = _categoryService.GetAllCategories()
                .OrderBy(c => c.CategoryId) 
                .ToList();
        }
    }
}