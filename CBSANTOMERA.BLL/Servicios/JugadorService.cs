using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class JugadorService : IJugadorService
    {
        private readonly IGenericRepository<Jugador> _jugadorRepository;
        // private readonly IGenericRepository<Club> _clubRepository;

        private readonly IArchivosService _ArchivoService;
        private readonly ITemporadaService _TemporadaRepository;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private  string rutaServidor = @"wwwroot\archivos\Jugadores\";
        private string carpetaLocal = "Clubes";
        private readonly string mirutajugadores =@"Jugadores\";

        public JugadorService(IGenericRepository<Jugador> jugadorRepository, IMapper mapper, IArchivosService _ArchivoService, ITemporadaService temporadaService)
        {
            _jugadorRepository = jugadorRepository;
            //_clubRepository = clubRepository;
            _mapper = mapper;
            this._ArchivoService = _ArchivoService;
            this._TemporadaRepository = temporadaService;
        }
        public async Task<JugadorDTO> Crear(JugadorDTO modelo)
        {
            var jugadorCreado = new Jugador() ;
            try
            {
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.EsActivo = false;
               


                if (modelo.imagen == null)
                {

                    if (modelo.Sexo == "Femenino")
                    {
                        modelo.Foto = "female_default_profile.png";
                        modelo.ThumbnailImageSrc = "female_default_profile.png";

                    }

                    if (modelo.Sexo == "Masculino")
                    {
                        modelo.Foto = "male_default_profile.png";
                        modelo.ThumbnailImageSrc = "male_default_profile.png";
                    }

                }

                try {
                    //TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.TemporadaActiva();
                    //modelo.temporada = temporada;
                    //rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, "Albumes");
                    Jugador jugador = JugadorDTO.ToModel(modelo);
                    jugadorCreado = await _jugadorRepository.Crear(jugador);
                } 
                catch (Exception e) {
                    throw new Exception("No se ha podido crear el jugador en la BBDD", e);
                }

                
                //modelo.IdJugador = jugadorCreado.IdJugador; 
                string filename = " ";
                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha =jugadorCreado.ToString()+DateTime.Now;
                    string name = hasha.GetHashCode().ToString();


                    filename = name + extension;
                    
                    jugadorCreado.Foto = filename;
                    jugadorCreado.ThumbnailImageSrc = filename;

                    
                    
                    string rutaImagen = this.mirutajugadores ;

                    try {
                        if (!Directory.Exists(rutaServidor))
                        {
                            Directory.CreateDirectory(rutaServidor);
                        }
                        
                       

                        


                    } catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }

                   
                        try
                        {
                        AccionFile accion = new AccionFile();

                        accion.accion = "Guardar";
                        accion.file = modelo.imagen;
                        accion.thumbnail = false;
                        rutaImagen = Path.Combine(rutaServidor, jugadorCreado.Foto);
                        accion.rutaDesten = rutaImagen;
                        //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                        // accion.sizeThumbnail = 120;
                        this._ArchivoService.Ejecutar(accion);



                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }
                    
                   
                }
                


               // modelo.IdJugador = jugadorCreado.IdJugador;

                //var jugadorModelo = _mapper.Map<Jugador>(modelo);
                /*var jugadorEncontrado = await _jugadorRepository.Obtener(u => u.IdJugador == jugadorModelo.IdJugador);*/

                
                
                    bool respuesta = await _jugadorRepository.Editar(jugadorCreado);






                if (jugadorCreado.IdJugador == 0 && !respuesta)
                {
                    throw new TaskCanceledException("No se pudo crear el jugador");
                }

                return JugadorDTO.ToDto(jugadorCreado) ;
            }


            catch
            {
                throw;

            }
        }

        public async Task<bool> Editar(JugadorDTO modelo)
        {
            try
            {


                Jugador jugador = new Jugador();
                //var jugadorModelo = jugador;
                var jugadorEncontrado = await _jugadorRepository.Obtener(u => u.IdJugador == modelo.IdJugador);

                if (jugadorEncontrado == null) { throw new TaskCanceledException("El jugador no existe"); }

                    //modelo.temporada = await this._TemporadaRepository.Ver((int)jugadorEncontrado.Temporada);
                    jugador = JugadorDTO.ToModel(modelo);

                if (jugadorEncontrado.Sexo !=  jugador.Sexo) {
                    if (jugador.Sexo == "Femenino")
                    {
                        jugador.Foto = "female_default_profile.png";
                    }

                    if (jugador.Sexo == "Masculino")
                    {
                        jugador.Foto = "male_default_profile.png";
                    }
                }






               

                var filename = "";


                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = jugadorEncontrado.ToString()+DateTime.Now ;
                    string name = hasha.GetHashCode().ToString();
                    filename = name + extension;
                    var filepath = Path.Combine(this.mirutajugadores, filename);
                    if (jugadorEncontrado.Foto == "female_default_profile.png" || jugadorEncontrado.Foto == "male_default_profile.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {

                        jugador.Foto = filename;


                        string rutaImagen = this.mirutajugadores;

                        try
                        {
                            if (!Directory.Exists(rutaServidor))
                            {
                                Directory.CreateDirectory(rutaServidor);
                            }

                           /* if (!Directory.Exists(Path.Combine(rutaServidor, modelo.temporada.Nombre)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaServidor, modelo.temporada.Nombre));
                            }*/

                           /* rutaImagen = Path.Combine(rutaServidor, modelo.temporada.Nombre);

                            if (!Directory.Exists(Path.Combine(rutaImagen, mirutajugadores)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaImagen, mirutajugadores));
                            }

                            rutaImagen = Path.Combine(rutaImagen, mirutajugadores);*/


                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;

                            accion.rutaDesten = Path.Combine(rutaServidor, name + extension);
                            accion.rutaOrigen = rutaImagen = Path.Combine(rutaServidor, name + extension);
                            //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                            // accion.sizeThumbnail = 120;
                            this._ArchivoService.Ejecutar(accion);



                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }
                    }
                    else
                    {

                        jugador.Foto = filename;
                        //jugadorEncontrado.Foto = filename;

                        string rutaImagen = this.mirutajugadores;
                       
                        
                        try
                        {
                            if (!Directory.Exists(rutaServidor))
                            {
                                Directory.CreateDirectory(rutaServidor);
                            }

                           /* if (!Directory.Exists(Path.Combine(rutaServidor, modelo.temporada.Nombre)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaServidor, modelo.temporada.Nombre));
                            }

                            rutaImagen = Path.Combine(rutaServidor, modelo.temporada.Nombre);

                            if (!Directory.Exists(Path.Combine(rutaImagen, mirutajugadores)))
                            {
                                Directory.CreateDirectory(Path.Combine(rutaImagen, mirutajugadores));
                            }

                            rutaImagen = Path.Combine(rutaImagen, mirutajugadores);*/


                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;
                            accion.rutaDesten = Path.Combine(rutaServidor, jugador.Foto); 
                            accion.rutaOrigen = Path.Combine(rutaServidor, jugadorEncontrado.Foto);
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
            

                   
                 

                   jugadorEncontrado.Foto = filename;

                jugador.FechaCreacion = jugadorEncontrado.FechaCreacion;
                jugador.FechaModificacion = jugadorEncontrado.FechaModificacion;
                
                jugador.Temporada = jugadorEncontrado.Temporada;
                bool respuesta = await _jugadorRepository.Editar(jugador);
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
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var jugadorEncontrado = await _jugadorRepository.Obtener(p => p.IdJugador == id);
                if (jugadorEncontrado == null) { throw new TaskCanceledException("El Jugador no existe"); }
                bool respuesta = await _jugadorRepository.Eliminar(jugadorEncontrado);
                if (respuesta)
                {

                    if (jugadorEncontrado.Foto != "female_default_profile.png" && jugadorEncontrado.Foto != "male_default_profile.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {
                        JugadorDTO jugador = JugadorDTO.ToDto(jugadorEncontrado);
                        jugador.temporada = await this._TemporadaRepository.Ver((int)jugadorEncontrado.Temporada);
                        string rutaImagen = Path.Combine(rutaServidor, jugador.temporada.Nombre, mirutajugadores);
                        AccionFile accion = new AccionFile();

                        accion.accion = "Eliminar";
                        accion.thumbnail = false;
                        accion.rutaOrigen = rutaImagen + jugador.Foto;
                        this._ArchivoService.Ejecutar(accion);
                    }
                    
                }
                else {


                    throw new TaskCanceledException("No se pudo Eliminar");

                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<JugadorDTO>> Lista()
        {
            try
            {
                var listaJugadores = await _jugadorRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<JugadorDTO>lista = new List<JugadorDTO>();

                foreach (var item in listaJugadores)
                {
                    JugadorDTO jugador = JugadorDTO.ToDto(item);
                    
                    if (item.Temporada != null) {
                        jugador.temporada = await this._TemporadaRepository.Ver((int)item.Temporada);
                    }
                    
                    

                    lista.Add(jugador);
                }
                return lista;

            }
            catch
            {
                throw;

            }
        }


        public async Task<JugadorDTO> BuscarJugador(int id)
        {
            try
            {
                var listaJugadores = await _jugadorRepository.ObtenerUnModelo(p => p.IdJugador == id);

                if (listaJugadores == null) {
                    return null;
                }
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
               

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                JugadorDTO jugador = JugadorDTO.ToDto(listaJugadores);
                jugador.temporada = await this._TemporadaRepository.Ver((int)listaJugadores.Temporada);

                try {
                    if (listaJugadores == null) {
                        throw new Exception("El jugador no se encuentra en la base de datos");
                    }
                
                } 
                catch (Exception e) { 
                    throw new Exception(e.ToString());
                }
                return jugador;

            }
            catch
            {
                throw;

            }
        }

      
    }
}
