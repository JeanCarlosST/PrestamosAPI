using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrestamosAPI.Models
{
    public class Moras
    {
        [Key]
        public int MoraID { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public float Total { get; set; }

        [ForeignKey("MoraID")]
        public virtual List<MorasDetalle> Detalle { get; set; } = new List<MorasDetalle>();
    }

    public class MorasDetalle
    {
        [Key]
        public int MoraDetalleID { get; set; }

        public int MoraID { get; set; }

        [Required(ErrorMessage = "Seleccione un préstamo")]
        public int PrestamoID { get; set; }

        [Required(ErrorMessage = "Ingrese un valor")]
        public float Valor { get; set; }
    }
}
