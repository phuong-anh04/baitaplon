using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace baitaplon.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly ILogger<ShopController> _logger;

        public ShopController(ILogger<ShopController> logger, EazydealsContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            var userId = User.FindFirst("AccountId")?.Value;
            // Lấy tất cả các sản phẩm trong giỏ hàng của người dùng hiện tại
            var cartItems = _context.Carts
                .Include(c => c.Product) // Kết nối với bảng Product để lấy thông tin sản phẩm
                .Where(c => c.Uid == int.Parse(userId))
                .ToList();
            decimal totalAmount = 0;
            ViewBag.CartItems = cartItems;
            System.Diagnostics.Debug.WriteLine(cartItems);
            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    decimal itemAmount = (item.Quantity ?? 0) * (decimal)item.Product.Price;// Calculate the amount for each item
                    totalAmount += itemAmount; // Add the item amount to the total amount
                }
            }
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.TotalAmount = totalAmount;
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            System.Diagnostics.Debug.WriteLine($"AddToCart chay vao day da: {productId}");
            // Lấy UserId của người dùng hiện tại
            var userId = User.FindFirst("AccountId")?.Value;

            // Kiểm tra nếu UserId null (người dùng chưa đăng nhập)
            if (string.IsNullOrEmpty(userId))
            {
                // Redirect về trang đăng nhập nếu chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Parse UserId sang kiểu số nguyên
            var intUserId = int.Parse(userId);

            // Tìm sản phẩm trong giỏ hàng của người dùng
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.Uid == intUserId && c.Pid == productId);

            if (cartItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng lên 1
                cartItem.Quantity += 1;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm sản phẩm mới với số lượng là 1
                cartItem = new Cart
                {
                    Uid = intUserId,
                    Pid = productId,
                    Quantity = 1
                };

                _context.Carts.Add(cartItem);
            }

            // Lưu thay đổi vào database
            _context.SaveChanges();

            // Redirect về trang giỏ hàng hoặc một trang khác
            // Tính tổng giỏ hàng
            var totalItemsCount = _context.Carts
                    .Where(c => c.Uid == intUserId)
                    .Sum(c => c.Quantity);

            
            decimal totalAmount = 0;
            var cartItems = _context.Carts
                .Include(c => c.Product) // Kết nối với bảng Product để lấy thông tin sản phẩm
                .Where(c => c.Uid == int.Parse(userId))
                .ToList();
            foreach (var item in cartItems)
            {
                System.Diagnostics.Debug.WriteLine($"ProductId: {item.Pid}, Quantity: {item.Quantity}, UserId: {item.Uid}");
                if (item.Product != null)
                {
                    decimal itemAmount = (item.Quantity ?? 0) * (decimal)item.Product.Price;// Calculate the amount for each item
                    totalAmount += itemAmount; // Add the item amount to the total amount
                }
            }

            return Json(new { success = true, totalAmount, totalItemsCount });
        }


        public IActionResult RemoveFromCart(int productId)
        {
            // Lấy UserId của người dùng hiện tại
            var userId = User.FindFirst("AccountId")?.Value;

            // Kiểm tra nếu UserId null (người dùng chưa đăng nhập)
            if (string.IsNullOrEmpty(userId))
            {
                // Redirect về trang đăng nhập nếu chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Parse UserId sang kiểu số nguyên
            var intUserId = int.Parse(userId);

            // Tìm sản phẩm trong giỏ hàng của người dùng
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.Uid == intUserId && c.Pid == productId);

            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    // Giảm số lượng sản phẩm trong giỏ hàng
                    cartItem.Quantity -= 1;
                }
                else
                {
                    // Xóa sản phẩm khỏi giỏ hàng nếu số lượng là 1
                    _context.Carts.Remove(cartItem);
                }

                // Lưu thay đổi vào database
                _context.SaveChanges();
            }
            // Tính tổng giỏ hàng
            var totalItemsCount = _context.Carts
                    .Where(c => c.Uid == intUserId)
                    .Sum(c => c.Quantity);
            decimal totalAmount = 0;
            var cartItems = _context.Carts
                .Include(c => c.Product) // Kết nối với bảng Product để lấy thông tin sản phẩm
                .Where(c => c.Uid == int.Parse(userId))
                .ToList();
            foreach (var item in cartItems)
            {
                System.Diagnostics.Debug.WriteLine($"ProductId: {item.Pid}, Quantity: {item.Quantity}, UserId: {item.Uid}");
                if (item.Product != null)
                {
                    decimal itemAmount = (item.Quantity ?? 0) * (decimal)item.Product.Price;// Calculate the amount for each item
                    totalAmount += itemAmount; // Add the item amount to the total amount
                }
            }
            // Redirect về trang giỏ hàng hoặc một trang khác
            return Json(new { success = true , totalAmount, totalItemsCount });
        }

        // tạo một action để xóa sản phẩm khỏi giỏ hàng
        public IActionResult DeleteFromCart(int productId)
        {
            // Lấy UserId của người dùng hiện tại
            var userId = User.FindFirst("AccountId")?.Value;

            // Kiểm tra nếu UserId null (người dùng chưa đăng nhập)
            if (string.IsNullOrEmpty(userId))
            {
                // Redirect về trang đăng nhập nếu chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Parse UserId sang kiểu số nguyên
            var intUserId = int.Parse(userId);

            // Tìm sản phẩm trong giỏ hàng của người dùng
            var cartItem = _context.Carts
                .FirstOrDefault(c => c.Uid == intUserId && c.Pid == productId);

            if (cartItem != null)
            {
                // Xóa sản phẩm khỏi giỏ hàng
                _context.Carts.Remove(cartItem);

                // Lưu thay đổi vào database
                _context.SaveChanges();
            }

            // Tính tổng giỏ hàng
            var totalItemsCount = _context.Carts
                    .Where(c => c.Uid == intUserId)
                    .Sum(c => c.Quantity);
            decimal totalAmount = 0;
            var cartItems = _context.Carts
                .Include(c => c.Product) // Kết nối với bảng Product để lấy thông tin sản phẩm
                .Where(c => c.Uid == int.Parse(userId))
                .ToList();
            foreach (var item in cartItems)
            {
                System.Diagnostics.Debug.WriteLine($"ProductId: {item.Pid}, Quantity: {item.Quantity}, UserId: {item.Uid}");
                if (item.Product != null)
                {
                    decimal itemAmount = (item.Quantity ?? 0) * (decimal)item.Product.Price;// Calculate the amount for each item
                    totalAmount += itemAmount; // Add the item amount to the total amount
                }
            }
            // Redirect về trang giỏ hàng hoặc một trang khác
            return Json(new { success = true, totalAmount, totalItemsCount });
        }
    }
}
