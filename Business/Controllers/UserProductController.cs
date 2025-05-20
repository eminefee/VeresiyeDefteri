using Business.Context;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    [Authorize(Roles = "Admin")]
    //Çalışmıyor

    public class UserProductController : Controller
    {

        private readonly AppDbContext _context;

        public UserProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Users = _context.Userss.ToList();
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Miktar 1 veya daha büyük olmalı.");
                ViewBag.Users = _context.Userss.ToList();
                ViewBag.Products = _context.Products.ToList();
                return View();
            }

            var existing = _context.UserProducts.FirstOrDefault(up => up.UserId == userId && up.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var userProduct = new UserProduct
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.UserProducts.Add(userProduct);
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Ürün kullanıcıya başarıyla atandı.";
            return RedirectToAction(nameof(Index));
        }
    }
}
