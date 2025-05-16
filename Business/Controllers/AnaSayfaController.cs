using Business.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    [Authorize]

    public class AnaSayfaController : Controller
    {
        private readonly AppDbContext _context;

        public AnaSayfaController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Veritabanından Amount alanlarının toplamını al
            float totalAmount = _context.debtrecords.Sum(d => d.Amount);
            int totalUser = _context.debtrecords.Count();
            int totalEmail = _context.SentEmails.Count();

            ViewBag.TotalEmail = totalEmail;
            ViewBag.TotalUser = totalUser;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }
    }
}
