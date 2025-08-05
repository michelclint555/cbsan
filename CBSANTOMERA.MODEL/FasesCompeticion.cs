using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FasesCompeticion
{
    public int Id { get; set; }

    public int? Competicion { get; set; }

    public string? Nombre { get; set; }

    public int? NumEquipos { get; set; }

    public int? NumPartidos { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Competicione? CompeticionNavigation { get; set; }

    public virtual ICollection<Jornada> Jornada { get; set; } = new List<Jornada>();

    public virtual ICollection<Liga> Ligas { get; set; } = new List<Liga>();

    public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}
