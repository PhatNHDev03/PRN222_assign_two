using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public EditModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public SystemAccountViewModel SystemAccount { get; set; }

        public IActionResult OnGet(int id)
        {
            var account = _systemAccountService.GetSystemAccountByShort((short)id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("Index");
            }

            SystemAccount = new SystemAccountViewModel
            {
                AccountId = account.AccountId,
                AccountEmail = account.AccountEmail,
                NewAccountEmail = account.AccountEmail,
                AccountName = account.AccountName,
                AccountRole = account.AccountRole
            };

            return Page(); // Trả về trang Edit.cshtml
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Trả lại trang Edit.cshtml nếu validation không hợp lệ
            }

            var existingAccount = _systemAccountService.GetSystemAccountByShort((short)SystemAccount.AccountId);
            if (existingAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("Index");
            }

            // Kiểm tra email có bị trùng không
            if (!SystemAccount.AccountEmail.Equals(SystemAccount.NewAccountEmail))
            {
                var checkMail = _systemAccountService.IsExistEmail(SystemAccount.NewAccountEmail.Trim());
                if (checkMail != null)
                {
                    TempData["ErrorMessage"] = "Email is already exist !!!";
                    return RedirectToPage("Index");
                }
            }

            existingAccount.AccountName = SystemAccount.AccountName;
            existingAccount.AccountEmail = SystemAccount.NewAccountEmail;
            existingAccount.AccountRole = SystemAccount.AccountRole;
            existingAccount.Status = SystemAccount.Status;
            _systemAccountService.UpdateSystemAccount(existingAccount);

            TempData["SuccessMessage"] = "Account updated successfully!";
            return RedirectToPage("Index");
        }
    }
}

