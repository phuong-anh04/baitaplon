using Microsoft.AspNetCore.Mvc;

namespace baitaplon.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
