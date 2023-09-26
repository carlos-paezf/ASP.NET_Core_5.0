using System;
using System.ComponentModel.DataAnnotations;

namespace Section03.Classes
{
    public class PersonaClass
    {
        [Display(Name = "Id Persona")]
        public int IidPersona { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Sexo")]
        public string NombreSexo { get; set; }

        [Display(Name = "Id Sexo")]
        public int Iidsexo { get; set; }
    }
}