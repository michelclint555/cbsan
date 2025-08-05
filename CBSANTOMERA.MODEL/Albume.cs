using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Albume
{
    public int IdAlbum { get; set; }

    public DateTime Fecha { get; set; }

    public string Nombre { get; set; } = null!;

    public string Portada { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int? Temporada { get; set; }

    public bool? EsActivo { get; set; }

    public bool? Fijar { get; set; }

    public virtual ICollection<FotosAlbum> FotosAlbums { get; set; } = new List<FotosAlbum>();

    public virtual ICollection<Noticia> Noticia { get; set; } = new List<Noticia>();

    public virtual Temporada? TemporadaNavigation { get; set; }
}
