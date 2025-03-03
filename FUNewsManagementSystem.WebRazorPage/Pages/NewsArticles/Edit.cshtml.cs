using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    [BindProperties]
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

        public NewsArticle NewsArticle { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<int> SelectedTags { get; set; } = new List<int>();
    
        public IActionResult OnGet(int id)
        {
            var article = _newsArticleService.GetNewsArticleById(id.ToString());
            if (article == null)
            {
                return NotFound();
            }

            NewsArticle = article;
            SelectedTags = article.Tags?.Select(t => t.TagId).ToList() ?? new List<int>();

            Categories = _categoryService.getAllValidCategory().ToList();
            Tags = _tagService.GetAllTags().ToList();
        
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                Categories = _categoryService.getAllValidCategory().ToList();
                Tags = _tagService.GetAllTags().ToList();
                return Page();
            }
            Tags = _tagService.GetAllTags().ToList();
            var existingArticle = _newsArticleService.GetNewsArticleById(NewsArticle.NewsArticleId);
            if (existingArticle == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin bài viết
            existingArticle.NewsTitle = NewsArticle.NewsTitle;
            existingArticle.Headline = NewsArticle.Headline;
            existingArticle.NewsContent = NewsArticle.NewsContent;
            existingArticle.NewsSource = NewsArticle.NewsSource;
            existingArticle.CategoryId = NewsArticle.CategoryId;
            existingArticle.NewsStatus = NewsArticle.NewsStatus;
            existingArticle.ModifiedDate = DateTime.Now;
            existingArticle.UpdatedById = short.Parse(userId);
            // Cập nhật danh sách tags
            existingArticle.Tags = Tags.Where(t => SelectedTags.Contains(t.TagId)).ToList();

            // Lưu dữ liệu vào database
            _newsArticleService.UpdateNewsArticle(existingArticle);

       
            return  RedirectToPage("Index");
        }
    }
}
