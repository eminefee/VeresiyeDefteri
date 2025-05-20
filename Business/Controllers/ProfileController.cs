using Business.Configurations;
using Business.Context;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var user = _context.Userss.ToList();
            return View(user);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var user = _context.Userss.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            _context.Userss.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Userss.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
                return NotFound();

            var hasher = new PasswordHasher<User>();

            // Şifre doğrulaması Microsoft'un hasher'ı ile yapılmalı
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Eski şifre yanlış.");
                return View(model);
            }

            // Yeni şifreyi hashle
            user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi.";
            return RedirectToAction("Index");
        }


    }
}
