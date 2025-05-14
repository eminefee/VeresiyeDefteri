using Business.Context;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Business.Controllers
{
    public class DebtController : Controller
    {
        private readonly AppDbContext _context;

        public DebtController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var records = _context.debtrecords.ToList();
            return View(records);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var debtRecord = _context.debtrecords.FirstOrDefault(d => d.Id == id);

            if (debtRecord == null)
            {
                return NotFound(); // Kayıt bulunamazsa hata döndür
            }

            _context.debtrecords.Remove(debtRecord); // Kayıt silinir
            _context.SaveChanges(); // Değişiklikleri kaydet

            return RedirectToAction("Index"); // Listeleme sayfasına yönlendir
        }

        public IActionResult Edit(int id)
        {

            var debtRecord = _context.debtrecords.FirstOrDefault(d => d.Id == id);

            if (debtRecord == null)
            {
                return NotFound(); // Kayıt bulunamazsa hata döndür
            }

            return View(debtRecord); // Güncellenebilir veriyi view'a gönder
        }

        // Güncelleme işlemini gerçekleştiren POST aksiyonu
        [HttpPost]
        public IActionResult Edit(int id, DebtRecord model)
        {


            if (model.Date == default || model.Date.Kind != DateTimeKind.Utc)
            {
                model.Date = DateTime.UtcNow;
            }

            if (id != model.Id)
            {
                return BadRequest(); // İd uyuşmazsa hata döndür
            }

            var debtRecord = _context.debtrecords.FirstOrDefault(d => d.Id == id);

            if (debtRecord == null)
            {
                return NotFound(); // Kayıt bulunamazsa hata döndür
            }

            // Kayıt güncelleniyor
            debtRecord.CustomerName = model.CustomerName;
            debtRecord.Amount = model.Amount;
            debtRecord.Date = model.Date;
            debtRecord.Description = model.Description;

            _context.SaveChanges(); // Değişiklikleri kaydet

            return RedirectToAction("Index"); // Listeleme sayfasına yönlendir
        }

        [HttpPost]
        public IActionResult Add(DebtRecord model)
        {
            if (ModelState.IsValid)
            {
                if (model.Date == default || model.Date.Kind != DateTimeKind.Utc)
                {
                    model.Date = DateTime.UtcNow;
                }


                _context.debtrecords.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kayıt başarıyla eklendi!";
                return RedirectToAction("Add");
            }

            TempData["ErrorMessage"] = "Kayıt sırasında bir hata oluştu.";
            return View("Add");
        }

    }
}
