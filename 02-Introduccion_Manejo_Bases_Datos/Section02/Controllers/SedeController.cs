using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class SedeController : Controller
    {
        public IActionResult Index()
        {
            List<SedeClass> sedesList = new List<SedeClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                sedesList = (
                    from sede in db.Sedes
                    where sede.Bhabilitado == 1
                    select new SedeClass
                    {
                        iidSede = sede.Iidsede,
                        nombreSede = sede.Nombre,
                        direccion = sede.Direccion
                    }
                ).ToList();
            }

            return View(sedesList);
        }
    }
}