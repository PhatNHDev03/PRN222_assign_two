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

        [BindProperty]
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

            Categories = _categoryService.GetAllCategories().ToList();
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

            // Kiểm tra NewsArticle không null (nên không cần kiểm tra nữa vì đã bind)
            if (NewsArticle == null)
            {
                return BadRequest("News article model is null.");
            }

            // Kiểm tra NewsArticleID không null hoặc rỗng
            if (string.IsNullOrEmpty(NewsArticle.NewsArticleID))
            {
                return BadRequest("Invalid news article ID.");
            }

            var existingArticle = _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleID);
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
            existingArticle.UpdatedById = short.Parse("1"); // Hoặc sử dụng HttpContext.Session.GetString("UserId") nếu đã có
            existingArticle.ModifiedDate = DateTime.Now;

            // Cập nhật Tags
            existingArticle.Tags = NewsArticle.SelectedTags?.Select(tagId =>
            {
                var tag = _tagService.GetTagById(tagId);
                if (tag == null)
                {
                    throw new Exception($"Tag with ID {tagId} not found.");
                }
                return tag;
            }).ToList() ?? new List<Tag>();

            _newsArticleService.UpdateNewsArticle(existingArticle);

            return RedirectToPage("Index");
        }
    }
}