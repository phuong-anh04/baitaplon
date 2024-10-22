using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace baitaplon.Controllers
{
    [Authorize]
    public class CheckController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly ILogger<CheckController> _logger;
        public CheckController(ILogger<CheckController> logger, EazydealsContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList(); // Hoặc cách truy vấn phù hợp

            // Các dữ liệu khác nếu có
            var userId = User.FindFirst("AccountId")?.Value;
            int intUserId = int.Parse(userId);

            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng
            var cartItems = _context.Carts.Where(c => c.Uid == intUserId)
                                          .Include(c => c.Product)
                                          .ToList();
            var cartViewItems = cartItems.Select(item => new
            {
                ProductName = item.Product.Name,
                Quantity = item.Quantity ?? 0,
                Price = item.Product.Price,
                TotalPrice = (item.Quantity ?? 0) * (decimal)item.Product.Price
            }).ToList();
            foreach (var item in cartViewItems)
            {
                System.Diagnostics.Debug.WriteLine($"ProductName: {item.ProductName}, Quantity: {item.Quantity}, Price: {item.Price}, TotalPrice: {item.TotalPrice}");
            }
            // Tính tổng tiền giỏ hàng
            var totalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);
            ViewBag.CartItems = cartViewItems;
            ViewBag.TotalAmount = totalAmount;
            return View();
        }
        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("chay vao day nhe");
            System.Diagnostics.Debug.WriteLine("First Name: " + model.FirstName);
            System.Diagnostics.Debug.WriteLine("Last Name: " + model.LastName);
            System.Diagnostics.Debug.WriteLine("Country: " + model.Country);
            System.Diagnostics.Debug.WriteLine("Address: " + model.Address);
            System.Diagnostics.Debug.WriteLine("Phone: " + model.Phone);
            System.Diagnostics.Debug.WriteLine("Email: " + model.Email);

            // tạo một orderId ngẫu nhiên với tiền tố Order_
            var orderId = "Order_" + Guid.NewGuid().ToString();
            var userId = User.FindFirst("AccountId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the error appropriately, e.g., return an error response
                return BadRequest("User ID is null or empty.");
            }
            // Tạo một order mới và lưu vào bảng orders
            var newOrder = new Order
            {
                Orderid = orderId,
                UserId = int.Parse(userId),
                PaymentType = model.PaymentType,
                Status = "Pending",  // Trạng thái đơn hàng
                Date = DateTime.Now,
                // thêm các thông tin cá nhân như firt nam, lastnam,..
                // Bổ sung các thông tin cá nhân
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email


            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            System.Diagnostics.Debug.WriteLine("longdzz");
            // Sau khi lưu order, lấy OrderId vừa tạo
            var userCartItems = _context.Carts
                .Include(c => c.Product) // Bao gồm thực thể Product
                .Where(c => c.Uid == int.Parse(userId))
                .ToList();
            
            // lấy orderid vừa tạo
            var order = _context.Orders.FirstOrDefault(o => o.Orderid == orderId);
            if (order == null)
            {
                return BadRequest("Order not found.");
            }
            //Lưu các sản phẩm vào bảng ordered_product
            foreach (var item in userCartItems)

            {
                if (item.Product == null)
                {
                    // Handle the null product case appropriately, e.g., log an error or skip the item
                    continue;
                }
                var orderedProduct = new OrderedProduct
                {
                    Orderid = order.Id,
                    Name = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Product.Price.ToString(),
                };

                _context.OrderedProducts.Add(orderedProduct);
            }

            _context.SaveChanges();

            // Xóa giỏ hàng của người dùng sau khi đặt hàng thành công
            

            _context.Carts.RemoveRange(userCartItems); // Xóa tất cả sản phẩm trong giỏ hàng
            _context.SaveChanges();

            // Chuyển hướng đến trang xác nhận đơn hàng hoặc trang lịch sử đơn hàng
            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }
    }


}
