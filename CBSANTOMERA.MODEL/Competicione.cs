using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Competicione
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Idtipo { get; set; }

    public int? NumEquipos { get; set; }

    public string? Logo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? Temporada { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? Categoria { get; set; }

    public string? Estado { get; set; }

    public string? Tipo { get; set; }

    public int? NumVuelta { get; set; }

    public virtual ICollection<FasesCompeticion> FasesCompeticions { get; set; } = new List<FasesCompeticion>();

    public virtual TipoCompeticion? IdtipoNavigation { get; set; }

    public virtual ICollection<Jornada> Jornada { get; set; } = new List<Jornada>();

    public virtual ICollection<Liga> Ligas { get; set; } = new List<Liga>();

    public virtual Temporada? TemporadaNavigation { get; set; }
}
