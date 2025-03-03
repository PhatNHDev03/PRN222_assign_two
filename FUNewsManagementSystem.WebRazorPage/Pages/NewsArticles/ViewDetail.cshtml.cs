using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    [BindProperties]
    public class ViewDetailModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ISystemAccountService _systemAccountService;
        public NewsArticle NewsArticle { get; set; }
        public SystemAccount SystemAccount { get; set; }
        public ViewDetailModel(INewsArticleService newsArticleService,ISystemAccountService systemAccountService)
        {
            _newsArticleService = newsArticleService;
            _systemAccountService = systemAccountService;
        }
        public void OnGet(string id)
        {
            NewsArticle = _newsArticleService.getNewShowDetailById(id);
            SystemAccount = _systemAccountService.GetSystemAccountByShort((short)NewsArticle.UpdatedById);
        }
    }
}
