using Microsoft.AspNetCore.Mvc;

namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}