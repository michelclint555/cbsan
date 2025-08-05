using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Contacto
{
    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Correo { get; set; }
}
