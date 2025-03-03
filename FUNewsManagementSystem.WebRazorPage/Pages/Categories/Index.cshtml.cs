using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.BusinessObject.Pagination;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Categories
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        public Pager Pager { get; set; }
        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public List<Category> Categories { get; set; }

        public void OnGet(int pg=1)
        {
            int pageSize = 5;
            var (CategoriesPagination, totalItems) = _categoryService.findALlWithPagination(pg,pageSize);
            Pager = new Pager(totalItems, pg, pageSize);
            Categories = CategoriesPagination;
        }
    }
}