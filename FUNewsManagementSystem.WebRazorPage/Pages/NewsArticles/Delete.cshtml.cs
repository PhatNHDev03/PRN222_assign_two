using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DeleteModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public string NewsArticleId { get; set; }  // Dùng ?? bind ID khi submit
        public NewsArticle NewsArticle { get; set; }

        // Khi fetch (GET) vào /NewsArticles/Delete?id=xxx
        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // L?y thông tin bài vi?t ?? hi?n th? lên modal
            NewsArticle = _newsArticleService.GetNewsArticleById(id);

            if (NewsArticle == null)
            {
                return NotFound();
            }

            // Bind ID cho form hidden
            NewsArticleId = NewsArticle.NewsArticleId;

            return Page(); // Tr? v? partial view Delete.cshtml
        }

        // Khi Ajax POST ??n /NewsArticles/Delete?handler=Delete
        public IActionResult OnPostDelete()
        {
            if (string.IsNullOrEmpty(NewsArticleId))
            {
                return new JsonResult(new { success = false, message = "Invalid Article ID." });
            }
            try
            {
                _newsArticleService.DeleteNewsArticle(NewsArticleId);
                return new JsonResult(new { success = true, message = "News article deleted successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error deleting news article: {ex.Message}, Details: {ex.InnerException?.Message}" });
            }

        }
    }
}
