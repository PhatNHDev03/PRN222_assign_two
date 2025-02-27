using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    public class DeleteModel : PageModel
    {
        public SystemAccountViewModel MyProperty { get; set; }
        private readonly ISystemAccountService _systemAccountService;

        public DeleteModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public IActionResult OnGet(int id)
        {
            var account = _systemAccountService.GetSystemAccountByShort((short)id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("Index");
            }

            MyProperty = new SystemAccountViewModel
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var account = _systemAccountService.GetSystemAccountByShort((short)id);

            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("Index");
            }
            account.Status = false;
            _systemAccountService.UpdateSystemAccount(account);
            TempData["SuccessMessage"] = "Account deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
