using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class TecnicoEquipo
{
    public int Id { get; set; }

    public int? IdEquipo { get; set; }

    public int? IdTecnico { get; set; }

    public string? Funcion { get; set; }

    public int? Temporada { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? EsActivo { get; set; }

    public virtual ICollection<FotoTecnico> FotoTecnicos { get; set; } = new List<FotoTecnico>();

    public virtual Equipo? IdEquipoNavigation { get; set; }

    public virtual Tecnico? IdTecnicoNavigation { get; set; }

    public virtual Temporada? TemporadaNavigation { get; set; }
}
