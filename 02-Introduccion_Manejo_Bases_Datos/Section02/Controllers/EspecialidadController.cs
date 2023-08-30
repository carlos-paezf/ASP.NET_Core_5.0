using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index()
        {
            List<EspecialidadClass> especialidadList = new List<EspecialidadClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                especialidadList = (
                    from especialidad in db.Especialidades
                    where especialidad.Bhabilitado == 1
                    select new EspecialidadClass
                    {
                        iidespecialidad = especialidad.Iidespecialidad,
                        nombre = especialidad.Nombre,
                        descripcion = especialidad.Descripcion
                    }
                ).ToList();
            }

            return View(especialidadList);
        }
    }
}