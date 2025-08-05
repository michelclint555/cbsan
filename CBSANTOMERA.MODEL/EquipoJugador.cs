using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class EquipoJugador
{
    public int Id { get; set; }

    public int IdEquipo { get; set; }

    public int IdJugador { get; set; }

    public int? Dorsal { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public int? Temporada { get; set; }

    public virtual ICollection<FotoJugadorEquipo> FotoJugadorEquipos { get; set; } = new List<FotoJugadorEquipo>();

    public virtual Temporada? TemporadaNavigation { get; set; }
}
