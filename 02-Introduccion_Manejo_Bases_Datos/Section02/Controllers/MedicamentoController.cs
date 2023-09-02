using System;
using Microsoft.AspNetCore.Mvc;
using Section02.Classes;
using Section02.Models;

namespace Section02.Controllers
{
    public class MedicamentoController : Controller
    {
        public IActionResult Index()
        {
            List<MedicamentoClass> medicamentosList = new List<MedicamentoClass>();

            using (BDHospitalContext db = new BDHospitalContext())
            {
                medicamentosList = (
                    from medicamento in db.Medicamentos
                    join formaFarmaceutica in db.FormaFarmaceuticas
                    on medicamento.Iidformafarmaceutica equals formaFarmaceutica.Iidformafarmaceutica
                    where medicamento.Bhabilitado == 1
                    select new MedicamentoClass
                    {
                        iidMedicamento = medicamento.Iidmedicamento,
                        nombre = medicamento.Nombre,
                        precio = medicamento.Precio,
                        stock = medicamento.Stock,
                        nombreFormaFarmaceutica = formaFarmaceutica.Nombre
                    }
                ).ToList();
            }

            return View(medicamentosList);
        }
    }
}