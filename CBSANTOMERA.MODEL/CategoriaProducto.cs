using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class CategoriaProducto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
