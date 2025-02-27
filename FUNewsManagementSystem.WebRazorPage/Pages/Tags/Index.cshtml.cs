using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    public class IndexModel : PageModel
    {
        public List<Tag> Tags { get; set; }
        private readonly ITagService _tagService;


        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;

        }

        public IActionResult OnGet()
        {
            Tags = _tagService.GetAllTags();
            return Page();
        }
    }
}
