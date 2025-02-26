using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public  List<SystemAccount> list { get; set; }
        private readonly ISystemAccountService _systemAccountService;
        public IndexModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }
        public void OnGet()
        {
            list = _systemAccountService.findAllSystem();
        }
    }
}
