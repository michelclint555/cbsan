using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class FotosAlbum
{
    public int IdFoto { get; set; }

    public string Imagen { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime Fecha { get; set; }

    public int IdAlbum { get; set; }

    public string ThumbnailImageSrc { get; set; } = null!;

    public int? Portada { get; set; }

    public int? Temporada { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Albume IdAlbumNavigation { get; set; } = null!;

    public virtual Temporada? TemporadaNavigation { get; set; }
}
