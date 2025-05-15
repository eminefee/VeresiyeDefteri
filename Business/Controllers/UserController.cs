using System.Security.Claims;
using Business.Context;
using Business.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Business.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /User/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Kullanıcıyı veritabanından kontrol et
            var user = _context.Userss.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
                return View();
            }

            // Claims oluştur (kullanıcı bilgileri)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username)
        // Eğer rol eklemek istersen: new Claim(ClaimTypes.Role, "Admin")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Oturum başlat (çerezi oluştur)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Debt"); // Giriş başarılıysa yönlendirme
        }

        //POST: /User/Register
        [HttpPost]
        public IActionResult Register(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor!";
                return View();
            }

            var existingUser = _context.Userss.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu kullanıcı adı zaten mevcut!";
                return View();
            }

            var newUser = new User
            {
                Username = username,
                Password = password // Gerçek projede hash'lenmeli!
            };

            _context.Userss.Add(newUser);
            _context.SaveChanges();

            // Kayıt sonrası otomatik giriş:
            HttpContext.Session.SetString("Username", newUser.Username);
            return RedirectToAction("Index", "Debt");
        }

        // GET: /User/Logout
        public async Task<IActionResult> Logout()
        {
            // Cookie'yi temizler, oturumu kapatır
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}
