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
using System.Transactions;

namespace CBSANTOMERA.BLL.Servicios
{
    public class CompeticionService : ICompeticionService
    {
        private readonly IGenericRepository<Competicione> _Repository;
        private readonly IGenericRepository<TipoCompeticion> _RepositoryTipo;
        private  ILigaService _LigaRepository;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = @"Competiciones\";
        private readonly ITemporadaService _TemporadaRepository;
        private readonly ICategoriaJugadorService _CategoriaRepository;
        private readonly IEquipoCompeticionService _EquipoCompeticionRepository;
        private readonly IEquipoService _EquipoRepository;
        private readonly IFaseCompeticionService _FaseCompeticionService;
        private readonly IMapper _mapper;
        private readonly IArchivosService _ArchivoService;
        public CompeticionService(IGenericRepository<Competicione> jugadorRepository, ILigaService ligaRepository,IGenericRepository<TipoCompeticion> _RepositoryTipo, IMapper mapper, IArchivosService _ArchivoService, ITemporadaService temporadaService, IEquipoService equipoRepository, ICategoriaJugadorService categoriaRepository, IFaseCompeticionService faseCompeticionService, IEquipoCompeticionService equipòCompeticionRepository)
        {
            _Repository = jugadorRepository;
            //_clubRepository = clubRepository;
            _mapper = mapper;
            this._ArchivoService = _ArchivoService;
            _TemporadaRepository = temporadaService;
            _EquipoRepository = equipoRepository;
            _CategoriaRepository = categoriaRepository;
            this._RepositoryTipo = _RepositoryTipo;
            _FaseCompeticionService = faseCompeticionService;
            _EquipoCompeticionRepository = equipòCompeticionRepository;
            _LigaRepository = ligaRepository;
        }
        public async Task<CompeticionDTO> CrearCompeticion(CompeticionDTO modelo)
        {

            Competicione competicion = new Competicione();
            try {

                var tipo = await this._RepositoryTipo.ObtenerUnModelo(t => t.Id == modelo.IdTipo);

                if (tipo == null)
                {
                    throw new Exception("El tipo de competición no es válido");
                }

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.EsActivo = false;
                modelo.Estado = "Creado";
                modelo.NumEquipos  = 0;
                modelo.IdTipo = tipo.Id;//gestionar mejor
                modelo.Tipo = tipo.Subtipo;

                if (modelo.imagen == null)
                {
                    modelo.Logo = "Logo_default.png";

                }
                try
                {
                    
                    TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.TemporadaActiva();
                    modelo.Temporada = temporada;
                    //rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, "Albumes");
                   
                    competicion = await _Repository.Crear(modelo.ToModel());
                }
                catch (Exception e)
                {
                    throw new Exception("No se ha podido crear la competición en la BBDD", e);
                }

                string filename = " ";
                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = competicion.ToString() + DateTime.Now;
                    string name = hasha.GetHashCode().ToString();


                    filename = name + extension;

                    competicion.Logo = filename;

                    string carpetaAlbum = competicion.Id+ "\\";

                    string rutaImagen = this.carpetaLocal;

                    try
                    {
                        if (!Directory.Exists(rutaServidor))
                        {
                            Directory.CreateDirectory(rutaServidor);
                        }

                        if (!Directory.Exists(Path.Combine(rutaServidor, modelo.Temporada.Nombre)))
                        {
                            Directory.CreateDirectory(Path.Combine(rutaServidor, modelo.Temporada.Nombre));
                        }

                        rutaImagen = Path.Combine(rutaServidor, modelo.Temporada.Nombre);

                        if (!Directory.Exists(Path.Combine(rutaImagen, carpetaLocal)))
                        {
                            Directory.CreateDirectory(Path.Combine(rutaImagen, carpetaLocal));
                        }
                        if (!Directory.Exists(Path.Combine(rutaImagen, carpetaLocal, carpetaAlbum)))
                        {
                            Directory.CreateDirectory(Path.Combine(rutaImagen, carpetaLocal, carpetaAlbum));
                        }
                        rutaImagen = Path.Combine(rutaImagen, carpetaLocal, carpetaAlbum);


                    }
                    catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero de la competición."); }


                    try
                    {
                        AccionFile accion = new AccionFile();

                        accion.accion = "Guardar";
                        accion.file = modelo.imagen;
                        accion.thumbnail = false;
                        accion.rutaDesten = rutaImagen + competicion.Logo;
                        //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                        // accion.sizeThumbnail = 120;
                        this._ArchivoService.Ejecutar(accion);



                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se pudo Crear el Logo");
                    }


                }

                bool respuesta = await _Repository.Editar(competicion);






                if (competicion.Id == 0 && !respuesta)
                {
                    throw new TaskCanceledException("No se pudo crear la competición");
                }

                return CompeticionDTO.ToDTO(competicion);

            }
            catch (Exception ex) { }
            throw new NotImplementedException();
        }



