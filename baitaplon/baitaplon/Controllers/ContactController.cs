using baitaplon.Models;
using Microsoft.AspNetCore.Mvc;

namespace baitaplon.Controllers
{
    public class ContactController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly ILogger<ContactController> _logger;
        public ContactController(ILogger<ContactController> logger, EazydealsContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList(); // Hoặc cách truy vấn phù hợp

            // Các dữ liệu khác nếu có
            return View();
        }
    }
}
