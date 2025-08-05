using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FotoJugador
{
    public int Id { get; set; }

    public int? IdJugador { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? EsActivo { get; set; }

    public bool? Principal { get; set; }

    public int? Temporada { get; set; }

    public string? ThumbnailImageSrc { get; set; }

    public virtual Jugador? IdJugadorNavigation { get; set; }

    public virtual Temporada? TemporadaNavigation { get; set; }
}