        public async Task<bool> GuardarCalendario(CompeticionDTO modelo)
        {
            try {
                if (modelo.calendario != null)
                {
                    string extension = Path.GetExtension(modelo.calendario.FileName);
                    if (extension != ".pdf") { 
                        throw new Exception("EL tipo de archivo no es pdf");
                    }
                    TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.TemporadaActiva();
                    modelo.Temporada = temporada;
                    string id = modelo.Id + "\\";
                    string rutaImagen = this.carpetaLocal;
                    
                    try
                    {
                        var ruta = Path.Combine(rutaServidor, id);
                        AccionFile accion = new AccionFile();

                        accion.accion = "Guardar";
                        accion.file = modelo.calendario;
                        accion.thumbnail = false;
                        accion.rutaDesten = ruta + "calendario.pdf";
                        //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                        // accion.sizeThumbnail = 120;
                        this._ArchivoService.Ejecutar(accion);



                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se ha podido guardar el archivo");
                    }


                }
                else
                {

                }

            }
            catch (Exception ex) { }
           

            return true;
        
        }

            public async Task<bool> EditarCompeticion(CompeticionDTO modelo)
        {
            if (modelo.IdTipo == null)
            {
                throw new Exception("El tipo de competición no es válido");
            }
            var tipo = await this._RepositoryTipo.ObtenerUnModelo(t => t.Id == modelo.IdTipo);

            if (tipo == null)
            {
                throw new Exception("El tipo de competición no es válido");
            }


            
            string filename = "";
            string extension = "";
            string hasha = modelo.Id.ToString() + DateTime.Now;
            string name = hasha.GetHashCode().ToString();
            if (modelo.imagen != null)
            {
                extension = Path.GetExtension(modelo.imagen.FileName);
                modelo.Logo = name + extension;
            }

            try
            {
                var competicione = await this._Repository.ObtenerUnModelo(c=> c.Id == modelo.Id);

                if (modelo.calendario != null || modelo.IdTipo != competicione.Idtipo || modelo.Categoria != competicione.Categoria) {
                    await this._LigaRepository.EliminarJornadaEnteraPorCompeticion(modelo.Id);
                    if (competicione.NumEquipos >= 3) {
                        competicione.Estado = "Configurado";
                      
                       
                    }
                    else {
                        competicione.Estado = "Creado";
                    }
                
                    //eliminar todas las jornadas
                }

                TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.Ver((int)competicione.Temporada);
                if (temporada.Id == 0)
                {
                    throw new Exception("No se ha podido editar la competicion, porque no hay ninguna temporada activa.");
                }

              

                if (competicione.Id == 0)
                {
                    throw new Exception("La competición a actualizar no existe en el BBDD");
                }

                rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);

                competicione.Categoria = modelo.Categoria;
                competicione.Idtipo = modelo.IdTipo;
                
                competicione.Tipo = tipo.Subtipo;
                competicione.Logo = modelo.Logo;
                //competicione.Estado = modelo.Estado;

                var res = await _Repository.Editar(competicione);

                if (res == true)
                {
                    if (modelo.imagen != null)
                    {
                        string carpetaAlbum = modelo.Id + "\\";
                        string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                        string filepath = Path.Combine(rutaImagen, modelo.Logo);
                        extension = Path.GetExtension(modelo.imagen.FileName);
                        try
                        {
                            AccionFile accion = new AccionFile();
                            accion.accion = "Update";
                            accion.thumbnail = false;
                            accion.rutaOrigen = rutaImagen + competicione.Logo;

                            accion.rutaDesten = rutaImagen + name + extension;
                            //accion.rutaDestenThumbnail = rutaImagen + name + "_thumbnail" + extension; ;

                            //accion.sizeThumbnail = 600;
                            accion.file = modelo.imagen;

                            this._ArchivoService.Ejecutar(accion);


                        }
                        catch (Exception ex) { throw new Exception(ex.Message); }


                    }
                   
                    if (modelo.calendario != null)
                    {
                       await this.GuardarCalendario(modelo);
                    }
                    

                    }
            
                else
            {
                return false;
            }

            return true;
        }
            catch (Exception ex)
            {
                throw;

            }
        }

        public async Task<bool> EliminarCompeticion(int id)
        {
            try
            {


                var modeloEncontrado = await _Repository.ObtenerUnModelo(p => p.Id == id);

                //Eliminar Clasificacion, eliminar Jornadas, Eliminar fases, eliminar equipoCompeticion

                

                if (modeloEncontrado == null) { throw new TaskCanceledException("La competición no existe"); }
                List<FaseCompeticionDTO> faseCompeticiones = await _FaseCompeticionService.Listar(modeloEncontrado.Id);

                TipoCompeticion tipo = await this._RepositoryTipo.ObtenerUnModelo(t=> t.Id == modeloEncontrado.Idtipo);
                
                    if (!await this._LigaRepository.EliminarLigaEntera(id)) {

                        throw new TaskCanceledException("No se ha podido eliminar la clasificacion de la competición");
                    }

                foreach (var item in faseCompeticiones)
                {
                    await this._LigaRepository.EliminarJornadaEntera(item.Id, modeloEncontrado.Id);
                }



                if (!await this._EquipoCompeticionRepository.EliminarEquiposCompeticion(id)) {

                    throw new TaskCanceledException("No se ha podido eliminar los equipos de la competición");
                }
                foreach (var item in faseCompeticiones)
                {
                    item.Competicion  = CompeticionDTO.ToDTO(modeloEncontrado);
                    await this._FaseCompeticionService.EliminarCompeticion(item.Id);
                }



                bool respuesta = await _Repository.Eliminar(modeloEncontrado);
                if (respuesta)
                {

                    if (modeloEncontrado.Logo != "female_default_profile.png" && modeloEncontrado.Logo != "male_default_profile.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {
                        CompeticionDTO modelo = CompeticionDTO.ToDTO(modeloEncontrado);
                        modelo.Temporada = await this._TemporadaRepository.Ver((int)modeloEncontrado.Temporada);
                        string rutaImagen = Path.Combine(rutaServidor, modelo.Temporada.Nombre,carpetaLocal );
                        AccionFile accion = new AccionFile();

                        accion.accion = "Eliminar";
                        accion.thumbnail = false;
                        accion.rutaOrigen = rutaImagen + modelo.Logo;
                        this._ArchivoService.Ejecutar(accion);
                    }

                }
                else
                {


                    throw new TaskCanceledException("No se pudo Eliminar");

                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public Task<List<CompeticionDTO>> Equipo_Temporada(int idEquipo)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CompeticionDTO>> Lista()
        {
            try
            {
                var listaModelos = await _Repository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<CompeticionDTO> lista = new List<CompeticionDTO>();

                foreach (var item in listaModelos)
                {
                    CompeticionDTO modelo = CompeticionDTO.ToDTO(item); 
                        

                    if (item.Temporada != null)
                    {
                       modelo.Temporada = await _TemporadaRepository.Ver((int)item.Temporada);
                    }



                    lista.Add(modelo);
                }
                return lista;

            }
            catch
            {
                throw new Exception("No se ha podido obtener la lista de competiciones");

            }
        }

        public async Task<List<CompeticionDTO>> Lista(int idTemporada)
        {
            try
            {
                var listaModelos = await this._Repository.Consultar(c=> c.Temporada == idTemporada);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<CompeticionDTO> lista = new List<CompeticionDTO>();

                foreach (var item in listaModelos)
                {
                    CompeticionDTO modelo = CompeticionDTO.ToDTO(item);


                    if (item.Temporada != null)
                    {
                        modelo.Temporada = await _TemporadaRepository.Ver((int)item.Temporada);
                    }



                    lista.Add(modelo);
                }
                return lista;

            }
            catch
            {
                throw new Exception("No se ha podido obtener la lista de competiciones");

            }
        }

        public async Task<List<CompeticionDTO>> Lista(int idEquipo, int temporada)
        {
            try
            {
                var listaModelos = await this._Repository.Consultar(c => c.Temporada == temporada);

                //listaModelos.

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<CompeticionDTO> lista = new List<CompeticionDTO>();

                foreach (var item in listaModelos)
                {
                    CompeticionDTO modelo = CompeticionDTO.ToDTO(item);


                    if (item.Temporada != null)
                    {
                        modelo.Temporada = await _TemporadaRepository.Ver((int)item.Temporada);
                    }



                    lista.Add(modelo);
                }
                return lista;

            }
            catch
            {
                throw new Exception("No se ha podido obtener la lista de competiciones");

            }
        }

        public async Task <List<List<JornadaDTOSimple>>> IniciarCompeticion(CompeticionDTO modelo)
        {
            List<List<JornadaDTOSimple>> jornadas = new List<List<JornadaDTOSimple>>();
            try {
                var competicion = await this.VerCompeticion(modelo.Id);

                if (competicion.Estado != "Configurado") {

                    var listaLiga = await this._LigaRepository.ListaJornadas(competicion);
                    if (listaLiga != null && competicion.Estado == "Creado") {
                        competicion.Estado = "Configurado";
                        await this._Repository.Editar(competicion.ToModel());
                        return listaLiga;
                    }
                    throw new Exception("El estado de la competición no permite Iniciarla, dicho estado actual es "+competicion.Estado);
                }
                var tipo = await this._RepositoryTipo.ObtenerUnModelo(t=> t.Id == competicion.IdTipo);
                if (tipo == null) {

                    throw new Exception("La estructura de la competición es incorrecta");
                }
                if (tipo.Tipo == "Liga") {
                    var listaEquipos = await this._EquipoCompeticionRepository.Listar(competicion.Id );
                    if (listaEquipos == null)
                    {
                        throw new Exception("No hay equipos en la competición");

                    }

                   

                    if (listaEquipos.Count <= 2)
                    {
                        throw new Exception("No hay equipos suficientes para crear una liga");

                    }
                    List<EquipoDTO> equipos = new List<EquipoDTO>();
                    foreach (var equip in listaEquipos)
                    {
                        equipos.Add(equip.Equipo);
                    }

                    competicion.NumEquipos = equipos.Count;

                    await this._Repository.Editar(competicion.ToModel());
                    FaseCompeticionDTO fase = new FaseCompeticionDTO();
                    fase.Competicion = competicion;
                    fase.NumEquipos = listaEquipos.Count;
                    fase.Estado = "Creado";
                    fase.Nombre = "Clasificación Liga";   
                    fase.FechaCreacion = DateTime.Now;
                    fase.FechaModificacion = DateTime.Now;

                    if (tipo.Tipo == "Liga")
                    {

                        var listaLiga = await this._LigaRepository.EliminarLigaEntera(competicion.Id);
                        var listaJorndas = await this._LigaRepository.EliminarJornadaEnteraPorCompeticion(competicion.Id);
                        var lista = await this._FaseCompeticionService.Listar(competicion.Id);
                        foreach (var item in lista)
                        {
                            var fase0 = await this._FaseCompeticionService.Eliminar(item.Id);
                        }

                    }
                    if (tipo.Subtipo== "Ida y Vuelta") { 
                        
                       

                        var fase0 = await this._FaseCompeticionService.Crear(fase, modelo);
                        /*var lista = await this._LigaRepository.ListaJornadas(await this.VerCompeticion(modelo.Id));
                        if (lista != null && lista.Count != 0)
                        {

                            if (equipos.Count % 2 != 0)
                            {
                                fase0.NumPartidos = (equipos.Count)*2;
                            }
                            else
                            {
                                fase0.NumPartidos = (equipos.Count - 1)*2;
                            }

                            if (fase0.NumPartidos != lista.Count)
                            {
                                //se puede eliminar y volver acrear
                                throw new Exception("Los partidos ya han sido generados");
                            }


                        }
                        else
                        {
                            jornadas = await this._LigaRepository.InicializarJornadas(listaEquipos, competicion, fase0);
                        }*/

                        fase0.Competicion = competicion;
                        jornadas = await this._LigaRepository.InicializarJornadasCalendarioPDF(listaEquipos, competicion, fase0);
                        if (!await this._LigaRepository.InicializarLiga(equipos, fase0))
                        {

                            throw new Exception("No se ha podido Iniciar la competición, se ha producido un error a la hora de crear la clasificación");
                        }
                        //jornadas = await this._LigaRepository.InicializarJornadas(listaEquipos, competicion, fase0);
                        //await this._LigaRepository.InicializarLiga(equipos, fase0);
                        //fase.NumPartidos = jornadas.Count;
                        /* if (fase.NumPartidos != jornadas.Count) {
                             throw new Exception("La liga se ha generado incorrectamente");
                         }*/
                        //competicion.Estado = "Configurado";
                        await this._Repository.Editar(competicion.ToModel());
                        return jornadas;
                    }
                    if (tipo.Subtipo == "Ida") {
                        //fase.NumPartidos = (listaEquipos.Count - 1);
                        var fase0 = await this._FaseCompeticionService.Crear(fase, modelo);
                        /*var lista = await this._LigaRepository.VerLiga(modelo.Id);
                        if (lista != null)
                        {

                            if (equipos.Count % 2 != 0)
                            {
                                fase0.NumPartidos = lista.Count;
                            }
                            else
                            {
                                fase0.NumPartidos = lista.Count - 1;
                            }

                            if (fase0.NumPartidos != lista.Count)
                            {
                                //se puede eliminar y volver acrear
                                throw new Exception("Los partidos ya han sido generados");
                            }


                        }
                        else {
                            jornadas = await this._LigaRepository.InicializarJornadas(listaEquipos, competicion, fase0);
                        }*/


                        fase0.Competicion = competicion;
                        jornadas = await this._LigaRepository.InicializarJornadas(listaEquipos, competicion, fase0);
                        if (!await this._LigaRepository.InicializarLiga(equipos, fase0)) {

                            throw new Exception("No se ha podido Iniciar la competición, se ha producido un error a la hora de crear la clasificación");
                        }
                        /*if (fase.NumPartidos != jornadas.Count)
                        {
                            throw new Exception("La liga se ha generado incorrectamente");
                        }*/

                        //competicion.Estado = "Configurado";
                        competicion.NumEquipos = equipos.Count;

                        competicion.Estado = "Preparado";
                        
                        await this._Repository.Editar(competicion.ToModel());
                        return jornadas;
                    }

                   
                    



                }

                if (tipo.Tipo == "PlayOffs")
                {


                }
                return null;

            }
            catch(Exception ex) { throw; }
            

        }

            public async Task<bool> CrearLiga(CompeticionDTO modelo) {

            try{
                var tipo = await this._RepositoryTipo.ObtenerUnModelo(t => t.Id == modelo.IdTipo);

                if (tipo == null)
                {
                    throw new Exception("El tipo de competición no es válido");
                }
                if (tipo.Tipo == "Liga")
                {
                    FaseCompeticionDTO fase = new FaseCompeticionDTO();
                    fase.Competicion = modelo;
                    fase.Nombre = "Clasificación";
                    fase.Estado = "Creado";
                    
                }



                return true;

            }
            catch(Exception ex) { throw; }
           
        }


        public async Task<bool> CrearPlayOffs(CompeticionDTO modelo)
        {

            try
            {
                var tipo = await this._RepositoryTipo.ObtenerUnModelo(t => t.Id == modelo.IdTipo);

                if (tipo == null)
                {
                    throw new Exception("El tipo de competición no es válido");
                }




                if (tipo.Tipo == "PlayOffs")
                {


                    FaseCompeticionDTO fase = new FaseCompeticionDTO();
                    fase.Competicion = modelo;
                    fase.Nombre = "Clasificación";
                    fase.Estado = "Creado";

                }



                return true;

            }
            catch (Exception ex) { throw; }

        }

        public async Task<bool> AddEquipos()
        {

            return true;

        }

        public async Task<CompeticionDTO> VerCompeticion(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var modelo0 = await _Repository.ObtenerUnModelo(p => p.Id== id);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                CompeticionDTO modelo= CompeticionDTO.ToDTO(modelo0);
                modelo.Temporada = await this._TemporadaRepository.Ver((int)modelo0.Temporada);
                var Categoria = await this._CategoriaRepository.Obtener(id);
               

                try
                {
                    if (modelo0 == null)
                    {
                        throw new Exception("La competición no se encuentra en la base de datos");
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return modelo;

            }
            catch
            {
                throw;

            }
        }
    }
}
