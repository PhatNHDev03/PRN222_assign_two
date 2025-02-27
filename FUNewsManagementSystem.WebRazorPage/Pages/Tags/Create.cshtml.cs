using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

       
        public Tag Tag { get; set; }
        public List<Tag> Tags { get; set; }

        public CreateModel(ITagService tagService)
        {
            _tagService = tagService;
        }
        public IActionResult OnGet()
        {
            Tags = _tagService.GetAllTags();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            try
            {
                Tag.TagId = _tagService.getLastId()+1;
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
