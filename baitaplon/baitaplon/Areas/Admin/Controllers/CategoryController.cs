using AspNetCoreHero.ToastNotification.Abstractions;
using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly INotyfService _toastNotification;

        public CategoryController(EazydealsContext context, INotyfService notyfService)
        {
            _context = context;
            _toastNotification = notyfService;
        }
        public IActionResult Index(string? name, int page = 1)
        {
            int pageSize = page > 1 ? page : 1;
            int limit = 2;
            ViewBag.names = name;
            var categories = _context.Categories.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                categories =  _context.Categories.Where(x => x.Name.Contains(name)).OrderByDescending(x => x.Cid).ToList();
            }
            var pagedData = categories.ToPagedList(pageSize, limit);
            ViewBag.Cate = categories;
            return View(pagedData);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int Cid)
        {
            // Lấy ra bản ghi cần xóa
            Category obj = _context.Categories.FirstOrDefault(c => c.Cid == Cid);

            if (obj == null)
            {
                // Nếu không tìm thấy danh mục, trả về trang lỗi hoặc thông báo
                return NotFound();
            }

            // Kiểm tra xem danh mục có sản phẩm nào hay không
            bool hasProducts = _context.Products.Any(p => p.Cid == Cid);

            if (hasProducts)
            {
                // Thêm thông báo lỗi nếu có sản phẩm trong danh mục
                TempData["ErrorMessage"] = "This category cannot be deleted because there are already products in the category.";
                return RedirectToAction("Index"); // Trả về trang danh sách với thông báo lỗi
            }

            // Nếu không có sản phẩm, tiến hành xóa
            _context.Categories.Remove(obj);
            _context.SaveChanges();

            // Trả về trang danh sách sau khi xóa thành công
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int Cid) {
            //Lấy bản ghi cần sửa
            Category obj = _context.Categories.FirstOrDefault(c => c.Cid == Cid);
            //Đổ ra form
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(Category data)
        {
            //Lấy lại bản ghi cần sửa
            Category obj = _context.Categories.FirstOrDefault(c => c.Cid.Equals(data.Cid));
            //Gán giá trị mới sửa vào các cột trong table
            obj.Name = data.Name;
            obj.Status = data.Status;
            //lưu lại
            _context.SaveChanges();
            //Load lại dữ liệu
            return RedirectToAction("Index");
        }
    }
}
