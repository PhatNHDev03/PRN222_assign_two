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
            _newsArticleService = newsArticleService;
        }

        public IList<NewsArticle> NewsArticles { get; set; }

        public void OnGet()
        {
            NewsArticles = _newsArticleService.GetAllNewsArticles()
                .OrderBy(n => int.TryParse(n.NewsArticleId, out int id) ? id : 0) // S?p x?p theo giá tr? s?
                .ToList();
        }
    }
}