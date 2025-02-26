using FUNewsManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{

    public class IndexModel : PageModel
    {
        private readonly ITagService _tagService;
        public List<FUNewsManagementSystem.BusinessObject.Tag> Tags { get; set; }

        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet()
        {
            Tags = _tagService.GetAllTags();
        }
    }
}
