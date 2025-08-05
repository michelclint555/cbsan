using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Empresa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Logo { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<ContratoEmpresa> ContratoEmpresas { get; set; } = new List<ContratoEmpresa>();
}
