using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccount SystemAccount { get; set; } = new SystemAccount();

        public CreateModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [IgnoreAntiforgeryToken] // Tắt CSRF kiểm tra (có thể bật nếu cần)
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data!" });
            }

            var existingAccount = _systemAccountService.IsExistEmail(SystemAccount.AccountEmail.Trim());
            if (existingAccount != null)
            {
                return new JsonResult(new { success = false, message = "Email already exists!" });
            }

            SystemAccount.AccountId = (short)(_systemAccountService.getLastId() + 1);
            SystemAccount.AccountPassword = "@1";
            _systemAccountService.SaveSystemAccount(SystemAccount);

            return new JsonResult(new { success = true, message = "Account created successfully!", redirectUrl = "/SystemAccounts/Index" });
        }
    }
}
