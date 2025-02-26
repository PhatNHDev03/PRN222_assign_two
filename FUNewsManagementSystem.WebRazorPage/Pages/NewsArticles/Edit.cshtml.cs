using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public EditModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }


        public NewsArticleViewModel NewsArticle { get; set; }

        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }

        public IActionResult OnGet(int id)
        {
            var article = _newsArticleService.GetNewsArticleById(id.ToString());
            if (article == null)
            {
                return NotFound();
            }

            NewsArticle = new NewsArticleViewModel
            {
                NewsArticleID = article.NewsArticleId,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryID = article.CategoryId,
                NewsStatus = (bool)article.NewsStatus,
                SelectedTags = article.Tags?.Select(t => t.TagId).ToList() ?? new List<int>()
            };

            Categories = _categoryService.getAllValidCategory().ToList();
            Tags = _tagService.GetAllTags().ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Categories = _categoryService.GetAllCategories();
                Tags = _tagService.GetAllTags();
                return Page();
            }

            var existingArticle = _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleID.ToString());
            if (existingArticle == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin bài báo
            existingArticle.NewsTitle = NewsArticle.NewsTitle;
            existingArticle.Headline = NewsArticle.Headline;
            existingArticle.NewsContent = NewsArticle.NewsContent;
            existingArticle.NewsSource = NewsArticle.NewsSource;
            existingArticle.CategoryId = NewsArticle.CategoryID;
            existingArticle.NewsStatus = NewsArticle.NewsStatus;
            existingArticle.UpdatedById = short.Parse(1 + "");
            // existingArticle.UpdatedById = short.Parse(HttpContext.Session.GetString("UserId"));
            existingArticle.ModifiedDate = DateTime.Now;

            // Cập nhật Tags
            existingArticle.Tags = NewsArticle.SelectedTags?.Select(tagId => _tagService.GetTagById(tagId)).ToList() ?? new List<Tag>();

            _newsArticleService.UpdateNewsArticle(existingArticle);

            return RedirectToPage("Index");
        }
    }
}
