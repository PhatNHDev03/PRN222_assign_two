using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService ?? throw new ArgumentNullException(nameof(newsArticleService));
        }

        public List<NewsArticle> NewsArticles { get; set; }

        public IActionResult OnGet()
        {
            NewsArticles = _newsArticleService.GetAllNewsArticles()
                .ToList();
            return Page();
        }

        // Ph??ng th?c ?? l?y AccountName c?a SystemAccount d?a trên UpdatedById t? INewsArticleService
        public string GetUpdaterName(short? updatedById)
        {
            return _newsArticleService.GetUpdaterName(updatedById);
        }
    }
}
