using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CBSANTOMERA.BLL.Servicios
{
    public class CompeticionService : ICompeticionService
    {
        private readonly IGenericRepository<Competicione> _Repository;
        private readonly IGenericRepository<TipoFase> _RepositoryTipo;
        private  ILigaService _LigaRepository;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = @"Competiciones\";
        private readonly ITemporadaService _TemporadaRepository;
        private readonly ICategoriaJugadorService _CategoriaRepository;
        private readonly IEquipoCompeticionService _EquipoCompeticionRepository;
        private readonly IGenericRepository<EquipoCompeticion> _EquipoCompeticion;
        private readonly IEquipoService _EquipoRepository;
        private readonly IFaseCompeticionService _FaseCompeticionService;
        private readonly IMapper _mapper;
        private readonly IArchivosService _ArchivoService;
        private readonly IPlayoffService _playoffService;
        private readonly ILigaService _ligaService;
        private readonly IGenericRepository<FasesCompeticion> _faseService;
        private readonly IGenericRepository<Jornada>  _jornadaRepo;
        private readonly IGenericRepository<Partido> _partidoRepo;
        public CompeticionService(IGenericRepository<Competicione> jugadorRepository, ILigaService ligaRepository,IPlayoffService playoffService, IGenericRepository<TipoFase> _RepositoryTipo, IMapper mapper, IArchivosService _ArchivoService, ITemporadaService temporadaService, IEquipoService equipoRepository, ICategoriaJugadorService categoriaRepository, IFaseCompeticionService faseCompeticionService, IEquipoCompeticionService equipòCompeticionRepository, IGenericRepository<Jornada> jornadaRepo, IGenericRepository<Partido> partidoRepo, IFaseCompeticionService _faseService, IGenericRepository<EquipoCompeticion> _EquipoCompeticion )
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
            _playoffService = playoffService;
            _jornadaRepo = jornadaRepo;
            _partidoRepo = partidoRepo;
        }
        public async Task<CompeticionDTO> CrearCompeticion(CompeticionDTO modelo)
        {
            try
            {
                // 1️⃣ Validar tipo de competición
                var tipo = await _RepositoryTipo.ObtenerUnModelo(t => t.Id == modelo.IdTipo);
                if (tipo == null)
                    throw new Exception("El tipo de competición no es válido");

                // 2️⃣ Obtener temporada activa
                var temporada = (TemporadaDTO)await _TemporadaRepository.TemporadaActiva();
                if (temporada == null)
                    throw new Exception("No hay temporada activa");

                // 3️⃣ Inicializar modelo
                modelo.Estado = "Creado";
                modelo.NumEquipos = 0;
                modelo.EsActivo = false;
                modelo.FechaCreacion = DateTime.UtcNow;
                modelo.FechaModificacion = DateTime.UtcNow;
                modelo.Temporada = temporada;
                modelo.IdTipo = tipo.Id;
                modelo.Nombre = tipo.Nombre;

                modelo.Logo ??= "Logo_default.png";

                // 4️⃣ Crear competición en BBDD
                var competicion = await _Repository.Crear(modelo.ToModel());
                if (competicion.Id == 0)
                    throw new Exception("No se pudo crear la competición");

                // 5️⃣ Guardar logo si existe
                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);
                    string nombreArchivo = $"{competicion.Id}_{DateTime.UtcNow.Ticks}{extension}";

                    string rutaBase = Path.Combine(
                        rutaServidor,
                        temporada.Nombre,
                        carpetaLocal,
                        competicion.Id.ToString()
                    );

                    Directory.CreateDirectory(rutaBase);

                    var accion = new AccionFile
                    {
                        accion = "Guardar",
                        file = modelo.imagen,
                        thumbnail = false,
                        rutaDesten = Path.Combine(rutaBase, nombreArchivo)
                    };

                    _ArchivoService.Ejecutar(accion);

                    competicion.Logo = nombreArchivo;
                    competicion.FechaModificacion = DateTime.UtcNow;

                    await _Repository.Editar(competicion);
                }

                return CompeticionDTO.ToDTO(competicion);
            }
            catch (Exception ex)
            {
                // 🔥 AQUÍ sí devolvemos error real
                throw new Exception("Error al crear la competición", ex);
            }
        }



        public async Task IniciarCompeticionAsync(int competicionId)
        {
            var competicion = await _Repository.ObtenerUnModelo(c => c.Id == competicionId);

            if (competicion == null)
                throw new Exception("Competición no encontrada");

            if (competicion.Estado != "Configurado")
                throw new Exception("La competición no puede iniciarse");

            var tipo = await _RepositoryTipo.ObtenerUnModelo(t => t.Id == competicion.Idtipo);

            if (tipo == null)
                throw new Exception("Tipo de competición inválido");

            // 🔵 LIGA
            if (tipo.Nombre == "Liga")
            {
                var faseLiga = await this._faseService.ObtenerUnModelo(f =>
                    f.Competicion == competicionId &&
f.TipoFase == 1); // Asumiendo que TipoFase es un enum y Liga es un valor válido

                if (faseLiga == null)
                    throw new Exception("No existe fase de liga");

                var equipos = await _EquipoCompeticion.Consultar(e =>
                    e.Competicion == competicionId);

                await _ligaService.IniciarLigaAsync(
                    competicion.Id,
                    faseLiga.Id,
                    equipos.Select(e => e.Equipo).ToList()
                );
            }

            competicion.Estado = "EnCurso";
            competicion.FechaModificacion = DateTime.UtcNow;

            await _Repository.Editar(competicion);
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

                if (modelo.calendario != null /*|| modelo.IdTipo != competicione.Idtipo*/ || modelo.Categoria != competicione.Categoria) {
                  // await this._LigaRepository.EliminarJornadaEnteraPorCompeticion(modelo.Id);
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
              //  competicione.Idtipo = modelo.IdTipo;
                
                //competicione.Tipo = tipo.Subtipo;
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


        public async Task ResetearFasesAsync(int competicionId)
        {
            var competicion = await _Repository.ObtenerUnModelo(c => c.Id == competicionId);

            if (competicion == null)
                throw new Exception("Competición no encontrada");

            if (competicion.Estado == EstadoCompeticion.EnCurso ||
                competicion.Estado == EstadoCompeticion.Finalizada)
            {
                throw new InvalidOperationException(
                    "No se pueden resetear fases con la competición iniciada o finalizada");
            }

            // 1️⃣ Obtener todas las fases
            var fases = await this._FaseCompeticionService.Listar(competicionId);

            foreach (var fase in fases)
            {
                // 2️⃣ Jornadas de la fase
                var jornadas = await _jornadaRepo.Consultar(j => j.Fase == fase.Id);

                foreach (var jornada in jornadas)
                {
                    // 3️⃣ Partidos
                    var partidos = await _partidoRepo.Consultar(p => p.Jornada == jornada.Id);

                    foreach (var partido in partidos)
                    {
                        await _partidoRepo.Eliminar(partido);
                    }

                    await _jornadaRepo.Eliminar(jornada);
                }

                await this._faseService.Eliminar(fase.ToModel());
            }

            // 4️⃣ Reset estado competición
            competicion.Estado = EstadoCompeticion.Configurado;
            competicion.FechaModificacion = DateTime.UtcNow;

            await _Repository.Editar(competicion);
        }


        public async Task<bool> EliminarCompeticion(int id)
        {
            try
            {


                var competicion = await _Repository.ObtenerUnModelo(c => c.Id == id);

                //Eliminar Clasificacion, eliminar Jornadas, Eliminar fases, eliminar equipoCompeticion



                if (competicion == null)
                    throw new Exception("La competición no existe");

                // 1️⃣ Resetear TODAS las fases (si no iniciada)
                if (competicion.Estado != EstadoCompeticion.EnCurso &&
                    competicion.Estado != EstadoCompeticion.Finalizada)
                {
                    await ResetearFasesAsync(id);
                }
                // 2️⃣ Eliminar equipos competición
                await _EquipoCompeticionRepository.EliminarEquiposCompeticion(id);

                // 3️⃣ Eliminar competición
                bool eliminado = await _Repository.Eliminar(competicion);

                if (!eliminado)
                    throw new Exception("No se pudo eliminar la competición");

              
                if (eliminado)
                {

                    if (competicion.Logo != "female_default_profile.png" && competicion.Logo != "male_default_profile.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {
                        CompeticionDTO modelo = CompeticionDTO.ToDTO(competicion);
                        modelo.Temporada = await this._TemporadaRepository.Ver((int)competicion.Temporada);
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

                return true;

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

        //PASA LA COMPETICION A ESTADO PREPARADO
        public async Task <List<List<JornadaDTOSimple>>> IniciarCompeticion(CompeticionDTO modelo)
        {
            List<List<JornadaDTOSimple>> jornadas = new List<List<JornadaDTOSimple>>();
            try {
                var competicion = await this.VerCompeticion(modelo.Id);

                if (competicion.Estado != "Configurado") {

                    throw new Exception("El estado de la competición no permite Iniciarla,el estado de la competicion ha de ser Configurado, dicho estado actual es " + competicion.Estado);
                    /*var listaLiga = await this._LigaRepository.ListaJornadas(competicion);
                    if (listaLiga != null && competicion.Estado == "Creado") {
                        competicion.Estado = "Configurado";
                        await this._Repository.Editar(competicion.ToModel());
                        return listaLiga;
                    }*/
                    //throw new Exception("El estado de la competición no permite Iniciarla, dicho estado actual es "+competicion.Estado);
                }
                var tipo = await this._RepositoryTipo.ObtenerUnModelo(t=> t.Id == competicion.IdTipo);
                if (tipo == null) {

                    throw new Exception("La estructura de la competición es incorrecta");
                }
                if (tipo.Nombre == "Liga") {
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

                    if (tipo.Nombre == "Liga")
                    {

                        //var listaLiga = await this._LigaRepository.EliminarLigaEntera(competicion.Id);
                       // var listaJorndas = await this._LigaRepository.EliminarJornadaEnteraPorCompeticion(competicion.Id);
                        var lista = await this._FaseCompeticionService.Listar(competicion.Id);
                        foreach (var item in lista)
                        {
                            var fase0 = await this._FaseCompeticionService.Eliminar(item.Id);
                        }

                    }
                    if (tipo.Nombre== "Ida y Vuelta") { 
                        
                       

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
                        //jornadas = await this._LigaRepository.InicializarJornadasCalendarioPDF(listaEquipos, competicion, fase0);
                       // if (!await this._LigaRepository.InicializarLiga(equipos, fase0))
                       // {

                            throw new Exception("No se ha podido Iniciar la competición, se ha producido un error a la hora de crear la clasificación");
                       // }
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
                    if (tipo.Nombre == "Ida") {
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
                       // jornadas = await this._LigaRepository.InicializarJornadas(listaEquipos, competicion, fase0);
                       // if (!await this._LigaRepository.InicializarLiga(equipos, fase0)) {

                            throw new Exception("No se ha podido Iniciar la competición, se ha producido un error a la hora de crear la clasificación");
                       // }
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

                if (tipo.Nombre == "PlayOffs")
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
                if (tipo.Nombre == "Liga")
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




                if (tipo.Nombre == "PlayOffs")
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
public static class EstadoCompeticion
{
    public const string Creado = "Creado";
    public const string Configurado = "Configurado";
    public const string EnCurso = "EnCurso";
    public const string Finalizada = "Finalizada";
}
