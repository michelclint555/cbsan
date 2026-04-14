using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Session
{
    public int IdSesion { get; set; }

    public int? IdUsuario { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    public string? Token { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? Usado { get; set; }
}
