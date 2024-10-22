using System.Diagnostics;
using baitaplon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace baitaplon.Controllers
{
    public class HomeController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, EazydealsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList(); // Ho?c cách truy v?n phù h?p
            var categories = _context.Categories.Include(c => c.Products).ToList(); // L?y danh sách category
            var products = _context.Products.ToList(); // L?y danh sách product
            System.Diagnostics.Debug.WriteLine("chay vao day" + User.Identity.IsAuthenticated);
            var cheapProducts = _context.Products
                    .OrderBy(p => p.Price)  // S?p x?p theo giá t?ng d?n
                    .Take(6)
                    .ToList();

            ViewBag.HotProducts = _context.Products
                   .Where(p => p.Quantity == 3)  // Ch? l?y s?n ph?m có t?n kho b?ng 3
                   .Take(6)                      // L?y t?i ?a 6 s?n ph?m
                   .ToList();


            ViewBag.CheapProducts = cheapProducts;
            ViewBag.Categories = categories; // Truy?n danh sách category vào ViewBag
            ViewBag.Products = products;     // Truy?n danh sách product vào ViewBag

            return View(products);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Detail(int Pid)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(m => m.Pid == Pid);

            if (product == null)
            {
                Console.WriteLine("Product not found for Pid: " + Pid);
                return NotFound();
            }
            else
            {
                Console.WriteLine("Product found: " + product.Name);
            }

            return View(product);
        }



    }
}
