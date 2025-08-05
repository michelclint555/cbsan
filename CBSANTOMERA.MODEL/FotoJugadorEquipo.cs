using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FotoJugadorEquipo
{
    public int Id { get; set; }

    public string? Foto { get; set; }

    public string? UrlFoto { get; set; }

    public int IdEquipo { get; set; }

    public int Jugador { get; set; }

    public bool? Principal { get; set; }

    public bool? Secundaria { get; set; }

    public string? ThumbnailImageSrc { get; set; }

    public int? EquipoJugador { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? Temporada { get; set; }

    public virtual EquipoJugador? EquipoJugadorNavigation { get; set; }

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual Jugador JugadorNavigation { get; set; } = null!;

    public virtual Temporada? TemporadaNavigation { get; set; }
}
