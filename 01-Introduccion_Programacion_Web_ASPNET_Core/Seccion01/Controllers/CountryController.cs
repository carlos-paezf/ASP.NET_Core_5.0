using Microsoft.AspNetCore.Mvc;

namespace Seccion01.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string Country()
        {
            return "Colombia";
        }

        public char InitialLetter()
        {
            return 'C';
        }

        public double CountrySurfaceKM()
        {
            return 1.142;
        }

        public bool HasFourSeasons()
        {
            return false;
        }

        public string SayWelcome(string name, string country = "Colombia")
        {
            return "Hi " + name + ", welcome to " + country;
        }
    }
}