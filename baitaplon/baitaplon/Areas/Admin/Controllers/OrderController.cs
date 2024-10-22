using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using static NuGet.Packaging.PackagingConstants;
namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly EazydealsContext _context;

        public OrderController(EazydealsContext context)
        {
            _context = context;
        }
        public IActionResult Index(string? name, int page = 1)
        {
            int pageSize = page > 1 ? page : 1;
            int limit = 2;
            ViewBag.names = name;
            var orders = _context.Orders.Include(x => x.User).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                orders = _context.Orders.Where(x => x.Orderid.Contains(name)).OrderByDescending(x => x.Orderid).ToList();
            }
            var pagedData = orders.ToPagedList(pageSize, limit);
            ViewBag.Or = orders;
            return View(pagedData);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            //Lấy ra bản ghi cần xóa
            Order obj = _context.Orders.FirstOrDefault(c => c.Id == Id);
            if (obj != null)
            {
                //Xóa
                _context.Orders.Remove(obj);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int Id)
        {
            // 
            var or = _context.Orders.Find(Id);
            return View(or);
        }
        [HttpPost]
        public IActionResult Edit(Order data)
        {
            //Lấy lại bản ghi cần sửa
            Order obj = _context.Orders.FirstOrDefault(c => c.Id.Equals(data.Id));
            //Gán giá trị mới sửa vào các cột trong table
            obj.Status = data.Status;
            //lưu lại
            _context.SaveChanges();
            //Load lại dữ liệu
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int Id)
        {
            // 

            var or = _context.Orders.Include(x => x.OrderedProducts).FirstOrDefault(x => x.Id == Id);
            var ord = _context.OrderedProducts.Where(x => x.Orderid == or.Id).ToList();
            ViewBag.ord = ord;
            return View(or);
        }
    }
}
