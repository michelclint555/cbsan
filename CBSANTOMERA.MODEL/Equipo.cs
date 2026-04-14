using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Equipo
{
    public int IdEquipo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdClub { get; set; }

    public int IdCategoria { get; set; }

    public string? Foto { get; set; }

    public bool? Principal { get; set; }

    public int? Patrocinador { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? EsActivo { get; set; }

    public int? Temporada { get; set; }

    public int? FotoPortada { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? Pabellon { get; set; }

    public string? ThumbnailImageSrc { get; set; }

    public virtual ICollection<FotoEquipo> FotoEquipos { get; set; } = new List<FotoEquipo>();

    public virtual ICollection<FotoJugadorEquipo> FotoJugadorEquipos { get; set; } = new List<FotoJugadorEquipo>();

    public virtual FotoEquipo? FotoPortadaNavigation { get; set; }

    public virtual ICollection<FotoTecnico> FotoTecnicos { get; set; } = new List<FotoTecnico>();

    public virtual ICollection<Liga> Ligas { get; set; } = new List<Liga>();

    public virtual ICollection<Partido> PartidoLocalNavigations { get; set; } = new List<Partido>();

    public virtual ICollection<Partido> PartidoVisitanteNavigations { get; set; } = new List<Partido>();

    public virtual ICollection<TecnicoEquipo> TecnicoEquipos { get; set; } = new List<TecnicoEquipo>();

    public virtual ICollection<FasesCompeticion> IdFases { get; set; } = new List<FasesCompeticion>();
}
