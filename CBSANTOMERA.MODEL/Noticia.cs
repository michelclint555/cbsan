using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Noticia
{
    public string? Subtitulo { get; set; }

    public int IdNoticia { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public string Portada { get; set; } = null!;

    public string TipoNoticia { get; set; } = null!;

    public int? Album { get; set; }

    public DateTime Fecha { get; set; }

    public string ThumbnailImageSrc { get; set; } = null!;

    public DateTime? FechaModificacion { get; set; }

    public bool? EsActivo { get; set; }

    public bool? Fijar { get; set; }

    public bool? Nuevo { get; set; }

    public virtual Albume? AlbumNavigation { get; set; }

    public virtual ICollection<ContratoEmpresa> ContratoEmpresas { get; set; } = new List<ContratoEmpresa>();
}
