using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public CreateModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [BindProperty]
        public NewsArticleViewModel NewsArticleViewModel { get; set; } = new NewsArticleViewModel();

        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }

        public IActionResult OnGet()
        {
            Categories = _categoryService.getAllValidCategory().ToList();
            Tags = _tagService.GetAllTags().ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = _categoryService.getAllValidCategory().ToList();
                Tags = _tagService.GetAllTags().ToList();
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = "Validation failed: " + string.Join(", ", errors) });
            }

            try
            {
                // L?y ID l?n nh?t hi?n có và c?ng thêm 1
                string lastId = _newsArticleService.LastId();
                int newId = string.IsNullOrEmpty(lastId) ? 1 : int.Parse(lastId) + 1;

                var newsArticle = new NewsArticle
                {
                    NewsArticleId = newId.ToString(), 
                    NewsTitle = NewsArticleViewModel.NewsTitle,
                    Headline = NewsArticleViewModel.Headline,
                    NewsContent = NewsArticleViewModel.NewsContent,
                    NewsSource = NewsArticleViewModel.NewsSource,
                    CategoryId = NewsArticleViewModel.CategoryID,
                    NewsStatus = NewsArticleViewModel.NewsStatus,
                    CreatedDate = DateTime.Now,
                    CreatedById = 1, 
                    ModifiedDate = null,
                    UpdatedById = null
                };

                if (NewsArticleViewModel.SelectedTags != null && NewsArticleViewModel.SelectedTags.Any())   
                {
                    newsArticle.Tags = NewsArticleViewModel.SelectedTags
                        .Select(tagId => _tagService.GetTagById(tagId))
                        .Where(tag => tag != null)
                        .ToList();
                }

                _newsArticleService.AddNewsArticle(newsArticle);
                return new JsonResult(new { success = true, message = "News article created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error creating news article: {ex.Message}" });
            }
        }
    }
}