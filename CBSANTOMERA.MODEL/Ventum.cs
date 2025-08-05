using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Ventum
{
    public int Id { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? TipoPago { get; set; }

    public decimal? Total { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();
}
