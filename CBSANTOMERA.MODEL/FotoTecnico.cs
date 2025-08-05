using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FotoTecnico
{
    public string? ThumbnailImageSrc { get; set; }

    public int Id { get; set; }

    public int? IdTecnico { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int IdEquipo { get; set; }

    public int Tecnico { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? Temporada { get; set; }

    public virtual Equipo IdEquipoNavigation { get; set; } = null!;

    public virtual TecnicoEquipo? IdTecnicoNavigation { get; set; }

    public virtual Tecnico TecnicoNavigation { get; set; } = null!;

    public virtual Temporada? TemporadaNavigation { get; set; }
}
