using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{
    public class EditModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public FUNewsManagementSystem.BusinessObject.Tag Tag { get; set; }

        public EditModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public void OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _tagService.UpdateTag(Tag);
            return RedirectToPage("Index");
        }
    }
}
