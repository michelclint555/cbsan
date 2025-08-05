using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class TecnicoService : ITecnicoService
    {
        private readonly IGenericRepository<Tecnico> _tecnicoRepository;

        private readonly ITemporadaService _TemporadaRepository;
        private readonly IArchivosService _ArchivoService;

        //private readonly IEquipoService _equipoService;
        //private readonly IFotoTecnicoEquipoService _fotoTecnicoEquipoService;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private readonly string mirutajugadores = @"Tecnicos\";

        public TecnicoService(IGenericRepository<Tecnico> tecnicoRepository, IMapper mapper, IArchivosService _ArchivoService, ITemporadaService temporadaService  /*IFotoTecnicoEquipoService _fotoTecnicoEquipoService*/
        /* IEquipoService _equipoService*/)
        {
            _tecnicoRepository = tecnicoRepository;
            this._TemporadaRepository = temporadaService;
            _mapper = mapper;
           // this._equipoService = _equipoService;
            //this._fotoTecnicoEquipoService = _fotoTecnicoEquipoService;
            this._ArchivoService = _ArchivoService;
        }
        public async Task<TecnicoDTO> Crear(TecnicoDTO modelo)
        {

            Tecnico tecnico = new Tecnico();
            var modelo0 = new Tecnico();

            try {
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.EsActivo = false;
                if (modelo.imagen == null)
                {
                    modelo.Foto = "coach.png";
                    modelo.ThumbnailImageSrc = "coach.png";


                }

                try
                {
                    TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.TemporadaActiva();
                    modelo.Temporada = temporada;
                    //rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, "Albumes");
                    tecnico = TecnicoDTO.ToModel(modelo);

                      modelo0 = await this._tecnicoRepository.Crear(tecnico);
                }
                catch (Exception e)
                {
                    throw new Exception("No se ha podido crear el técnico en la BBDD", e);
                }

                string filename = " ";
                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = modelo.ToString() + DateTime.Now;
                    string name = hasha.GetHashCode().ToString();


                    filename = name + extension;

                    tecnico.Foto = filename;
                    tecnico.ThumbnailImageSrc = filename;



                    string rutaImagen = this.mirutajugadores;

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

                        if (!Directory.Exists(Path.Combine(rutaImagen, mirutajugadores)))
                        {
                            Directory.CreateDirectory(Path.Combine(rutaImagen, mirutajugadores));
                        }

                        rutaImagen = Path.Combine(rutaImagen, mirutajugadores);


                    }
                    catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                    try
                    {
                        AccionFile accion = new AccionFile();

                        accion.accion = "Guardar";
                        accion.file = modelo.imagen;
                        //accion.thumbnail = true;
                        accion.rutaDesten = rutaImagen + tecnico.Foto;
                        //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                        // accion.sizeThumbnail = 120;
                        this._ArchivoService.Ejecutar(accion);



                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se pudo Crear el archivo");
                    }
                }


                } catch (Exception ex) { }

            tecnico.Id = modelo0.Id;

            bool respuesta = await this._tecnicoRepository.Editar(tecnico);






            if (modelo.Id== 0 && !respuesta)
            {
                throw new TaskCanceledException("No se pudo crear el jugador");
            }

            return TecnicoDTO.ToDTO(modelo0);


        }

        public async Task<bool> Editar(TecnicoDTO modelo)
        {
            try
            {


                Tecnico tecnico= new Tecnico();
                //var jugadorModelo = jugador;
                var tecnicoEncontrado = await this._tecnicoRepository.Obtener(u => u.Id == modelo.Id);

                if (tecnicoEncontrado == null) { throw new TaskCanceledException("El jugador no existe"); }

                modelo.Temporada = await this._TemporadaRepository.Ver((int)tecnicoEncontrado.Temporada);
                tecnico = TecnicoDTO.ToModel(modelo);

                








                var filename = "";


                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = tecnicoEncontrado.ToString() + DateTime.Now;
                    string name = hasha.GetHashCode().ToString();
                    filename = name + extension;
                    var filepath = Path.Combine(this.mirutajugadores, filename);
                    if (tecnicoEncontrado.Foto == "coach.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {

                        tecnico.Foto = filename;


                        string rutaImagen = this.mirutajugadores;

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

                            if (!Directory.Exists(Path.Combine(rutaImagen, mirutajugadores)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaImagen, mirutajugadores));
                            }

                            rutaImagen = Path.Combine(rutaImagen, mirutajugadores);


                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;
                            accion.rutaDesten = rutaImagen + tecnico.Foto;
                            accion.rutaOrigen = rutaImagen + modelo.Foto;
                            //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                            // accion.sizeThumbnail = 120;
                            this._ArchivoService.Ejecutar(accion);
                            tecnicoEncontrado.Foto = tecnico.Foto;


                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }
                    }
                    else
                    {

                        tecnico.Foto = filename;
                        //jugadorEncontrado.Foto = filename;

                        string rutaImagen = this.mirutajugadores;

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

                            if (!Directory.Exists(Path.Combine(rutaImagen, mirutajugadores)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaImagen, mirutajugadores));
                            }

                            rutaImagen = Path.Combine(rutaImagen, mirutajugadores);


                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;
                            accion.rutaDesten = rutaImagen + tecnico.Foto;
                            accion.rutaOrigen = rutaImagen + tecnicoEncontrado.Foto;
                            //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                            // accion.sizeThumbnail = 120;
                            this._ArchivoService.Ejecutar(accion);



                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }

                    }


                }




                if (modelo.imagen == null ) {
                    if (modelo.Foto == "") {
                        tecnico.Foto = "coach.png";
                    }
                    else
                    {
                        tecnico.Foto = tecnicoEncontrado.Foto;

                    }


                   
                }
                //tecnicoEncontrado.Foto = filename;
               
                tecnico.FechaCreacion = tecnicoEncontrado.FechaCreacion;
                tecnico.FechaModificacion = tecnicoEncontrado.FechaModificacion;
                //jugador.Usuario = tecnicoEncontrado.Usuario;
                tecnico.Temporada = tecnicoEncontrado.Temporada;
                bool respuesta = await _tecnicoRepository.Editar(tecnico);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el técnico");
                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var Encontrado = await _tecnicoRepository.Obtener(p => p.Id == id);
                if (Encontrado == null) { throw new TaskCanceledException("El Tecnico no existe"); }
                bool respuesta = await _tecnicoRepository.Eliminar(Encontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo Eliminar");
                }
                else {
                    TecnicoDTO tecnico= TecnicoDTO.ToDTO(Encontrado);
                    tecnico.Temporada = await this._TemporadaRepository.Ver((int)Encontrado.Temporada);
                    string rutaImagen = Path.Combine(rutaServidor, tecnico.Temporada.Nombre, mirutajugadores);
                    AccionFile accion = new AccionFile();

                    accion.accion = "Eliminar";
                    accion.thumbnail = false;
                    accion.rutaOrigen = rutaImagen + tecnico.Foto;
                    this._ArchivoService.Ejecutar(accion);

                }

                return respuesta;

            }
            catch
            {
                throw new TaskCanceledException("No se pudo eliminar el técnico");

            }
        }

        public async Task<List<TecnicoDTO>> Lista()
        {

            try
            {
                var listaTecnicos = await this._tecnicoRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<TecnicoDTO> lista = new List<TecnicoDTO>();

                foreach (var item in listaTecnicos)
                {
                    TecnicoDTO tecnico = TecnicoDTO.ToDTO(item);

                    if (item.Temporada != null)
                    {
                        tecnico.Temporada = await this._TemporadaRepository.Ver((int)item.Temporada);
                    }



                    lista.Add(tecnico);
                }
                return lista;

            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de técnicos");

            }
            
           
            




        }

        


        public async Task<List<TecnicoEquipoDTO>> ListaTodosTecnicos()
        {
            try
            {
                List<TecnicoDTO> listaTecnicos= await this.Lista();

                if (listaTecnicos== null)
                {

                }

                List<TecnicoEquipoDTO> listaDTO = new List<TecnicoEquipoDTO>();

                
                List<TecnicoDTO> Lista = this._mapper.Map<List<TecnicoDTO>>(listaTecnicos);

                Lista.ForEach((tecnico) =>
                {
                    TecnicoEquipoDTO EJDTO = new TecnicoEquipoDTO();
                    //JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);
                    EJDTO.tecnico= tecnico;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.Funcion = "";
                    EJDTO.IdEquipo = 0;
                    EJDTO.Id = 0;
                    EJDTO.listaFotos = new List<FotoTecnicoEquipoDTO>();
                    listaDTO.Add(EJDTO);



                });

                return listaDTO;
            }
            catch
            {
                throw;
            }



        }


        

        private List<TecnicoEquipoDTO> ListaJugadoresEquipoNoEstan2(List<TecnicoEquipoDTO> tecnicos, List<TecnicoEquipoDTO> tecnico2)
        {
            foreach (TecnicoEquipoDTO tecnico in tecnico2)
            {
                for (int i = 0; i < tecnicos.Count; i++)
                {
                    if (tecnico.tecnico.Id== tecnicos[i].tecnico.Id)
                    {
                        tecnicos.RemoveAt(i);
                    }
                }
            }

            return tecnicos;

        }





    


        public async Task<TecnicoDTO> BuscarTecnico(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var Tecnico0 = await _tecnicoRepository.ObtenerUnModelo(p => p.Id == id);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                TecnicoDTO tecnico = TecnicoDTO.ToDTO(Tecnico0);
                tecnico.Temporada = await this._TemporadaRepository.Ver((int)Tecnico0.Temporada);

                try
                {
                    if (Tecnico0 == null)
                    {
                        throw new Exception("El técnico no se encuentra en la base de datos");
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return tecnico;

            }
            catch
            {
                throw;

            }
        }

        





        /*public async Task<bool> Eliminar(EquipoDTO equipo, TecnicoDTO tecnico)
        {
            try
            {
                //var equipoEncontrado = await _equipoJugadorRepository.Obtener(p => p.Id == id);
                //if (equipoEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                if (await this._fotoTecnicoEquipoService.EliminarFotoSTecnico(tecnico.listaFotos, equipo.IdEquipo))
                {

                    TecnicoEquipo j = new TecnicoEquipo();
                    j.Id = tecnico.Id;
                    j.IdEquipo = tecnico.IdEquipo;
                    j.Funcion = tecnico.Funcion;
                    j.IdTecnico = tecnico.tecnico.Id;


                    if (await this._tecnicoRepository.Eliminar(j))
                    {
                        return true;

                    }
                    return false;

                }



                return false;

            }
            catch
            {
                throw;

            }
        }*/



       /* public async Task<bool> Editar(TecnicoEquipoDTO modelo)
        {
            try
            {
                var equipoModelo = _mapper.Map<TecnicoEquipo>(modelo);
                var equipoEncontrado = await this._tecnicoRepository.Obtener(u => u.Id == equipoModelo.IdEquipo);

                if (equipoEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                equipoEncontrado.IdTecnico = equipoModelo.IdTecnico;
                equipoEncontrado.Id = equipoModelo.Id;
                equipoEncontrado.IdEquipo = equipoModelo.IdEquipo;
                equipoEncontrado.Funcion = modelo.Funcion;


                bool respuesta = await this._tecnicoEquipoRepository.Editar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar");
                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }*/

    }
}
