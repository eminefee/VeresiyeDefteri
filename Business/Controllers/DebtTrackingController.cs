using Business.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    [Authorize(Roles = "User,Admin")]
    //Çalışmıyor
    public class DebtTrackingController : Controller
    {
        private readonly AppDbContext _context;

        public DebtTrackingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Kullanıcıyı session veya claims'den al
            var username = User.Identity.Name;

            var user = _context.Userss.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login", "User");

            var products = _context.UserProducts
                            .Where(up => up.UserId == user.Id)
                            .Select(up => new
                            {
                                up.Product.Name,
                                up.Product.Price,
                                up.Quantity,
                                Total = up.Product.Price * up.Quantity
                            })
                            .ToList();

            var totalDebt = products.Sum(p => p.Total);

            ViewBag.TotalDebt = totalDebt;

            return View(products);
        }
    }
}
