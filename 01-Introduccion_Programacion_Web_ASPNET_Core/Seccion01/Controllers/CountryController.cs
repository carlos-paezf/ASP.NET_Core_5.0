using Microsoft.AspNetCore.Mvc;
using Seccion01.Classes;

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

        public string SayWelcome(AmbassadorClass ambassador)
        {
            return "Hi " + ambassador.name + ", welcome to " + ambassador.country;
        }
    }
}