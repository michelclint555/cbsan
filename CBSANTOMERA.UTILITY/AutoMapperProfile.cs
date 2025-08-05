using AutoMapper;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System.Globalization;

namespace CBSANTOMERA.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region Categoria
            CreateMap<CategoriaJugador, CategoriaJugadorDTO>().ReverseMap();
            #endregion Categoria

            #region Club
            CreateMap<Club, ClubDTO>().ReverseMap();
            #endregion Club

            #region Albume
            CreateMap<Albume,AlbumDTO>().ReverseMap();
            #endregion Albume

            #region Equipo
            CreateMap<Equipo, EquipoDTO>().ReverseMap();
            #endregion Equipo

            #region Equipo
            CreateMap<Equipo, EquipoDTO>().ReverseMap();
            #endregion Equipo

            #region EquipoRival
            CreateMap<Equipo, EquipoRivalDTO>().ReverseMap();
            #endregion EquipoRival

            #region Jugador
            CreateMap<Jugador, JugadorDTO>().ReverseMap();
            #endregion Jugador

            #region FotoAlbum
            CreateMap<FotosAlbum, FotoAlbumDTO>().ReverseMap();
            #endregion FotoAlbum

            #region Tecnico
            CreateMap<Tecnico, TecnicoDTO>().ReverseMap();
            #endregion Tecnico



            #region FotoTecnicoEquipo
            CreateMap<FotoTecnico, FotoTecnicoEquipoDTO>().ReverseMap();
            #endregion Tecnico

            #region TecnicoEquipo
            CreateMap<TecnicoEquipo, TecnicoEquipoDTO>().ReverseMap();
            #endregion Tecnico


            #region CategoriaJugador
            CreateMap<CategoriaJugador, CategoriaJugadorDTO>().ReverseMap();
            #endregion CategoriaJugador


            #region EquipoJugador
            CreateMap<EquipoJugador, EquipoJugadorDTO>().ReverseMap();
            #endregion EquipoJugador

            #region FotoJugadorEquipo
            CreateMap<FotoJugadorEquipo, FotoJugadorEquipoDTO>().ReverseMap();
            #endregion FotoJugadorEquipo

            #region Producto

            CreateMap<Producto, ProductoDTO>()/*.ForMember(destino => destino.DescripcionCategoria, opt => opt.MapFrom(origen => origen. .Nombre)
            )*/.
            ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-ES")))).
            ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<ProductoDTO, Producto>().
           // ForMember(destino => destino.IdCategoriaNavigation, opt => opt.Ignore()).
            ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-ES")))).
            ForMember(destino => destino.EsActivo, opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));
            #endregion Producto



            #region Usuario
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
                
            #endregion Usuario

          

            #region sesion
            CreateMap<UsuarioDTO, SesionDTO>().ReverseMap();

            #endregion sesion

            #region sesion
            CreateMap<Usuario, SesionDTO>().ReverseMap();

            #endregion sesion


            #region sesion2
            CreateMap<Session, SesionDTO>().ReverseMap();

            #endregion sesion2

            #region FotoJugadorEquipo
            CreateMap<FotoJugadorEquipo, FotoJugadorEquipoDTO>().ReverseMap();
            #endregion FotoAlbum

            #region Noticia
            CreateMap<Noticia, NoticiasDTO>().ReverseMap();
            #endregion Noticia

            #region Temporada
            CreateMap<Temporada, TemporadaDTO>().ReverseMap();
            #endregion Temporada

            

            #region Promocion
            CreateMap<Promocione, PromocionDTO>().ReverseMap();
            #endregion Promocion

            #region Promocion
            CreateMap<PromocionDTOSmall, PromocionDTO>().ReverseMap();
            #endregion Temporada
            
            #region Promocion
            CreateMap<PromocionDTOSmall, Promocione>().ReverseMap();
            #endregion Temporada


            #region Empresa
            CreateMap<Empresa, EmpresaDTO>().ReverseMap();
            #endregion Empresa

            #region Empresa
            CreateMap<EmpresaDTOSmall, EmpresaDTO>().ReverseMap();
            #endregion Empresa

            #region Empresa
            CreateMap<PromocionDTOSmall, Empresa>().ReverseMap();
            #endregion 

            #region Empresa
            CreateMap<PromocionDTO, Empresa>().ReverseMap();
            #endregion Empresa



            #region ContratoEmpresa
            CreateMap<ContratoEmpresa, ContratoEmpresaDTO>().ReverseMap();
            #endregion ContratoEmpresa

            #region ContratoEmpresaDTO
            CreateMap<ContratoEmpresaDTOSmall, ContratoEmpresaDTO>().ReverseMap();
            #endregion ContratoEMpresaDTO

            #region ContratoEMpresaDTOSmall
            CreateMap<ContratoEmpresaDTOSmall, ContratoEmpresa>().ReverseMap();
            #endregion ContratoEMpresaDTOSmall

            /*#region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-ES"))))
                .ForMember(destino => destino.FechaRegistro, opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy")));

            CreateMap<VentaDTO, Venta>()
                .ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Total, new CultureInfo("es-ES"))));
            #endregion Venta*/


            /*#region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>().
                ForMember(destino => destino.DescripcionProductos, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)).
                ForMember(destino => destino.PrecioTexto, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-ES")))).
                ForMember(destino => destino.TotalTexto, opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-ES"))));

            CreateMap<DetalleVentaDTO, DetalleVenta>().
                ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-ES")))).
                 ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-ES"))));

            #endregion DetalleVenta

            #region Reporte*/

            /*CreateMap<DetalleVenta, ReporteDTO>().
               ForMember(destino => destino.FechaRegistro, opt => opt.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))).
               ForMember(destino => destino.NumeroDocumento, opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento)).
               ForMember(destino => destino.TipoPago, opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago)).
               ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-ES")))).
               ForMember(destino => destino.TotalVenta, opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Total.Value, new CultureInfo("es-ES")))).
               ForMember(destino => destino.Producto, opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)).
               ForMember(destino => destino.Precio, opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-ES")))).
               ForMember(destino => destino.Total, opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-ES"))));
            #endregion*/

        }


    }
}
