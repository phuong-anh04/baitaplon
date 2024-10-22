using AspNetCoreHero.ToastNotification.Abstractions;
using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitaplon.Controllers
{
    [Authorize]
    public class WistlistController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly INotyfService _toastNotification;
        public WistlistController(EazydealsContext context)
        {
            _context = context;
           
        }
        public IActionResult Index()
        {
            var accountId = User.FindFirst("AccountId")?.Value;
            
            System.Diagnostics.Debug.WriteLine("dsafasdfsda"+accountId);
            
            ViewBag.Categories = _context.Categories.ToList();
            if (int.TryParse(accountId, out int parsedAccountId))
            {
                // Lấy danh sách wishlist của người dùng hiện tại
                var wishlist = _context.Wishlist
                                       .Where(w => w.AccountId == parsedAccountId)
                                       .Include(w => w.Product) // Bao gồm thông tin sản phẩm nếu cần
                                       .ToList();

                // Truyền danh sách wishlist vào View
                ViewBag.Wishlists = wishlist;
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult AddToWishlist(int productId)
        {
            // Lấy thông tin AccountId từ Claims
            var accountId = User.FindFirst("AccountId")?.Value;

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (accountId == null)
            {
                return Json(new { success = true }); ;
            }

            // Kiểm tra nếu sản phẩm đã có trong wishlist
            var existingWishlist = _context.Wishlist
                .FirstOrDefault(w => w.AccountId == int.Parse(accountId) && w.ProductId == productId);

            if (existingWishlist == null)
            {
                // Nếu chưa có, thêm vào wishlist
                var wishlist = new Wishlist
                {
                    AccountId = int.Parse(accountId),
                    ProductId = productId
                };

                _context.Wishlist.Add(wishlist);
                _context.SaveChanges();
            }

            // Thông báo đã thêm vào wishlist và chuyển hướng
            return Json(new { success = true });
        }

    }
}
