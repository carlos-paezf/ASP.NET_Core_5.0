using System;
using System.ComponentModel.DataAnnotations;

namespace Section02.Classes
{
    public class PersonaClass
    {
        [Display(Name = "Id Persona")]
        public int iidPersona { get; set; }

        [Display(Name = "Nombre Completo")]
        public string nombreCompleto { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Sexo")]
        public string nombreSexo { get; set; }
    }
}