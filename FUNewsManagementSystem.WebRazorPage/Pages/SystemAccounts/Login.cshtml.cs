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
        private readonly IConfiguration _configuration;
        public LoginModel(ISystemAccountService systemAccountService, IConfiguration configuration)
        {
            _systemAccountService = systemAccountService;
            _configuration = configuration;
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

            var claims = new List<Claim>();
            ClaimsIdentity identity = null;
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            // Kiểm tra nếu đăng nhập là Admin
            if (adminEmail == LoginInput.Email && adminPassword == LoginInput.Password)
            {
                claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Email, adminEmail),
            new Claim(ClaimTypes.Role, "Admin") // Viết hoa chữ "Admin" cho thống nhất với hệ thống
        };

                identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                return RedirectToPage("/Index");
            }

            // Kiểm tra đăng nhập user thông thường
            var user = _systemAccountService.ValidateLogin(LoginInput.Email, LoginInput.Password);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Kiểm tra tài khoản có bị vô hiệu hóa không
            if (user.Status == false)
            {
                ErrorMessage = "Your account is inactive. Please contact admin.";
                return Page();
            }

            // Gán quyền và thông tin user
            claims = new List<Claim>
    {
                new Claim(ClaimTypes.NameIdentifier,user.AccountId.ToString()),
        new Claim(ClaimTypes.Name, user.AccountName),
        new Claim(ClaimTypes.Email, user.AccountEmail),
        new Claim(ClaimTypes.Role,( user.AccountRole==1)?"Staff":"Lecture")
    };
            
            identity = new ClaimsIdentity(claims, "Cookies");
            var principalUser = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principalUser);
            return RedirectToPage("/Index");
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
