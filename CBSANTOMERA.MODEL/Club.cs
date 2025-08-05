using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Club
{
    public int IdClub { get; set; }

    public string? Foto { get; set; }

    public string? Nombre { get; set; }

    public string? Alias { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
