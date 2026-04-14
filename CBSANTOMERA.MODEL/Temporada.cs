using System;
using System.Collections.Generic;

namespace CBSANTOMERA.MODEL;

public partial class Temporada
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly Inicio { get; set; }

    public DateOnly Fin { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? Source { get; set; }

    public bool? Visible { get; set; }

    public virtual ICollection<Album> Albumes { get; set; } = new List<Album>();

    public virtual ICollection<Competicione> Competiciones { get; set; } = new List<Competicione>();

    public virtual ICollection<ContratoEmpresa> ContratoEmpresas { get; set; } = new List<ContratoEmpresa>();

    public virtual ICollection<EquipoJugador> EquipoJugadors { get; set; } = new List<EquipoJugador>();

    public virtual ICollection<FotoEquipo> FotoEquipos { get; set; } = new List<FotoEquipo>();

    public virtual ICollection<FotoJugadorEquipo> FotoJugadorEquipos { get; set; } = new List<FotoJugadorEquipo>();

    public virtual ICollection<FotoJugador> FotoJugadors { get; set; } = new List<FotoJugador>();

    public virtual ICollection<FotoTecnico> FotoTecnicos { get; set; } = new List<FotoTecnico>();

    public virtual ICollection<FotosAlbum> FotosAlbums { get; set; } = new List<FotosAlbum>();

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();

    public virtual ICollection<TecnicoEquipo> TecnicoEquipos { get; set; } = new List<TecnicoEquipo>();

    public virtual ICollection<Tecnico> Tecnicos { get; set; } = new List<Tecnico>();
}
