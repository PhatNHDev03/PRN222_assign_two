using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [BindProperty]
        public Category Category { get; set; }

        public IActionResult OnGet()
        {
            Category = new Category
            {
                IsActive = true // M?c ??nh IsActive l� true khi t?o m?i
            };
            return Page();
        }

        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid category data. Please check the form." });
            }

            try
            {
                // Ki?m tra Category kh�ng null
                if (Category == null)
                {
                    return new JsonResult(new { success = false, message = "Category model is null." });
                }

                // ??m b?o CategoryName v� CategoryDesciption kh�ng r?ng
                if (string.IsNullOrWhiteSpace(Category.CategoryName))
                {
                    return new JsonResult(new { success = false, message = "Category Name is required." });
                }

                // X? l� IsActive: N?u kh�ng c� gi� tr? t? checkbox, m?c ??nh l� false
                if (!Category.IsActive.HasValue)
                {
                    Category.IsActive = false;
                }

                // N?u ParentCategoryId kh�ng h?p l?, ??t v? null
                if (Category.ParentCategoryId.HasValue && Category.ParentCategoryId.Value <= 0)
                {
                    Category.ParentCategoryId = null;
                }

                _categoryService.AddCategory(Category);
                return new JsonResult(new { success = true, message = "Category created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error creating category: {ex.Message}" });
            }
        }
    }
}