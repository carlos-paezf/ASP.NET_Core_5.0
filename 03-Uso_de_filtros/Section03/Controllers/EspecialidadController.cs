using Microsoft.AspNetCore.Mvc;
using Section03.Classes;
using Section03.Models;

namespace Section03.Controllers
{
    public class EspecialidadController : Controller
    {
        public IActionResult Index(EspecialidadClass objEspecialidad)
        {
            List<EspecialidadClass> especialidadList = new List<EspecialidadClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                if (objEspecialidad.Nombre == null || objEspecialidad.Nombre == "")
                {
                    especialidadList = (
                        from especialidad in db.Especialidades
                        where especialidad.Bhabilitado == 1
                        select new EspecialidadClass
                        {
                            Iidespecialidad = especialidad.Iidespecialidad,
                            Nombre = especialidad.Nombre,
                            Descripcion = especialidad.Descripcion
                        }
                    ).ToList();

                    ViewBag.NombreEspecialidad = "";
                }
                else
                {
                    especialidadList = (
                        from especialidad in db.Especialidades
                        where especialidad.Bhabilitado == 1 && especialidad.Nombre.Contains(objEspecialidad.Nombre)
                        select new EspecialidadClass
                        {
                            Iidespecialidad = especialidad.Iidespecialidad,
                            Nombre = especialidad.Nombre,
                            Descripcion = especialidad.Descripcion
                        }
                    ).ToList();

                    ViewBag.NombreEspecialidad = objEspecialidad.Nombre;
                }
            }

            return View(especialidadList);
        }
    }
}