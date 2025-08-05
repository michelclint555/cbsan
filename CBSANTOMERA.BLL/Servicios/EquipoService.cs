using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Diagnostics;

namespace CBSANTOMERA.BLL.Servicios
{
    public class EquipoService : IEquipoService
    {
        private readonly IGenericRepository<Equipo> _equipoRepository;
       // private readonly IGenericRepository<Club> _clubRepository;
        private readonly IMapper _mapper;
        private int _Miclub = 1048;
        private readonly ICategoriaJugadorService _categoriaService;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        //private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        //private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";
        //private readonly string mirutaequiposplantilla = @"wwwroot\archivos\Equipos\Plantilla";
        private string carpetaLocal = "Equipos";

        private readonly IArchivosService _ArchivosRepository;
        private readonly ITemporadaService temporadaService;
        private readonly IClubService clubService;
        public EquipoService(IClubService clubService,IGenericRepository<Equipo> equipoRepository, IGenericRepository<Club> clubRepository,  IMapper mapper, ICategoriaJugadorService _categoriaService, IArchivosService _ArchivosRepository, ITemporadaService temporadaService)
        {
            _equipoRepository = equipoRepository;
            this.clubService = clubService;
            // _clubRepository = clubRepository;
            this._categoriaService = _categoriaService;
            _mapper = mapper;
            this._ArchivosRepository = _ArchivosRepository;
            this.temporadaService = temporadaService;
        }

        public async Task<EquipoDTO> Crear(EquipoDTO modelo)
        {
            try
            {

                var temporal = modelo.ToModel();
                TemporadaDTOSmall temporada= await this.temporadaService.TemporadaActiva();
                temporal.Temporada = temporada.Id;
                temporal.FechaModificacion = DateTime.Now;
                temporal.FechaModificacion = DateTime.Now;
                temporal.Principal = false;
                string filename = "";
                string extension = "";
                string hasha = modelo.idCategoria.ToString() + modelo.IdEquipo.ToString() + modelo.IdClub.ToString() + DateTime.Now;
                string name = hasha.GetHashCode().ToString();
                if (modelo.imagen != null && modelo.IdClub == _Miclub)
                {
                    extension = Path.GetExtension(modelo.imagen.FileName);
                    filename = name + extension;
                    temporal.Foto = filename;
                }
                else {
                    filename = "equipo_default.jpeg";
                    temporal.Foto = filename;
                    

                  
                }
                Equipo equipoCreado= await _equipoRepository.Crear(temporal);


                if (equipoCreado.IdEquipo == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el equipo");
                }

                try
                {
                    // Copia el archivo, sobrescribiendo si ya existe
                    string ruta = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);

                    string carpetaAlbum = equipoCreado.IdEquipo + "\\";
                    string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                    string filepath = Path.Combine(ruta,carpetaAlbum, filename);

                    string carpeta = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);
                    if (!Directory.Exists(Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal)))
                    {
                        Directory.CreateDirectory(Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal));
                    }

