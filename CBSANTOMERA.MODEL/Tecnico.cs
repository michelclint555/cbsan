using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Tecnico
{
    public int Id { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public DateOnly? FechaNac { get; set; }

    public string? Foto { get; set; }

    public string? ThumbnailImageSrc { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? EsActivo { get; set; }

    public int Temporada { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<FotoTecnico> FotoTecnicos { get; set; } = new List<FotoTecnico>();

    public virtual ICollection<TecnicoEquipo> TecnicoEquipos { get; set; } = new List<TecnicoEquipo>();

    public virtual Temporada TemporadaNavigation { get; set; } = null!;
}
