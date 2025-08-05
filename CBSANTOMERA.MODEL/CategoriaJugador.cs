using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class CategoriaJugador
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public bool? EsActivo { get; set; }

    public string? Sexo { get; set; }

    public int? PrimerAnio { get; set; }

    public int? SegundoAnio { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
