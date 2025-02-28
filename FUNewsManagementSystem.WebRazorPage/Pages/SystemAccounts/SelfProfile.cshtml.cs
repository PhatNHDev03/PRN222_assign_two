using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.WebRazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    [BindProperties]
    public class SelfProfileModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public SelfProfileModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
            SystemAccount = new SystemAccountViewModel();
        }

        public SystemAccountViewModel SystemAccount { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        // Properties for Change Password form
        [BindProperty]
        public string? OldPassword { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        [BindProperty]
        public string? ConfirmNewPassword { get; set; }

        public IActionResult OnGet()
        {
            string? userEmail;
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                // Lấy email
                userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            }
            else
            {
                userEmail = "Not authenticated";
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "User not authenticated!";
                return Redirect("/Index");
            }

            var account = _systemAccountService.IsExistEmail(userEmail);

            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return Redirect("/Index");
            }

            SystemAccount = new SystemAccountViewModel
            {
                AccountId = account.AccountId,
                AccountEmail = account.AccountEmail,
                NewAccountEmail = account.AccountEmail,
                AccountName = account.AccountName,
                Status = account.Status,
                AccountRole = account.AccountRole
            };

            return Page(); 
        }

        public IActionResult OnPost()
        {
            ModelState.Remove("OldPassword");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmNewPassword");
            if (!ModelState.IsValid)
            {
                return Page(); // Trả lại trang Edit.cshtml nếu validation không hợp lệ
            }

            var existingAccount = _systemAccountService.GetSystemAccountByShort((short)SystemAccount.AccountId);
            if (existingAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("SelfProfile");
            }

            // Kiểm tra password 
            if (!_systemAccountService.ValidatePassword(existingAccount.AccountEmail, Password ?? throw new ArgumentNullException(nameof(Password), "Password cannot be null")))
            {
                TempData["ErrorMessage"] = "Incorrect password!";
                return RedirectToPage("SelfProfile");
            }

            // Kiểm tra email có bị trùng không, true if email khác nhau
            bool isEmailChanged = !SystemAccount.AccountEmail.Equals(SystemAccount.NewAccountEmail);

            // Check email có tồn tại kh
            if (isEmailChanged)
            {
                var checkMail = _systemAccountService.IsExistEmail(SystemAccount.NewAccountEmail.Trim());
                if (checkMail != null)
                {
                    TempData["ErrorMessage"] = "Email is already exist !!!";
                    return RedirectToPage("SelfProfile");
                }
            }

            // Update Profile
            existingAccount.AccountName = SystemAccount.AccountName;
            existingAccount.AccountRole = SystemAccount.AccountRole;
            existingAccount.Status = SystemAccount.Status;
            if (isEmailChanged)
            {
                existingAccount.AccountEmail = SystemAccount.NewAccountEmail;
            }
            _systemAccountService.UpdateSystemAccount(existingAccount);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("SelfProfile");
        }

        public IActionResult OnPostChangePassword()
        {
            ModelState.Remove("SystemAccount.AccountName");
            ModelState.Remove("SystemAccount.AccountEmail");
            ModelState.Remove("SystemAccount.NewAccountEmail");
            ModelState.Remove("SystemAccount.Status");
            ModelState.Remove("SystemAccount.AccountRole");
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                return RedirectToPage("SelfProfile");
            }

            var existingAccount = _systemAccountService.GetSystemAccountByShort((short)SystemAccount.AccountId);
            if (existingAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found!";
                return RedirectToPage("SelfProfile");
            }

            // Validate old password
            if (!_systemAccountService.ValidatePassword(existingAccount.AccountEmail, OldPassword ?? throw new ArgumentNullException(nameof(OldPassword), "Password cannot be null")))
            {
                TempData["ErrorMessage"] = "Incorrect old password!";
                return RedirectToPage("SelfProfile");
            }

            // Check if new password and confirmation match
            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmNewPassword) || NewPassword != ConfirmNewPassword)
            {
                TempData["ErrorMessage"] = "New password and confirmation do not match or are empty!";
                return RedirectToPage("SelfProfile");
            }
            // Update the password (assumes _systemAccountService has a method to change password)
            _systemAccountService.ChangePassword(existingAccount.AccountEmail, NewPassword);

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToPage("SelfProfile");
        }
    }
}
