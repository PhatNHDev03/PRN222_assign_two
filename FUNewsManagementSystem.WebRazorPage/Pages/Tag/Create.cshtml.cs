using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace FUNewsManagementSystem.WebRazorPage.Pages.Tag
{
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public FUNewsManagementSystem.BusinessObject.Tag Tag { get; set; } 

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("OnPost called with TagName: " + Tag?.TagName + ", Note: " + Tag?.Note);
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine("ModelState is invalid. Errors: " + errors);
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            try
            {
                _tagService.AddTag(Tag);
                return new JsonResult(new { success = true, message = "Tag created successfully!", redirectUrl = "/Tag/Index" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.ToString());
                return new JsonResult(new { success = false, message = $"Error creating tag: {ex.Message}" });
            }
        }

    }
}
