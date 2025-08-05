using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Liga
{
    public int Id { get; set; }

    public int? Competicion { get; set; }

    public int? Puntos { get; set; }

    public int? PartidosJugados { get; set; }

    public int? PartidosGanados { get; set; }

    public int? PartidosJugadosLocal { get; set; }

    public int? PartidosPerdidos { get; set; }

    public int? PartidosJugadosVisitante { get; set; }

    public int? PartidosGanadosVisitante { get; set; }

    public int? PuntosTotalesVisitante { get; set; }

    public int? PuntosTotalesLocal { get; set; }

    public int? PartidosGanadosLocal { get; set; }

    public int? PatidosPerdidosVisitante { get; set; }

    public int? PartidosPerdidosLocal { get; set; }

    public int? Racha { get; set; }

    public int? DifetenciaPuntos { get; set; }

    public int? Equipo { get; set; }

    public int? Posicion { get; set; }

    public int? Fase { get; set; }

    public virtual Competicione? CompeticionNavigation { get; set; }

    public virtual Equipo? EquipoNavigation { get; set; }

    public virtual FasesCompeticion? FaseNavigation { get; set; }
}
