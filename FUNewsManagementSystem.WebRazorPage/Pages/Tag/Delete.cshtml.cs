using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public FUNewsManagementSystem.BusinessObject.Tag Tag { get; set; }

        public DeleteModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
        }

        public IActionResult OnPost()
        {
            _tagService.DeleteTag(Tag.TagId);
            TempData["SuccessMessage"] = "Tag deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
