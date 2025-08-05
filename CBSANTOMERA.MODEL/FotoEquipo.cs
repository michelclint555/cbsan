using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FotoEquipo
{
    public string? ThumbnailImageSrc { get; set; }

    public int Id { get; set; }

    public int IdEquipo { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Foto { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? Temporada { get; set; }

    public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual Temporada? TemporadaNavigation { get; set; }
}
