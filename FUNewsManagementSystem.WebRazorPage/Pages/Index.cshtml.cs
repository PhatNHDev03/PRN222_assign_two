using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.BusinessObject;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Home
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;

        public List<NewsArticle> Articles { get; set; }
        public NewsArticle RandomArticle { get; set; }

        public IndexModel(INewsArticleService newsArticleService, ICategoryService categoryService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
        }

        public void OnGet()
        {
            Articles = _newsArticleService.GetAllNewsArticlesWithDetails()
                                          .ToList();

            RandomArticle = Articles.OrderBy(a => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
