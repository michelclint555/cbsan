using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
