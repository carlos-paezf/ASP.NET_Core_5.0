using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class PaginaClass
    {
        [Display(Name = "Id Página")]
        public int iidPagina { get; set; }

        [Display(Name = "Mensaje")]
        public string mensaje { get; set; }

        [Display(Name = "Acción")]
        public string accion { get; set; }

        [Display(Name = "Controlador")]
        public string controlador { get; set; }
    }
}