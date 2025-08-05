using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Jornada
{
    public int Id { get; set; }

    public int Local { get; set; }

    public int Visitante { get; set; }

    public string Ubicacion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public int PuntosLocal { get; set; }

    public int PuntosVisitante { get; set; }

    public int Competicion { get; set; }

    public int Jornada1 { get; set; }

    public int? Temporada { get; set; }

    public int? Fase { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Competicione CompeticionNavigation { get; set; } = null!;

    public virtual FasesCompeticion? FaseNavigation { get; set; }

    public virtual Equipo LocalNavigation { get; set; } = null!;

    public virtual Equipo VisitanteNavigation { get; set; } = null!;
}
