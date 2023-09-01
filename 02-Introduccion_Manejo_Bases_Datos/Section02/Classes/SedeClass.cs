using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class SedeClass
    {
        [Display(Name = "Id Sede")]
        public int iidSede { get; set; }

        [Display(Name = "Nombre de la Sede")]
        public string nombreSede { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }
    }
}