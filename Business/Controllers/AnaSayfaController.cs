using Microsoft.AspNetCore.Mvc;

namespace Business.Controllers
{
    public class AnaSayfaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
