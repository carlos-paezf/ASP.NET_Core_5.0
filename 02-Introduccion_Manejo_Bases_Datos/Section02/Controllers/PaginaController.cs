using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class PaginaController : Controller
    {
        public IActionResult Index()
        {
            List<PaginaClass> paginasList = new List<PaginaClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                paginasList = (
                    from pagina in db.Paginas
                    where pagina.Bhabilitado == 1
                    select new PaginaClass
                    {
                        iidPagina = pagina.Iidpagina,
                        mensaje = pagina.Mensaje,
                        accion = pagina.Accion,
                        controlador = pagina.Controlador
                    }
                ).ToList();
            }

            return View(paginasList);
        }
    }
}