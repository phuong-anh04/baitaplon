using AspNetCoreHero.ToastNotification.Abstractions;
using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]

    [Authorize]
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly INotyfService _toastNotification;

        public HomeController(EazydealsContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
