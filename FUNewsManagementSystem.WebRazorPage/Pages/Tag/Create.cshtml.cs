using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public FUNewsManagementSystem.BusinessObject.Tag NewTag { get; set; }

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _tagService.AddTag(NewTag);
            return RedirectToPage("Index");
        }
    }
}
