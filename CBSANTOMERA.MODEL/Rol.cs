using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Rol
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<MenuRol> MenuRols { get; set; } = new List<MenuRol>();
}
