using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrestamosAPI.Models
{
    public class Personas
    {
        [Key]
        public int PersonaID { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Ingrese un apellido")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Ingrese la cédula")]
        [RegularExpression(@"^\b\d{3}-\d{7}-\d$", ErrorMessage = "Ingrese una cédula válida")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Ingrese una dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Ingrese una fecha")]
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;

        public float Balance { get; set; }

        [ForeignKey("PersonaID")]
        public virtual List<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
    }
}
