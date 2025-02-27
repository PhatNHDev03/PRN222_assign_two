using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FUNewsManagementSystem.Services.IService;
using FUNewsManagementSystem.BusinessObject;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public LoginModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; } = new LoginInputModel();

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Index"); 
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _systemAccountService.ValidateLogin(LoginInput.Email, LoginInput.Password);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Kiểm tra nếu cột Status tồn tại và đảm bảo tài khoản còn hoạt động
            if (user.Status != null && user.Status == false)
            {
                ErrorMessage = "Your account is inactive. Please contact admin.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.AccountName),
                new Claim(ClaimTypes.Email, user.AccountEmail),
                new Claim(ClaimTypes.Role, user.AccountRole.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);


            return RedirectToPage("/Index"); // Chuyển hướng đến trang Home sau khi login
        }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
