using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    public class MyNewArticleModel : PageModel
    {
        [BindProperty]
        public List<NewsArticle> Articles { get; set; }
        private readonly INewsArticleService _newsArticleService; 
        public MyNewArticleModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        public void OnGet(short id)
        {
            Articles = _newsArticleService.GetAllNewsArticlesByUserId(id);
        }
    }
}
