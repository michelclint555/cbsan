using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class TipoCompeticion
{
    public int Id { get; set; }

    public string? Tipo { get; set; }

    public string? Subtipo { get; set; }

    public virtual ICollection<Competicione> Competiciones { get; set; } = new List<Competicione>();
}
