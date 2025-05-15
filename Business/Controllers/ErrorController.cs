using Microsoft.AspNetCore.Mvc;

namespace Business.Controllers
{
    public class ErrorController : Controller
    {
        // 404, 403 vs. için hâlihazırda mevcutsa ekleme gerek yok
        public IActionResult Status(int code)
        {
            if (code == 404)
                return View("NotFound");
            if (code == 403)
                return View("Forbidden");

            return View("GenericError");
        }

        // 500 gibi sistem hataları için:
        public IActionResult Error500()
        {
            return View("Error500");
        }
    }
}
