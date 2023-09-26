using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Section03.Classes;
using Section03.Models;

namespace Section03.Controllers
{
    public class PersonaController : Controller
    {
        public void FillSexComboBox()
        {
            List<SelectListItem> sexList = new List<SelectListItem>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                sexList = (
                    from sexo in db.Sexos
                    where sexo.Bhabilitado == 1
                    select new SelectListItem
                    {
                        Text = sexo.Nombre,
                        Value = sexo.Iidsexo.ToString()
                    }
                ).ToList();

                sexList.Insert(
                    0,
                    new SelectListItem
                    {
                        Text = "--Seleccione--",
                        Value = ""
                    }
                );
            }

            ViewBag.SexList = sexList;
        }

        public IActionResult Index()
        {
            List<PersonaClass> personaList = new List<PersonaClass>();

            FillSexComboBox();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                personaList = (
                    from persona in db.Personas
                    join sexo in db.Sexos
                    on persona.Iidsexo equals sexo.Iidsexo
                    where persona.Bhabilitado == 1
                    select new PersonaClass
                    {
                        IidPersona = persona.Iidpersona,
                        NombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
                        Email = persona.Email,
                        NombreSexo = sexo.Nombre
                    }
                ).ToList();
            }

            return View(personaList);
        }
    }
}