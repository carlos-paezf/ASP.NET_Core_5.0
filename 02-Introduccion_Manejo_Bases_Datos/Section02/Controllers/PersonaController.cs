using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class PersonaController : Controller
    {
        public IActionResult Index()
        {
            List<PersonaClass> personasList = new List<PersonaClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                personasList = (
                    from persona in db.Personas
                    join sexo in db.Sexos
                    on persona.Iidsexo equals sexo.Iidsexo
                    where persona.Bhabilitado == 1
                    select new PersonaClass
                    {
                        iidPersona = persona.Iidpersona,
                        nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
                        email = persona.Email,
                        nombreSexo = sexo.Nombre
                    }
                ).ToList();
            }

            return View(personasList);
        }
    }
}