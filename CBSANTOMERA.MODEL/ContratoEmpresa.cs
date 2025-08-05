using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class ContratoEmpresa
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public int? Contribucion { get; set; }

    public string Condiciones { get; set; } = null!;

    public int? Noticia { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaModificacion { get; set; }

    public int Empresa { get; set; }

    public int Temporada { get; set; }

    public string? Url { get; set; }

    public virtual Empresa EmpresaNavigation { get; set; } = null!;

    public virtual Noticia? NoticiaNavigation { get; set; }

    public virtual Temporada TemporadaNavigation { get; set; } = null!;
}
