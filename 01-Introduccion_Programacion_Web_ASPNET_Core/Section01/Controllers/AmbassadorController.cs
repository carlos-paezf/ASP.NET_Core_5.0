using Microsoft.AspNetCore.Mvc;
using Section01.Classes;

namespace Section01.Controllers
{
    public class AmbassadorController : Controller
    {
        public IActionResult Index()
        {
            List<AmbassadorClass> ambassadors = new()
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

            return View(ambassadors);
        }
    }
}