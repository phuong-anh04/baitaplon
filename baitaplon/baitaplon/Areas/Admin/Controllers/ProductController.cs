using System.Security.Cryptography;
using baitaplon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public ProductController(EazydealsContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(string? name, int page = 1)
        {
            var products = _context.Products
                          .Include(p => p.CidNavigation) // Nạp thông tin danh mục
                          .ToList();
            int pageSize = page > 1 ? page : 1;
            int limit = 5;
            ViewBag.names = name;
           
            if (!string.IsNullOrEmpty(name))
            {
                products = _context.Products.Where(x => x.Name.Contains(name)).OrderByDescending(x => x.Cid).ToList();
            }
            var pagedData = products.ToPagedList(pageSize, limit);
            ViewBag.Pro = products;
            return View(pagedData);
        }
        public IActionResult Add()
        {
            var categories = _context.Categories.ToList();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        public IActionResult Add(IFormFile fileUpload, Product product)
        {
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("BlogImage", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "products", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                product.Image = fileUpload.FileName;
            }
            else
            {
                ModelState.AddModelError("ProductImage", "Hình ảnh không được để trống.");
            }
            // Lưu sản phẩm vào database
            _context.Products.Add(product);
            _context.SaveChanges();

            // Lấy danh sách các danh mục từ cơ sở dữ liệu
            var categories = _context.Categories.ToList();

            // Tạo SelectList cho ViewBag.Categories
            ViewBag.Categories = new SelectList(categories, "Cid", "Name");

            // Chuyển hướng sau khi thêm sản phẩm thành công
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int Pid)
        {
            var categories = _context.Categories.ToList();
            ViewBag.categories = categories;

            // 
            var pro = _context.Products.Find(Pid);
            return View(pro); 
        }

        [HttpPost]
        public IActionResult Edit(int? id, string? oldImage, IFormFile fileUpload, Product product)
        {
            if (fileUpload != null)
            {
                var rootPath = _environment.ContentRootPath;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProductImage", "Hình ảnh không hợp lệ. Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif.");
                }

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "products", fileUpload.FileName);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "products", oldImage);
                    System.IO.File.Delete(pathOldFile);

                }

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }

                product.Image = fileUpload.FileName;

            }
            else
            {
                product.Image = oldImage;
            }
            
                try
                {
                    _context.Products.Update(product);
                    _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int Pid)
        {
            //Lấy ra bản ghi cần xóa
            Product obj = _context.Products.FirstOrDefault(c => c.Pid == Pid);
            if (obj != null)
            {
                //Xóa
                _context.Products.Remove(obj);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int Pid)
        {
            if (Pid == null)
            {
                return NotFound();
            }

            // Sử dụng Include để tải bảng Category liên quan
            var product = await _context.Products
                            .Include(p => p.CidNavigation)  // Tải thông tin Category liên quan
                            .FirstOrDefaultAsync(m => m.Pid == Pid);



            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


    }
}
