
//using CBSANTOMERA.BLL.Servicios;
//using CBSANTOMERA.DAL.Repositorios;
//using CBSANTOMERA.BLL.Servicios.Contrato;

//using CBSANTOMERA.DAL.Repositorios.Contrato;

//using CBSANTOMERA.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CBSANTOMERA.MODEL;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DAL.Repositorios;
using CBSANTOMERA.Utility;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.BLL.Servicios;

using CBSANTOMERA.DAL;
using CBSANTOMERA.DAL.DBContext;


namespace CBSANTOMERA.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration) {

            services.AddDbContext<CbsantomeraContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IArchivosService, ArchivosService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICategoriaJugadorService, CategoriaJugadorService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IJugadorService, JugadorService>();
            services.AddScoped<IEquipoJugadorService, EquipoJugadorService>();
            services.AddScoped<IFotoJugadorEquipoService,FotoJugadorEquipoService>();

            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IEquipoService, EquipoService>();
            services.AddScoped<IFotoAlbumService, FotoAlbumService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ITecnicoService, TecnicoService>();
            services.AddScoped<IFotoTecnicoEquipoService, FotoTecnicoEquipoService>();
            services.AddScoped<ITecnicoEquipoService, TecnicoEquipoService>();
            services.AddScoped<INoticiaService, NoticiaService>();
            services.AddScoped<IPromocionService, PromocionService>();
            services.AddScoped<ITemporadaService, TemporadaService>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IContratoEmpresaService, ContratoEmpresaService>();
            services.AddScoped<IOpenIAService, OpenIAService>();
            services.AddScoped<IMessageService, messageService>();
            services.AddScoped<IFaseCompeticionService, FaseCompeticionService>();
            services.AddScoped<IEquipoCompeticionService, EquipoCompeticionService>();
            services.AddScoped<ILigaService, LigaService>();
            services.AddScoped<IPartidoService, PartidoService>();
            services.AddScoped<ICompeticionService, CompeticionService>();

            services.AddScoped<IJornadaService, JornadaService>();
            services.AddScoped<ICierreCompeticionService, CierreCompeticionService>();

            services.AddScoped<IPlayoffService, PlayoffService>();

            //services.AddScoped<IOpenIAService, OpenIAService>();
        }
    }
}
