using Microsoft.AspNetCore.Mvc;
using Section01.Classes;

namespace Section01.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListCountries()
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
            return "Hi " + ambassador.Name + ", welcome to " + ambassador.Country;
        }

        public AmbassadorClass MeetAmbassador()
        {
            return new AmbassadorClass
            {
                Name = "David Ferrer",
                Country = "República de Colombia"
            };
        }

        public List<AmbassadorClass> ListAmbassadors()
        {
            return new List<AmbassadorClass>
            {
                new AmbassadorClass
                {
                    Name = "David Ferrer",
                    Country = "República de Colombia"
                },
                new AmbassadorClass
                {
                    Name = "Jackie Chan",
                    Country = "China"
                }
            };
        }
    }
}