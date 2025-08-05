using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Jugador
{
    public int IdJugador { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public DateOnly? FechaNac { get; set; }

    public string? Sexo { get; set; }

    public string? Dnie { get; set; }

    public string? NombreTutor { get; set; }

    public string? Telefono { get; set; }

    public string? Foto { get; set; }

    public string? TelefonoTutor { get; set; }

    public string? CorreoTutor { get; set; }

    public string? Direccion { get; set; }

    public string? Localidad { get; set; }

    public string? Provincia { get; set; }

    public string? Cp { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? ThumbnailImageSrc { get; set; }

    public int? Temporada { get; set; }

    public virtual ICollection<FotoJugadorEquipo> FotoJugadorEquipos { get; set; } = new List<FotoJugadorEquipo>();

    public virtual ICollection<FotoJugador> FotoJugadors { get; set; } = new List<FotoJugador>();

    public virtual Temporada? TemporadaNavigation { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
