using System;
using baitaplon.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace baitaplon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly EazydealsContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public AccountController(EazydealsContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(string? name, int page =1)
        {
            int pageSize = page > 1 ? page : 1;
            int limit = 2;
            ViewBag.names = name;
            var accounts = _context.Accounts.ToList();
            if (!string.IsNullOrEmpty(name))
            {
                accounts = _context.Accounts.Where(x => x.UserName.Contains(name)).OrderByDescending(x => x.UserName).ToList();
            }
            var pagedData = accounts.ToPagedList(pageSize, limit);
            ViewBag.Ac = accounts;
            return View(pagedData);
        }
        public IActionResult Add()
        {
            var accounts = _context.Accounts.ToList();
            ViewBag.accounts = accounts;
            return View();
        }

       
        [HttpPost]
        public IActionResult Add(IFormFile fileUpload, Account account)
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

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }
                account.Image = fileUpload.FileName;
            }
            else
            {
                ModelState.AddModelError("AccountImage", "Image cannot be blank.");
            }
            // Lưu vào database
            _context.Accounts.Add(account);
            _context.SaveChanges();


            // Chuyển hướng sau khi thêm  thành công
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int Id)
        {
            var accounts = _context.Accounts.ToList();
            ViewBag.accounts = accounts;

            // 
            var ac = _context.Accounts.Find(Id);
            return View(ac);
        }
        [HttpPost]
        public IActionResult Edit(int? id, string? oldImage, IFormFile fileUpload, Account account)
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

                var path = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", fileUpload.FileName);

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var pathOldFile = Path.Combine(rootPath, "wwwroot", "Uploads", "accounts", oldImage);
                    System.IO.File.Delete(pathOldFile);

                }

                using (var file = System.IO.File.Create(path))
                {
                    fileUpload.CopyTo(file);
                }

                account.Image = fileUpload.FileName;

            }
            else
            {
                account.Image = oldImage;
            }
            if (fileUpload != null || oldImage != null)
            {
                try
                {
                    _context.Accounts.Update(account);
                    _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        public IActionResult Delete(int Id)
        {
            //Lấy ra bản ghi cần xóa
            Account obj = _context.Accounts.FirstOrDefault(c => c.Id == Id);
            if (obj != null)
            {
                //Xóa
                _context.Accounts.Remove(obj);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
    }
}
