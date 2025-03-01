using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [BindProperty]
        public Category Category { get; set; }

        public IActionResult OnGet(short id)
        {
            Category = _categoryService.GetCategoryById(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid category data. Please check the form." });
            }

            try
            {
                // Ki?m tra Category không null
                if (Category == null)
                {
                    return new JsonResult(new { success = false, message = "Category model is null." });
                }

                // ??m b?o CategoryName không r?ng
                if (string.IsNullOrWhiteSpace(Category.CategoryName))
                {
                    return new JsonResult(new { success = false, message = "Category Name is required." });
                }

                // X? lý IsActive: N?u không có giá tr? t? checkbox, m?c ??nh là false
                if (!Category.IsActive.HasValue)
                {
                    Category.IsActive = false;
                }

                // N?u ParentCategoryId không h?p l?, ??t v? null
                if (Category.ParentCategoryId.HasValue && Category.ParentCategoryId.Value <= 0)
                {
                    Category.ParentCategoryId = null;
                }

                _categoryService.UpdateCategory(Category);
                return new JsonResult(new { success = true, message = "Category updated successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error updating category: {ex.Message}" });
            }
        }
    }
}