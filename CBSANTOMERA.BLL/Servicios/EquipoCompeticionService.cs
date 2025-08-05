using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class EquipoCompeticionService : IEquipoCompeticionService
    {
        private readonly IGenericRepository<EquipoCompeticion> _Repository;
        //private readonly IGenericRepository<TipoCompeticion> _RepositoryTipo;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = @"Competiciones\";
        private readonly ITemporadaService _TemporadaRepository;
        //private readonly ICompeticionService _CompeticionRepository;
        private readonly ICategoriaJugadorService _CategoriaRepository;
        private readonly IEquipoService _EquipoRepository;
        private readonly IFaseCompeticionService _FaseCompeticionService;
        private readonly IMapper _mapper;
        private readonly IArchivosService _ArchivoService;
        public EquipoCompeticionService(IGenericRepository<EquipoCompeticion> Repository,  IMapper mapper, IArchivosService _ArchivoService, ITemporadaService temporadaService, IEquipoService equipoRepository, ICategoriaJugadorService categoriaRepository, IFaseCompeticionService faseCompeticionService/*, ICompeticionService competicionRepository*/)
        {
            _Repository = Repository;
            //_clubRepository = clubRepository;
            _mapper = mapper;
            this._ArchivoService = _ArchivoService;
            _TemporadaRepository = temporadaService;
            _EquipoRepository = equipoRepository;
            _CategoriaRepository = categoriaRepository;
            //this._RepositoryTipo = _RepositoryTipo;
            _FaseCompeticionService = faseCompeticionService;
            //_CompeticionRepository = competicionRepository;
        }

        public Task<bool> Editar(EquipoCompeticionDTO modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Crear(EquipoCompeticionDTO modelo)
        {

            try {

                if (modelo.Competicion == 0 ||modelo.Incluido == true ||modelo.Equipo.IdEquipo == 0 || modelo.Equipo.idCategoria == 0) {
                    throw new Exception("Los datos son incorrectos.");
                }
                //comprobamos que no se encuentra el equipo en la competicion
                var equiposCompeticion = await this._Repository.ObtenerUnModelo(c => c.Competicion == modelo.Competicion && c.Equipo == modelo.Equipo.IdEquipo);

                if (equiposCompeticion != null)
                {
                    throw new Exception("El equipo ya se encuentra en la competición");
                }

                
                var equipo = await this._EquipoRepository.BuscarEquipo(modelo.Equipo.IdEquipo);
                if (equipo == null)
                {
                    throw new Exception("El equipo No existe");
                }

                //var competicion = await this._CompeticionRepository.VerCompeticion(modelo.Competicion);
                //if (competicion == null)
                //{
                    //throw new Exception("La competición no existe");
                //}
               // if (competicion.Estado != "Creado")
               // {
                    //throw new Exception("El estado de la competición no permite añadir equipos");
                //}

                /*equipo.Categoria = await this._CategoriaRepository.Obtener(modelo.Equipo.idCategoria);
                if (competicion.Categoria != equipo.Categoria.Id)
                {
                    throw new Exception("El equipo no corresponde a la categoria de la competición");
                }*/



                EquipoCompeticion equipoCompeticion = new EquipoCompeticion();
                equipoCompeticion.Competicion = modelo.Competicion;
                equipoCompeticion.Equipo = modelo.Equipo.IdEquipo;
                if (await this._Repository.Crear(equipoCompeticion) != null)
                {
                    return true;
                }
                else {
                    throw new Exception("No se ha podido incluir el equipo en la competición");
                }
               

            } catch (Exception ex) {
                throw new Exception("Ha habido un error al añadir el equipo a la competición");
            }
           

            return true;
        }

        public async Task<bool> EliminarEquiposCompeticion(int idCompeticion)
        {
            try
            {
                //var competicion = await this._CompeticionRepository.VerCompeticion(idCompeticion);
                /*if (competicion.Estado != "Creado")
                {
                    throw new Exception("El estado de la competición no permite eliminar equipos");
                }*/
                var equiposCompeticion = await this._Repository.Consultar(c => c.Competicion == idCompeticion );

                if (equiposCompeticion == null)
                {
                    throw new Exception("La competición no se encuentra en la BBDD");
                }
                foreach (var item in equiposCompeticion)
                {
                    if (!await this._Repository.Eliminar(item)) {

                        throw new Exception("Ha habido un error al eliminar un registro");
                    }
                }
                return true;
                


                
            }
            catch (Exception ex) { throw; }



        }

        public async Task<bool> Eliminar(int idEquipo, int idCompeticion)
        {
            try {
                //var competicion = await this._CompeticionRepository.VerCompeticion(idCompeticion);
                /*if (competicion.Estado != "Creado")
                {
                    throw new Exception("El estado de la competición no permite eliminar equipos");
                }*/
                var equiposCompeticion = await this._Repository.ObtenerUnModelo(c => c.Competicion == idCompeticion && c.Equipo == idEquipo);

                if (equiposCompeticion == null)
                {
                    throw new Exception("El equipo no se encuentra en la competición");
                }
                var res = await this._Repository.Eliminar(equiposCompeticion);
                if (res == true)
                {
                    return true;
                }
                
                
                else
                {
                    throw new Exception("No se ha podido eliminar el equipo en la competición");
                }
            } catch (Exception ex) { throw; }
            


        }

        public async Task<List<EquipoCompeticionDTO>> Listar()
        {
            List<EquipoCompeticionDTO> equiposIncluidos = new List<EquipoCompeticionDTO>();
            List<EquipoCompeticionDTO> equiposNoIncluidos = new List<EquipoCompeticionDTO>();
            return equiposNoIncluidos;

        }

        public async Task<List<EquipoCompeticionDTO>> Listar(int categoria, int club, int idCompeticion)
        {
            List<EquipoCompeticionDTO> equiposNoIncluidos = new List<EquipoCompeticionDTO>();
            List<EquipoCompeticionDTO> equiposIncluidos = await this.Listar(idCompeticion);
            try
            {
                var listaEquiposNO = await this._EquipoRepository.ListaEquiposClubCategoria(club, categoria);

                
               
                    
                        foreach (var item in listaEquiposNO)
                        { 
                            EquipoCompeticionDTO equipo = new EquipoCompeticionDTO();
                            equipo.Equipo = item;
                            equipo.Competicion = 0;
                            equipo.Incluido = false;
                            
                            foreach (var item1 in equiposIncluidos)
                            {
                               
                               
                                if (item.IdEquipo == item1.Equipo.IdEquipo)
                                {
                                    equipo.Competicion = item1.Competicion;
                                    equipo.Incluido = true;
                                }
                                
                            }
                            equiposNoIncluidos.Add(equipo);
                        }
                
                    return equiposNoIncluidos;
            }
            catch (Exception ex) { 
                
            }
              
         
            return equiposNoIncluidos;

        }


        //static List<List<JornadaDTO>> ExtraerPartidosDesdePdf(string ruta)
       // {


           // var partidos = new List<JornadaDTO>();

          /*  using var documento = PdfDocument.Open(ruta);
            int jornadaActual = 0;

            foreach (var pagina in documento.GetPages())
            {
                var texto = pagina.Text;
                var lineas = texto.Split('\n');

                foreach (var linea in lineas)
                {
                    var l = linea.Trim();

                    // Buscar nueva jornada
                    if (l.StartsWith("JORNADA:", StringComparison.OrdinalIgnoreCase))
                    {
                        jornadaActual++;
                        continue;
                    }

                    // Buscar partidos por línea que termina en "número número"
                    var tokens = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length >= 4 &&
                        int.TryParse(tokens[^2], out int puntosLocal) &&
                        int.TryParse(tokens[^1], out int puntosVisitante))
                    {
                        // El nombre del local es todo antes de los 2 últimos tokens
                        string textoSinPuntos = string.Join(' ', tokens, 0, tokens.Length - 2);
                        // Separar local y visitante basados en tabulaciones o doble espacio
                        var partes = textoSinPuntos.Split(new[] { "  ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        if (partes.Length >= 2)
                        {
                            partidos.Add(new Partido
                            {
                                Jornada = jornadaActual,
                                EquipoLocal = partes[0].Trim(),
                                EquipoVisitante = partes[1].Trim(),
                                PuntosLocal = puntosLocal,
                                PuntosVisitante = puntosVisitante
                            });
                        }
                    }
                }
            }

            return partidos;*/
      //  }

        public async Task<List<EquipoCompeticionDTO>> Listar(int idCompeticion)
        {
            try {

                /*var Competicion = await this._CompeticionRepository.VerCompeticion(idCompeticion);
                if (Competicion == null) {
                    throw new Exception("La competición no existe");
                }*/
                var equiposCompeticion = await this._Repository.Consultar(c => c.Competicion == idCompeticion);

                if (equiposCompeticion == null)
                {
                    throw new Exception("No hay equipos en esta competición");
                }

                List<EquipoCompeticionDTO>lista = new List<EquipoCompeticionDTO>();
                foreach (var item in equiposCompeticion)
                {
                       EquipoCompeticionDTO equipo = new EquipoCompeticionDTO();
                    equipo.Equipo = await this._EquipoRepository.BuscarEquipo(item.Equipo);
                    equipo.Competicion = item.Competicion;
                    equipo.Incluido = true;
                    lista.Add(equipo);
                }
                return lista;


            } catch (Exception ex) {

                throw new Exception(ex.Message);
            }
            
        }
    }
}
