using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public FUNewsManagementSystem.BusinessObject.Tag DeleteTag { get; set; }

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet(int id)
        {
            DeleteTag = _tagService.GetTagById(id);
        }

        public IActionResult OnPost()
        {
            _tagService.DeleteTag(DeleteTag.TagId);
            return RedirectToPage("Index");
        }
    }
}
