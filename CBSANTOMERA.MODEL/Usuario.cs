using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string? Correo { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public string? Clave { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? Token { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? Foto { get; set; }

    public bool? ContrasenaActivada { get; set; }

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();
}
