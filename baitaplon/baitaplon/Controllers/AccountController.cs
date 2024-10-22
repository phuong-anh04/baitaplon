using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using baitaplon.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace baitaplon.Controllers
{
    public class AccountController : Controller
    {
        private readonly EazydealsContext _context;
        // Using toast notify and notyf in this app
        private readonly INotyfService _toastNotification;
        public AccountController(EazydealsContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Login(Account account, string? returnUrl)
        //{
        //    System.Diagnostics.Debug.WriteLine("Email truyền vào : " + account.Email);
        //    var acc = _context.Accounts.FirstOrDefault(x => x.Email == account.Email);

        //    if (acc != null)
        //    {
        //        if (acc.Password == account.Password)
        //        {
        //            if (acc.Active == 1)
        //            {
        //                if (acc.Role == 1)
        //                {
        //                    System.Diagnostics.Debug.WriteLine("chạy vao day" + acc.Role.GetType());
        //                    var identity = new ClaimsIdentity(new[]
        //                    {
        //                            new Claim("AccountId", acc.Id.ToString()),
        //                            new Claim(ClaimTypes.Name, acc.FullName ?? string.Empty),
        //                            new Claim(ClaimTypes.Email, acc.Email ?? string.Empty),
        //                            new Claim("avatar", acc.Image ?? "default.png"),
        //                            new Claim(ClaimTypes.Role, acc.Role == 1 ? "Admin" : "User"),
        //                        }, CookieAuthenticationDefaults.AuthenticationScheme);
        //                    var principal = new ClaimsPrincipal(identity);
        //                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //                    if (!string.IsNullOrEmpty(returnUrl))
        //                    {
        //                        return Redirect(returnUrl);
        //                    }
        //                    // _toastNotification.Success("Đăng nhập thành công !", 5);
        //                    TempData["LoginSuccess"] = "Đăng nhập thành công!";
        //                    return RedirectToAction("Index", "Home");
        //                }
        //                else
        //                {
        //                    TempData["error"] = "Bạn không có quyền truy cập!";
        //                }
        //            }
        //            else
        //            {
        //                TempData["error"] = "Tài khoản đã bị khóa!";
        //            }
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("Đăng nhập thất bại!");

        //            TempData["error"] = "Mật khẩu không chính xác!";
        //        }
        //    }
        //    else
        //    {
        //        TempData["error"] = "Tài khoản không tồn tại!";
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Login(Account account, string? returnUrl)
        {
            System.Diagnostics.Debug.WriteLine("Email truyền vào : " + account.Email);
            var acc = _context.Accounts.FirstOrDefault(x => x.Email == account.Email);

            if (acc != null)
            {
                // Xác thực mật khẩu bằng BCrypt
                if (BCrypt.Net.BCrypt.Verify(account.Password, acc.Password))
                {
                    if (acc.Active == 1)
                    {
                        
                        System.Diagnostics.Debug.WriteLine("chạy vao day" + acc.Role.GetType());
                        var identity = new ClaimsIdentity(new[]
                        {
                            new Claim("AccountId", acc.Id.ToString()),
                            new Claim(ClaimTypes.Name, acc.FullName ?? string.Empty),
                            new Claim(ClaimTypes.Email, acc.Email ?? string.Empty),
                            new Claim("avatar", acc.Image ?? "default.png"),
                            new Claim(ClaimTypes.Role, acc.Role == 1 ? "Admin" : "User"),
                        }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        System.Diagnostics.Debug.WriteLine("Role stored in claim: " + identity.FindFirst(ClaimTypes.Role)?.Value);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        // _toastNotification.Success("Đăng nhập thành công !", 5);
                        /*TempData["LoginSuccess"] = "Đăng nhập thành công!";*/
                        return RedirectToAction("Index", "Home");
                     
                        
                    }
                    else
                    {
                        TempData["error"] = "Tài khoản đã bị khóa!";
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Đăng nhập thất bại!");

                    TempData["error"] = "Mật khẩu không chính xác!";
                }
            }
            else
            {
                TempData["error"] = "Tài khoản không tồn tại!";
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Logout()
        {
            /*_toastNotification.Success("Đăng xuất thành công!", 3);*/
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (login.IsCompletedSuccessfully)
            {
                return RedirectToAction("Login", "Account");
            }
            _toastNotification.Error("Đăng xuất thất bại !", 3);
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("Email: " + model.Email);
            System.Diagnostics.Debug.WriteLine("Password: " + model.Password);
            System.Diagnostics.Debug.WriteLine("ConfirmPassword: " + model.ConfirmPassword);

            // Kiểm tra tính hợp lệ của dữ liệu
            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu bằng BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Tạo đối tượng Account mới với mật khẩu đã mã hóa
                var newAccount = new Account
                {
                    UserName = model.Email,  // Giả sử UserName là email, bạn có thể thay đổi theo nhu cầu
                    Email = model.Email,
                    Password = hashedPassword, // LAưu mật khẩu đã mã hóa
                    // Giả sử bạn có thêm trường Phone trong RegisterViewModel
                    Role = 0,  // Giả sử Role mặc định là 0 (người dùng bình thường)
                    Active = 1
                };
                System.Diagnostics.Debug.WriteLine("UserName: " + newAccount.UserName);
                System.Diagnostics.Debug.WriteLine("Email: " + newAccount.Email);
                System.Diagnostics.Debug.WriteLine("Password: " + newAccount.Password);

                // Lưu vào cơ sở dữ liệu
                _context.Accounts.Add(newAccount);  // Sử dụng _context.Account thay vì _context.Users
                _context.SaveChanges();

                // Sau khi tạo tài khoản thành công, chuyển hướng tới trang đăng nhập
                TempData["SuccessMessage"] = "Đăng ký thành công. Vui lòng đăng nhập!";
                return RedirectToAction("Login");
            }


            return View(model);
        }
    }

}