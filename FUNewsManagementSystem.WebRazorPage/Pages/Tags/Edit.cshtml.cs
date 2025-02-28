using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly ITagService _tagService;

        [BindProperty]
        public Tag Tag { get; set; }
        private readonly IHubContext<SignalrServer> _hubContext;
        public EditModel(ITagService tagService, IHubContext<SignalrServer>hubContext)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _tagService.UpdateTag(Tag);
            await _hubContext.Clients.All.SendAsync("LoadAllTags");
            return RedirectToPage("Index");
        }
    }
}
