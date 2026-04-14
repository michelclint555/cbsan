using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Jornada
{
    public int? Id { get; set; }

    public int? Competicion { get; set; }

    public int? Fase { get; set; }

    public int? Numero { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
