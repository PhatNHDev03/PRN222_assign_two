using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
            var user = User.FindFirstValue(ClaimTypes.Role);
            if (user != "Staff") {
                return RedirectToPage("/Index");
            }
            NewsArticles = _newsArticleService.GetAllNewsArticles().OrderByDescending(x => int.Parse(x.NewsArticleId))
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
