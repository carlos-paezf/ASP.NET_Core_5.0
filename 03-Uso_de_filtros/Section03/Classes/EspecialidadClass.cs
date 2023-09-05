using System;
using System.ComponentModel.DataAnnotations;

namespace Section03.Classes
{
    public class EspecialidadClass
    {
        [Display(Name = "Id Especialidad")]
        public int Iidespecialidad { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
    }
}