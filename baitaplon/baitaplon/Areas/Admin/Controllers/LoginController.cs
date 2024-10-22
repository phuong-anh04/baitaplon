using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using baitaplon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly EazydealsContext _context;
        // Using toast notify and notyf in this app
        private readonly INotyfService _toastNotification;
        public LoginController(EazydealsContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(Account account, string? returnUrl)
        {
            // Kiểm tra tài khoản trong cơ sở dữ liệu
            var acc = _context.Accounts.FirstOrDefault(x => x.Email == account.Email);
            if (acc != null)
            {
                if (BCrypt.Net.BCrypt.Verify(account.Password, acc.Password))
                {                       
                    if (acc.Active == 1)
                    {
                        if (acc.Role == 1) // Chỉ cho phép Admin
                        {
                            var identity = new ClaimsIdentity(new[]
                            {
                        new Claim("AccountId", acc.Id.ToString()),
                        new Claim(ClaimTypes.Name, acc.FullName),
                        new Claim(ClaimTypes.Email, acc.Email),
                        new Claim("avatar", acc.Image ?? "default.png"),
                        new Claim(ClaimTypes.Role, "Admin"),
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                            var principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            // Hiển thị thông báo đăng nhập thành công
                            _toastNotification.Success("Login successful!", 3);

                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["error"] = "You do not have access!";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Account has been locked!";
                    }
                }
                else
                {
                    
                    TempData["error"] = "Password is incorrect!";
                }
            }
            else
            {
                TempData["error"] = "Account does not exist!";
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Thực hiện đăng xuất
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Kiểm tra lại trạng thái sau khi đăng xuất
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                _toastNotification.Success("Logout successful!", 3);
            }
            else
            {
                _toastNotification.Error("Logout failed!", 3);
            }

            // Reset TempData hoặc session để tránh hiển thị lại thông báo không cần thiết
            TempData.Clear();

            return RedirectToAction("Index", "Login");
        }

    }
}
