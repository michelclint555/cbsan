using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class DetalleVentaDTO
    {
        public int Id { get; set; }

        public int? IdVenta { get; set; }

        public int? IdProducto { get; set; }
        public string? DescripcionProductos { get; set; }

        public int? Cantidad { get; set; }

        public string? PrecioTexto { get; set; }

        public string? TotalTexto { get; set; }
    }
}
