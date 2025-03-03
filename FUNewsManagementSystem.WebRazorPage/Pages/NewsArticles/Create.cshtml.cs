using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace FUNewsManagementSystem.WebRazorPage.Pages.NewsArticles
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly ISystemAccountService _systemAccountService;

        public CreateModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, ISystemAccountService systemAccountService)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _systemAccountService = systemAccountService;
        }

        public NewsArticle NewsArticle { get; set; } = new NewsArticle();
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }



        public List<int> SelectedTags { get; set; }

        public IActionResult OnGet()
        {
            Categories = _categoryService.getAllValidCategory().ToList();
            Tags = _tagService.GetAllTags().ToList();
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ra id
            Tags = _tagService.GetAllTags().ToList();
            NewsArticle.CreatedBy= _systemAccountService.GetSystemAccountByShort(short.Parse(userId));
            NewsArticle.UpdatedById= short.Parse(userId);
            try
            {
          
                NewsArticle.NewsArticleId =(int.Parse(_newsArticleService.LastId())+1)+"";

                // Gán danh sách Tags được chọn
                NewsArticle.Tags = Tags.Where(t => SelectedTags.Contains(t.TagId)).ToList();
                NewsArticle.CreatedDate = DateTime.Now;
                NewsArticle.ModifiedDate = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    Categories = _categoryService.getAllValidCategory().ToList();
                    Tags = _tagService.GetAllTags().ToList();
                    return Page();
                }

                var check = NewsArticle;
                // Lưu vào database
                _newsArticleService.AddNewsArticle(NewsArticle);

                return new JsonResult(new { success = true, message = "News article created successfully!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = $"Error creating news article: {ex.Message}" });
            }
        }
    }
}