                    if (!Directory.Exists(Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal, equipoCreado.IdEquipo.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal, equipoCreado.IdEquipo.ToString()));
                    }

                    File.Copy(this.rutaServidor + filename, filepath, true);

                    Console.WriteLine("Archivo copiado exitosamente.");
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error al copiar el archivo: " + e.Message);
                }


                if (modelo.imagen != null && modelo.IdClub == _Miclub) {
                    extension = Path.GetExtension(modelo.imagen.FileName);

                    string ruta = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);

                    string carpetaAlbum = modelo.IdEquipo + "\\";
                    string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                    string filepath = Path.Combine(rutaImagen, modelo.Foto);

                    if (!Directory.Exists(rutaServidor))
                    {
                        Directory.CreateDirectory(rutaServidor);
                    }

                    if (!Directory.Exists(rutaServidor + modelo.IdEquipo))
                    {
                        Directory.CreateDirectory(rutaServidor + carpetaAlbum);
                    }

                    


                    try
                    {

                        AccionFile accionFile = new AccionFile();
                        accionFile.accion = "Crear";
                        accionFile.thumbnail = true;
                        accionFile.rutaDesten= filepath;
                        accionFile.file = modelo.imagen;
                        accionFile.rutaDestenThumbnail = name + "_thumbnail" + extension;
                        accionFile.sizeThumbnail = 501;
                        this._ArchivosRepository.Ejecutar(accionFile);
                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException(e.Message);
                    }




                    try
                    {


                        return modelo;
                    }



                    catch (Exception e)
                    {
                        throw new TaskCanceledException(e.Message);
                    }
                }

                if (modelo.TecnicoEquipos.Count != 0) { 
                        
                }

                if (modelo.EquipoJugadores.Count != 0) { 

                }

                EquipoDTO equipo = await MapearObjeto(equipoCreado);

                return equipo;

            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Editar(EquipoRivalDTO modelo)
        {
            try
            {
                var equipoModelo = modelo.ToModel();
                var equipoEncontrado = await _equipoRepository.Obtener(u => u.IdEquipo == modelo.IdEquipo);

                if (equipoEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                equipoEncontrado.Nombre = modelo.Nombre;
                equipoEncontrado.IdClub = modelo.IdClub;
                equipoEncontrado.IdEquipo = modelo.IdEquipo;
                equipoEncontrado.EsActivo = modelo.EsActivo;
                
                equipoEncontrado.IdCategoria = modelo.idCategoria;
                equipoEncontrado.FechaModificacion = DateTime.Now;

                bool respuesta = await _equipoRepository.Editar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar el equipo");
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
                var equipoEncontrado = await _equipoRepository.Obtener(p => p.IdEquipo == id);
                if (equipoEncontrado == null) { throw new Exception("El equipo no existe"); }

                   TemporadaDTO temporada =  await this.temporadaService.Ver((int)equipoEncontrado.Temporada);

                try { 
                        
                
                } catch (Exception ex) { }


                rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);
                string carpetaAlbum = equipoEncontrado.IdEquipo+ "\\";
                string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                string filepath = Path.Combine(rutaImagen, equipoEncontrado.Foto);

                bool respuesta = await _equipoRepository.Eliminar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new Exception("No se pudo Eliminar");
                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }


        public async Task<bool> EliminarMiEquipo(int id)
        {
            try
            {
                var equipoEncontrado = await _equipoRepository.Obtener(p => p.IdEquipo == id);
                if (equipoEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                bool respuesta = await _equipoRepository.Eliminar(equipoEncontrado);
                if (!respuesta)
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



        public async Task<EquipoDTO> BuscarEquipoPricipal() //Buscamos el primer equipo
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                TemporadaDTOSmall temporada = await this.temporadaService.TemporadaActiva();
                var equipo = await _equipoRepository.ObtenerUnModelo(p => p.Principal == true &&  p.IdClub == _Miclub && p.Temporada == temporada.Id && p.EsActivo == true);


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                EquipoDTO listaEquipo = EquipoDTO.EquipoToDTO(equipo);

                try
                {
                    if (equipo == null)
                    {
                        throw new Exception("No hay equipos para dicha consulta");
                    }


                    CategoriaJugadorDTO categoria = await this._categoriaService.Obtener(equipo.IdEquipo);
                    listaEquipo.Categoria = categoria;


                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return listaEquipo;

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<EquipoDTO>> BuscarEquiposCantera()
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                TemporadaDTOSmall temporada = await this.temporadaService.TemporadaActiva();

                var listaEquipos = await _equipoRepository.Consultar(p => p.Principal == false && p.IdClub == _Miclub && p.EsActivo == true && p.Temporada == temporada.Id);

                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                foreach (EquipoDTO equipo in listaEquipos1)
                {

                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                }
                return listaEquipos1;


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                //   return _mapper.Map<List<EquipoDTO>>(listaEquipos.ToList());

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<EquipoDTO>> BuscarEquiposCantera(int temporada)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var listaEquipos = await _equipoRepository.Consultar(p => p.Principal == false && p.IdClub == _Miclub && p.EsActivo == true && p.Temporada == temporada);

                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                foreach (EquipoDTO equipo in listaEquipos1)
                {

                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                }
                return listaEquipos1;


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                //   return _mapper.Map<List<EquipoDTO>>(listaEquipos.ToList());

            }
            catch
            {
                throw;

            }
        }




        public async Task<EquipoDTO> BuscarEquipo(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var equipo = await _equipoRepository.ObtenerUnModelo(p => p.IdEquipo == id);


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                EquipoDTO listaEquipo = await MapearObjeto(equipo);

                try
                {
                    if (equipo == null)
                    {
                        throw new Exception("No hay equipos para dicha consulta");
                    }

                   ClubDTO club = await this.clubService.VerClub(equipo.IdClub);
                    CategoriaJugadorDTO categoria = await this._categoriaService.Obtener(equipo.IdCategoria);
                    listaEquipo.Categoria = categoria;
                    listaEquipo.logo = club.Foto;


                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return listaEquipo;

            }
            catch
            {
                throw;

            }
        }

        public async Task<EquipoDTO> BuscarEquipoFront(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                TemporadaDTOSmall temporada = await this.temporadaService.TemporadaActiva();
                var equipo = await _equipoRepository.ObtenerUnModelo(p => p.IdEquipo == id && p.EsActivo == true && p.Temporada == temporada.Id  );


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                EquipoDTO listaEquipo = await MapearMiObjeto(equipo);

                try
                {
                    if (equipo == null)
                    {
                        throw new Exception("No hay equipos para dicha consulta");
                    }


                    CategoriaJugadorDTO categoria = await this._categoriaService.Obtener(equipo.IdCategoria);
                    listaEquipo.Categoria = categoria;


                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return listaEquipo;

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<EquipoDTO>> Lista()
        {
            try
            {
                var listaEquipos = await _equipoRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return await this.MapearMisListaEquipos(listaEquipos.ToList());

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<EquipoDTO>> ListaMisEquipos()
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                TemporadaDTOSmall temporada0 = await this.temporadaService.TemporadaActiva();
                var listaEquipos = await _equipoRepository.Consultar(p => p.IdClub == _Miclub && p.Temporada == temporada0.Id && p.Principal == false);

                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                
                return listaEquipos1;


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
             //   return _mapper.Map<List<EquipoDTO>>(listaEquipos.ToList());

            }
            catch
            {
                throw;

            }
        }

        public async Task<EquipoDTO> MapearObjeto(Equipo item)
        {

            EquipoDTO equipo = EquipoDTO.EquipoToDTO(item);
            equipo.temporada = await this.temporadaService.Ver((int)item.Temporada);
            equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
            return equipo;
        }
        //para mis equipos
        public async Task<EquipoDTO> MapearMiObjeto(Equipo item)
        {

            EquipoDTO equipo = EquipoDTO.EquipoToDTO(item);
            equipo.temporada = await this.temporadaService.Ver((int)item.Temporada);
            equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);

            return equipo;
        }

        public async Task<List<EquipoDTO>> MapearMisListaEquipos(List<Equipo> lista)
        {
            List<EquipoDTO> equipos = new List<EquipoDTO>();
            foreach (var item in lista)
            {
                EquipoDTO equipo = await this.MapearObjeto(item);
                equipos.Add(equipo);

            }
            return equipos;
        }

        

            public async Task<List<EquipoDTO>> ListaEquiposClub(int id)
        {
            try
            {
               // var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var listaEquipos = await _equipoRepository.Consultar(u => u.IdClub == id);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();



                List<EquipoDTO> listaEquipos1 = new List<EquipoDTO>();
                foreach (var item in listaEquipos) {
                    EquipoDTO equipo = EquipoDTO.EquipoToDTO(item);
                    equipo.temporada = await this.temporadaService.Ver((int)item.Temporada);
                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                    listaEquipos1.Add(equipo);  

                    
                }
                return listaEquipos1;

            }
            catch
            {
                throw;

            }
        }




        public async Task<List<EquipoDTO>> ListaEquiposMiClub( int temporada)
        {
            try
            {
                // var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var listaEquipos = await _equipoRepository.Consultar(u => u.IdClub == this._Miclub && u.Temporada == temporada);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();



                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                foreach (EquipoDTO equipo in listaEquipos1)
                {

                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                }
                return listaEquipos1;

            }
            catch
            {
                throw;

            }
        }
        public async Task<List<EquipoDTO>> ListaEquiposClub(int id, int temporada)
        {
            try
            {
                // var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var listaEquipos = await _equipoRepository.Consultar(u => u.IdClub == id && u.Temporada == temporada);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();



                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                foreach (EquipoDTO equipo in listaEquipos1)
                {

                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                }
                return listaEquipos1;

            }
            catch
            {
                throw;

            }
        }
        public async Task<EquipoDTO> CrearMiEquipo(EquipoDTO modelo)
        {
            try
            {
                string extension = Path.GetExtension(modelo.imagen.FileName);
                string filename = modelo.Nombre + extension;
                modelo.Foto = filename;
                modelo.IdClub = _Miclub;
                //
                //var clubEncontrado = await _clubRepository.Obtener(p => p.IdClub ==_Miclub);
                //if (clubEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                TemporadaDTOSmall temporada = await this.temporadaService.TemporadaActiva();
                string carpetaequipo = rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal); ;
                

                var equipoCreado = await _equipoRepository.Crear(_mapper.Map<Equipo>(modelo));

                if (!Directory.Exists(carpetaequipo))
                {
                    Directory.CreateDirectory(carpetaequipo);
                }

                if (!Directory.Exists(carpetaequipo))
                {
                    Directory.CreateDirectory(carpetaequipo);
                }
                carpetaequipo = carpetaequipo +"\\"+ equipoCreado.IdEquipo + " " + equipoCreado.Nombre;

                if (!Directory.Exists(carpetaequipo))
                {
                    Directory.CreateDirectory(carpetaequipo);
                }

                if (modelo.imagen!= null) {
                    try
                    {
                       
                        carpetaequipo = carpetaequipo + "\\";
                        using (FileStream newFile = System.IO.File.Create(carpetaequipo + filename))
                        {
                            modelo.imagen.CopyTo(newFile);
                            newFile.Flush();

                        }

                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se pudo Crear el archivo");
                    }
                }

                if (equipoCreado.IdEquipo == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el equipo");
                }

                return _mapper.Map<EquipoDTO>(equipoCreado);

            }
            catch
            {
                throw;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<EquipoDTO>  CrearFotoEquipo(EquipoDTO equipo) {
            Equipo equipo1 = await this._equipoRepository.ObtenerUnModelo(p => p.IdEquipo == equipo.IdEquipo);

             
            if (equipo == null) {
                throw new Exception("No se pudo Crear la foto del equipo, el equipo no existe");
            }

            if (equipo.imagen == null) {
                throw new Exception("Elija primero la foto a crear");
            }

            string filename = "";
            string extension = "";
            
            string hasha = equipo.idCategoria.ToString() + equipo.IdEquipo.ToString() + equipo.IdClub.ToString() + DateTime.Now;
            string name = hasha.GetHashCode().ToString();
            extension = Path.GetExtension(equipo.imagen.FileName); 
            filename = name+extension;
            TemporadaDTOSmall temporada = equipo.temporada;
            string ruta = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal);

            string carpetaAlbum = equipo.IdEquipo + "\\";
            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
            string filepath = Path.Combine(rutaImagen, filename);
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            ruta = Path.Combine(ruta, carpetaAlbum);
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }


           // foto.IdEquipo = jugador.Equipo;
            

           

            
            
            //equipo.ThumbnailImageSrc = name + "_thumbnail.jpg";
            // equipo1.= filename;
            string equipo0 = equipo.IdEquipo + "\\";
            

            try
            {


                try
                {

                    AccionFile accionFile = new AccionFile();
                    accionFile.accion = "Actualizar";
                    accionFile.thumbnail = false;
                    rutaImagen = Path.Combine(ruta, name+extension);
                    
                    accionFile.rutaDesten = rutaImagen;
                    Path.GetExtension(equipo.imagen.FileName);
                    accionFile.rutaOrigen = ruta + equipo1.Foto;
                    accionFile.file = equipo.imagen;
                   
                    //accionFile.rutaDestenThumbnail = Path.Combine(ruta, name + "_thumbnail" + extension);
                    //accionFile.sizeThumbnail = 500;
                    this._ArchivosRepository.Ejecutar(accionFile);
                }
                catch (Exception e)
                {
                    throw new TaskCanceledException(e.Message);
                }


            }
            catch (Exception e)
            {
                throw new TaskCanceledException("No se pudo Crear el archivo");
            }

            equipo1.Foto = filename;
            equipo1.FechaModificacion = DateTime.Now;

            bool respuesta = await _equipoRepository.Editar(equipo1);
            if (!respuesta)
            {
                throw new TaskCanceledException("No se pudo editar");
            }



            return  await this.BuscarEquipo(equipo.IdEquipo) ;

        }

        public Task<List<EquipoRivalDTO>> ListaRival()
        {
            throw new NotImplementedException();
        }

        public Task<EquipoRivalDTO> Crear(EquipoRivalDTO modelo)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idClub"></param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public async Task<List<EquipoDTO>> ListaEquiposClubCategoria(int idClub, int idCategoria)
        {
            try
            {
                // var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var listaEquipos = await _equipoRepository.Consultar(u => u.IdClub == idClub && u.IdCategoria == idCategoria && u.EsActivo == true);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();



                List<EquipoDTO> listaEquipos1 = await this.MapearMisListaEquipos(listaEquipos.ToList());

                foreach (EquipoDTO equipo in listaEquipos1)
                {

                    equipo.Categoria = await this._categoriaService.Obtener(equipo.idCategoria);
                }
                return listaEquipos1;

            }
            catch
            {
                throw;

            }
        }

      
    }
}
