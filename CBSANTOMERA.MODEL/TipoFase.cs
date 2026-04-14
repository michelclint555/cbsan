using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class TipoFase
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Competicione> Competiciones { get; set; } = new List<Competicione>();
}
