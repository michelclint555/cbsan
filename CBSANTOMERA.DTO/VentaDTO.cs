using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; }

        public string? NumeroDocumento { get; set; }

        public string? TipoPago { get; set; }

        public string? Total { get; set; }

        public string? FechaRegistro { get; set; }
        public virtual ICollection<DetalleVentaDTO> DetalleVenta { get; set; }
    }
}
