using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Partido
{
    public int Id { get; set; }

    public int Local { get; set; }

    public int Visitante { get; set; }

    public string Ubicacion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public int PuntosLocal { get; set; }

    public int PuntosVisitante { get; set; }

    public int? Jornada { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Jornadum? JornadaNavigation { get; set; }

    public virtual Equipo LocalNavigation { get; set; } = null!;

    public virtual Equipo VisitanteNavigation { get; set; } = null!;
}
