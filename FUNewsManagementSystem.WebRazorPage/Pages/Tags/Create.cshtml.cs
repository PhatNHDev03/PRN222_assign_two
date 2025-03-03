using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ITagService _tagService;

       private readonly     IHubContext<SignalrServer> _hubContext;
        public Tag Tag { get; set; }
        public List<Tag> Tags { get; set; }

        public CreateModel(ITagService tagService,IHubContext<SignalrServer> hubContext)
        {
            _tagService = tagService;
            _hubContext = hubContext;
        }
        public IActionResult OnGet()
        {
            Tags = _tagService.GetAllTags();
            return Page();
        }

        public async Task< IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            try
            {
                Tag.TagId = _tagService.getLastId()+1;
                _tagService.AddTag(Tag);
                await _hubContext.Clients.All.SendAsync("LoadAllTags");
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
