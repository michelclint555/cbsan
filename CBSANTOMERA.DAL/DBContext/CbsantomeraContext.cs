using System;
using System.Collections.Generic;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;

namespace CBSANTOMERA.DAL.DBContext;

public partial class CbsantomeraContext : DbContext
{
    public CbsantomeraContext()
    {
    }

    public CbsantomeraContext(DbContextOptions<CbsantomeraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albumes { get; set; }

    public virtual DbSet<CategoriaJugador> CategoriaJugadors { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Competicione> Competiciones { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<ContratoEmpresa> ContratoEmpresas { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EquipoCompeticion> EquipoCompeticions { get; set; }

    public virtual DbSet<EquipoJugador> EquipoJugadors { get; set; }

    public virtual DbSet<FasesCompeticion> FasesCompeticions { get; set; }

    public virtual DbSet<FotoEquipo> FotoEquipos { get; set; }

    public virtual DbSet<FotoJugador> FotoJugadors { get; set; }

    public virtual DbSet<FotoJugadorEquipo> FotoJugadorEquipos { get; set; }

    public virtual DbSet<FotoTecnico> FotoTecnicos { get; set; }

    public virtual DbSet<FotosAlbum> FotosAlbums { get; set; }

    public virtual DbSet<Jornadum> Jornada { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    public virtual DbSet<Liga> Ligas { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuRol> MenuRols { get; set; }

    public virtual DbSet<Noticia> Noticias { get; set; }

    public virtual DbSet<NumeroDocumento> NumeroDocumentos { get; set; }

    public virtual DbSet<Partido> Partidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Tecnico> Tecnicos { get; set; }

    public virtual DbSet<TecnicoEquipo> TecnicoEquipos { get; set; }

    public virtual DbSet<Temporada> Temporadas { get; set; }

    public virtual DbSet<TipoFase> TipoFases { get; set; }

    public virtual DbSet<TipoNoticium> TipoNoticia { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.IdAlbum);

            entity.Property(e => e.IdAlbum).HasColumnName("idAlbum");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Fijar).HasColumnName("fijar");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Portada)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("portada");
            entity.Property(e => e.Temporada).HasColumnName("temporada");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.Albumes)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_Albumes_Temporadas");
        });

        modelBuilder.Entity<CategoriaJugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83F23D22F75");

            entity.ToTable("Categoria_jugador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrimerAnio).HasColumnName("primerAnio");
            entity.Property(e => e.SegundoAnio).HasColumnName("segundoAnio");
            entity.Property(e => e.Sexo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sexo");
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83F6F3E3D76");

            entity.ToTable("Categoria_producto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3213E83F6779CA29");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.IdClub).HasName("PK__Club__8205271FBBE51AE4");

            entity.ToTable("Club");

            entity.Property(e => e.IdClub).HasColumnName("idClub");
            entity.Property(e => e.Alias)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("alias");
            entity.Property(e => e.Correo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Foto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Competicione>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Idtipo).HasColumnName("idtipo");
            entity.Property(e => e.Logo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.NumEquipos).HasColumnName("Num_equipos");
            entity.Property(e => e.NumVuelta).HasColumnName("numVuelta");

            entity.HasOne(d => d.IdtipoNavigation).WithMany(p => p.Competiciones)
                .HasForeignKey(d => d.Idtipo)
                .HasConstraintName("FK_Competiciones_Tipo_Competicion");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.Competiciones)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_Competiciones_Temporadas");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Contacto");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Correo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContratoEmpresa>(entity =>
        {
            entity.ToTable("ContratoEmpresa");

            entity.Property(e => e.Condiciones)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.ContratoEmpresas)
                .HasForeignKey(d => d.Empresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContratoEmpresa_Empresas");

            entity.HasOne(d => d.NoticiaNavigation).WithMany(p => p.ContratoEmpresas)
                .HasForeignKey(d => d.Noticia)
                .HasConstraintName("FK_ContratoEmpresa_Noticias");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.ContratoEmpresas)
                .HasForeignKey(d => d.Temporada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContratoEmpresa_Temporadas");
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DetalleV__3213E83F52A2FE52");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdVenta).HasColumnName("idVenta");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__DetalleVe__idPro__440B1D61");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__DetalleVe__idVen__4316F928");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Logo)
                .HasMaxLength(300)
                .IsFixedLength()
                .HasColumnName("logo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(300)
                .IsFixedLength()
                .HasColumnName("nombre");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("tipo");
            entity.Property(e => e.Url)
                .HasMaxLength(300)
                .IsFixedLength()
                .HasColumnName("url");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("PK__Equipo__981ACF53C54BF589");

            entity.ToTable("Equipo");

            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Foto)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.FotoPortada).HasColumnName("fotoPortada");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdClub).HasColumnName("idClub");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Pabellon)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Patrocinador).HasColumnName("patrocinador");
            entity.Property(e => e.Principal).HasColumnName("principal");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("thumbnailImageSrc");

            entity.HasOne(d => d.FotoPortadaNavigation).WithMany(p => p.Equipos)
                .HasForeignKey(d => d.FotoPortada)
                .HasConstraintName("FK_Equipo_FotoEquipo");

            entity.HasMany(d => d.IdFases).WithMany(p => p.Equipos)
                .UsingEntity<Dictionary<string, object>>(
                    "EquiposFase",
                    r => r.HasOne<FasesCompeticion>().WithMany()
                        .HasForeignKey("IdFase")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Equipos_Fases_Fases_Competicion"),
                    l => l.HasOne<Equipo>().WithMany()
                        .HasForeignKey("Equipo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Equipos_Fases_Equipo"),
                    j =>
                    {
                        j.HasKey("Equipo", "IdFase");
                        j.ToTable("Equipos_Fases");
                    });
        });

        modelBuilder.Entity<EquipoCompeticion>(entity =>
        {
            entity.HasKey(e => new { e.Equipo, e.Competicion });

            entity.ToTable("Equipo_Competicion");
        });

        modelBuilder.Entity<EquipoJugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EquipoJu__3213E83FF4C547B0");

            entity.ToTable("EquipoJugador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dorsal).HasColumnName("dorsal");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.IdJugador).HasColumnName("idJugador");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.EquipoJugadors)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_EquipoJugador_Temporadas");
        });

        modelBuilder.Entity<FasesCompeticion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Fases");

            entity.ToTable("Fases_Competicion");

            entity.Property(e => e.Competicion).HasColumnName("competicion");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumEquipos).HasColumnName("Num_Equipos");
            entity.Property(e => e.NumPartidos).HasColumnName("Num_Partidos");

            entity.HasOne(d => d.CompeticionNavigation).WithMany(p => p.FasesCompeticions)
                .HasForeignKey(d => d.Competicion)
                .HasConstraintName("FK_Fases_Competiciones");
        });

        modelBuilder.Entity<FotoEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FotoEqui__3213E83F22C9A95A");

            entity.ToTable("FotoEquipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Foto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.FotoEquipos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FotoEquip__idEqu__17F790F9");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.FotoEquipos)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_FotoEquipo_Temporadas");
        });

        modelBuilder.Entity<FotoJugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Foto_Jug__3213E83F11E69462");

            entity.ToTable("Foto_Jugador");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.IdJugador).HasColumnName("id_jugador");
            entity.Property(e => e.Principal).HasColumnName("principal");
            entity.Property(e => e.Temporada).HasColumnName("temporada");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.FotoJugadors)
                .HasForeignKey(d => d.IdJugador)
                .HasConstraintName("FK__Foto_Juga__id_ju__6AEFE058");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.FotoJugadors)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_Foto_Jugador_Temporadas");
        });

        modelBuilder.Entity<FotoJugadorEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FotoJugadorEquipo_1");

            entity.ToTable("FotoJugadorEquipo");

            entity.Property(e => e.EquipoJugador).HasColumnName("equipoJugador");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Foto)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.Jugador).HasColumnName("jugador");
            entity.Property(e => e.Principal).HasColumnName("principal");
            entity.Property(e => e.Secundaria).HasColumnName("secundaria");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UrlFoto)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("urlFoto");

            entity.HasOne(d => d.EquipoJugadorNavigation).WithMany(p => p.FotoJugadorEquipos)
                .HasForeignKey(d => d.EquipoJugador)
                .HasConstraintName("FK_FotoJugadorEquipo_EquipoJugador");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.FotoJugadorEquipos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FotoJugadorEquipo_Equipo");

            entity.HasOne(d => d.JugadorNavigation).WithMany(p => p.FotoJugadorEquipos)
                .HasForeignKey(d => d.Jugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FotoJugadorEquipo_jugador");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.FotoJugadorEquipos)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_FotoJugadorEquipo_Temporadas");
        });

        modelBuilder.Entity<FotoTecnico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FotoTecn__3213E83F6D49A8B3");

            entity.ToTable("FotoTecnico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.IdTecnico).HasColumnName("idTecnico");
            entity.Property(e => e.Tecnico).HasColumnName("tecnico");
            entity.Property(e => e.Temporada).HasColumnName("temporada");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.FotoTecnicos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FotoTecnico_IdEquipo");

            entity.HasOne(d => d.IdTecnicoNavigation).WithMany(p => p.FotoTecnicos)
                .HasForeignKey(d => d.IdTecnico)
                .HasConstraintName("FK_FotoTecnico_TecnicoEquipo");

            entity.HasOne(d => d.TecnicoNavigation).WithMany(p => p.FotoTecnicos)
                .HasForeignKey(d => d.Tecnico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FotoTecnico_IDtecnico");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.FotoTecnicos)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_FotoTecnico_Temporadas");
        });

        modelBuilder.Entity<FotosAlbum>(entity =>
        {
            entity.HasKey(e => e.IdFoto);

            entity.ToTable("FotosAlbum");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Imagen)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Portada).HasColumnName("portada");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("thumbnailImageSrc");

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.FotosAlbums)
                .HasForeignKey(d => d.IdAlbum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FotosAlbum_Albumes");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.FotosAlbums)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_FotosAlbum_Temporadas");
        });

        modelBuilder.Entity<Jornadum>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Jornada");

            entity.Property(e => e.Estado)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

            entity.HasOne(d => d.CompeticionNavigation).WithMany(p => p.Jornada)
                .HasForeignKey(d => d.Competicion)
                .HasConstraintName("FK_Jornada_Competicion");

            entity.HasOne(d => d.FaseNavigation).WithMany(p => p.Jornada)
                .HasForeignKey(d => d.Fase)
                .HasConstraintName("FK_Jornada_Fase");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.IdJugador).HasName("PK__jugador__73F34C022C7CA8C2");

            entity.ToTable("jugador");

            entity.Property(e => e.IdJugador).HasColumnName("idJugador");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.CorreoTutor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo_Tutor");
            entity.Property(e => e.Cp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CP");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Dnie)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("dnie");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaNac).HasColumnName("fechaNac");
            entity.Property(e => e.Foto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Localidad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("localidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreTutor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_Tutor");
            entity.Property(e => e.Provincia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("provincia");
            entity.Property(e => e.Sexo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sexo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.TelefonoTutor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Telefono_Tutor");
            entity.Property(e => e.Temporada).HasColumnName("temporada");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_jugador_Temporadas");
        });

        modelBuilder.Entity<Liga>(entity =>
        {
            entity.Property(e => e.Posicion).HasColumnName("posicion");

            entity.HasOne(d => d.CompeticionNavigation).WithMany(p => p.Ligas)
                .HasForeignKey(d => d.Competicion)
                .HasConstraintName("FK_Ligas_Competiciones");

            entity.HasOne(d => d.EquipoNavigation).WithMany(p => p.Ligas)
                .HasForeignKey(d => d.Equipo)
                .HasConstraintName("FK_Ligas_Equipo");

            entity.HasOne(d => d.FaseNavigation).WithMany(p => p.Ligas)
                .HasForeignKey(d => d.Fase)
                .HasConstraintName("FK_Ligas_Fases_Competicion");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3213E83F14FE0286");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icono");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<MenuRol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuRol__3213E83F6EB7DEAD");

            entity.ToTable("MenuRol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuRols)
                .HasForeignKey(d => d.IdMenu)
                .HasConstraintName("FK__MenuRol__idMenu__30F848ED");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.MenuRols)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__MenuRol__idRol__31EC6D26");
        });

        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.HasKey(e => e.IdNoticia);

            entity.Property(e => e.Album).HasColumnName("album");
            entity.Property(e => e.Contenido)
                .HasColumnType("text")
                .HasColumnName("contenido");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Fijar).HasColumnName("fijar");
            entity.Property(e => e.Portada)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("portada");
            entity.Property(e => e.Subtitulo)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("subtitulo");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("thumbnailImageSrc");
            entity.Property(e => e.TipoNoticia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoNoticia");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .HasColumnName("titulo");

            entity.HasOne(d => d.AlbumNavigation).WithMany(p => p.Noticia)
                .HasForeignKey(d => d.Album)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Noticias_Albumes");
        });

        modelBuilder.Entity<NumeroDocumento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NumeroDo__3213E83FA09F3247");

            entity.ToTable("NumeroDocumento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.UltimoNumero).HasColumnName("ultimo_numero");
        });

        modelBuilder.Entity<Partido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Partidos_Ligas");

            entity.ToTable("Partido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.PuntosLocal).HasColumnName("Puntos_Local");
            entity.Property(e => e.PuntosVisitante).HasColumnName("Puntos_Visitante");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            entity.HasOne(d => d.JornadaNavigation).WithMany(p => p.Partidos)
                .HasForeignKey(d => d.Jornada)
                .HasConstraintName("FK_Partido_Jornada");

            entity.HasOne(d => d.LocalNavigation).WithMany(p => p.PartidoLocalNavigations)
                .HasForeignKey(d => d.Local)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partidos_Ligas_Equipo_Local");

            entity.HasOne(d => d.VisitanteNavigation).WithMany(p => p.PartidoVisitanteNavigations)
                .HasForeignKey(d => d.Visitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partidos_Ligas_Equipo_Visitante");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3213E83FCC6CBAC9");

            entity.ToTable("Producto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK__Producto__idCate__38996AB5");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Enlace)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("enlace");
            entity.Property(e => e.EnlaceExterno).HasColumnName("enlace_externo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Fijar).HasColumnName("fijar");
            entity.Property(e => e.Orden).HasColumnName("orden");
            entity.Property(e => e.Portada)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("portada");
            entity.Property(e => e.Subtitulo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("subtitulo");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Titulo)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("titulo");
            entity.Property(e => e.TituloEnlace)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo_enlace");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3213E83F9A004B81");

            entity.ToTable("Rol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.IdSesion);

            entity.ToTable("Session");

            entity.Property(e => e.IdSesion).HasColumnName("idSesion");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(600)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(600)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tecnico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tecnico__3213E83F6EED950D");

            entity.ToTable("Tecnico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.EsActivo).HasColumnName("esActivo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaNac).HasColumnName("fechaNac");
            entity.Property(e => e.Foto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.Temporada).HasColumnName("temporada");
            entity.Property(e => e.ThumbnailImageSrc)
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.Tecnicos)
                .HasForeignKey(d => d.Temporada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tecnico_Temporadas");
        });

        modelBuilder.Entity<TecnicoEquipo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TecnicoE__3213E83F28A14CAD");

            entity.ToTable("TecnicoEquipo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EsActivo)
                .HasColumnType("datetime")
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Funcion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("funcion");
            entity.Property(e => e.IdEquipo).HasColumnName("idEquipo");
            entity.Property(e => e.IdTecnico).HasColumnName("idTecnico");
            entity.Property(e => e.Temporada).HasColumnName("temporada");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.TecnicoEquipos)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("FK__TecnicoEq__idEqu__74AE54BC");

            entity.HasOne(d => d.IdTecnicoNavigation).WithMany(p => p.TecnicoEquipos)
                .HasForeignKey(d => d.IdTecnico)
                .HasConstraintName("FK__TecnicoEq__idTec__75A278F5");

            entity.HasOne(d => d.TemporadaNavigation).WithMany(p => p.TecnicoEquipos)
                .HasForeignKey(d => d.Temporada)
                .HasConstraintName("FK_TecnicoEquipo_Temporadas");
        });

        modelBuilder.Entity<Temporada>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Fin).HasColumnName("fin");
            entity.Property(e => e.Inicio).HasColumnName("inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("source");
            entity.Property(e => e.Visible).HasColumnName("visible");
        });

        modelBuilder.Entity<TipoFase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tipo_Competicion");

            entity.ToTable("TipoFase");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoNoticium>(entity =>
        {
            entity.ToTable("Tipo_Noticia");

            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A6B3A2856E");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("apellidos");
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.ContrasenaActivada).HasColumnName("Contrasena_Activada");
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Foto)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("foto");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Token)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasMany(d => d.Jugadors).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioJugador",
                    r => r.HasOne<Jugador>().WithMany()
                        .HasForeignKey("Jugador")
                        .HasConstraintName("FK_UsuarioJugador_jugador"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("Usuario")
                        .HasConstraintName("FK_UsuarioJugador_Usuario"),
                    j =>
                    {
                        j.HasKey("Usuario", "Jugador");
                        j.ToTable("UsuarioJugador");
                        j.IndexerProperty<int>("Usuario").HasColumnName("usuario");
                        j.IndexerProperty<int>("Jugador").HasColumnName("jugador");
                    });
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Venta__3213E83F68F6DA9A");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroDocumento");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoPago");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
