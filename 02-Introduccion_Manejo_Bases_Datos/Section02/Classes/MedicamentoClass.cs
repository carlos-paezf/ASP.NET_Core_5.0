using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class MedicamentoClass
    {
        [Display(Name = "Id Medicamento")]
        public int iidMedicamento { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Display(Name = "Precio")]
        public decimal? precio { get; set; }

        [Display(Name = "Stock")]
        public int? stock { get; set; }

        [Display(Name = "Nombre Forma Farmacéutica")]
        public string nombreFormaFarmaceutica { get; set; }
    }
}