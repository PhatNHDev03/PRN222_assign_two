using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    public class DeleteModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public Tag Tag { get; set; }
        private readonly IHubContext<SignalrServer> _hubContext;
        public DeleteModel(ITagService tagService,IHubContext<SignalrServer>hubContext)
        {
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public void OnGet(int id)
        {
            Tag = _tagService.GetTagById(id);
        }

        public async Task<IActionResult> OnPost()
        {
            _tagService.DeleteTag(Tag.TagId);
            TempData["SuccessMessage"] = "Tag deleted successfully!";
            await _hubContext.Clients.All.SendAsync("LoadAllTags");
            return RedirectToPage("Index");
        }
    }
}
