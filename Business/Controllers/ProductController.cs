using Business.Context;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Business.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece admin ürün eklesin
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        // GET: Yeni ürün formu
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Delete(int id)
        {
            var products = _context.Products.FirstOrDefault(x => x.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            _context.Products.Remove(products);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {

            var productUpdate = _context.Products.FirstOrDefault(d => d.Id == id);

            if (productUpdate == null)
            {
                return NotFound(); // Kayıt bulunamazsa hata döndür
            }

            return View(productUpdate); // Güncellenebilir veriyi view'a gönder
        }

        // POST: Ürün ekleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                model.ImageUrl = fileName; // Dosya adını direkt Product.ImageUrl'a atıyoruz
            }

            _context.Products.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, Product model)
        {


            if (id != model.Id)
            {
                return BadRequest(); // İd uyuşmazsa hata döndür
            }

            var productUpdate = _context.Products.FirstOrDefault(d => d.Id == id);

            if (productUpdate == null)
            {
                return NotFound(); // Kayıt bulunamazsa hata döndür
            }

            // Kayıt güncelleniyor
            productUpdate.Name = model.Name;
            productUpdate.Price = model.Price;
            if (productUpdate.ImageUrl != null)
            {
                productUpdate.ImageUrl = model.ImageUrl;
            }
            else if (string.IsNullOrEmpty(productUpdate.ImageUrl))
            {
                // Daha önce hiç görsel yoksa, default ata
                productUpdate.ImageUrl = "default.jpg";
            }
            _context.SaveChanges(); // Değişiklikleri kaydet

            return RedirectToAction("Index"); // Listeleme sayfasına yönlendir
        }

    }
}
