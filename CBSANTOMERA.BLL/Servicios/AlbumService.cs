
using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
//using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Microsoft.AspNetCore.Http;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using System.ClientModel.Primitives;
using AutoMapper.Internal;

namespace CBSANTOMERA.BLL.Servicios
{
    public class AlbumService : IAlbumService
    {

        private readonly IGenericRepository<Albume> _albumRepository;
        private readonly IFotoAlbumService _FotosAlbumRepository;
        private readonly IArchivosService _ArchivosRepository;
        private readonly ITemporadaService _TemporadaRepository;

        private readonly IMapper _mapper;
        private  string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = "Albumes";


        public AlbumService(IGenericRepository<Albume> productoRepository, IMapper mapper, IFotoAlbumService _FotosAlbumRepository, IArchivosService _ArchivosRepository, ITemporadaService temporadaService)
        {
            this._ArchivosRepository = _ArchivosRepository;
            this._albumRepository = productoRepository;
            this._mapper = mapper;
            this._FotosAlbumRepository = _FotosAlbumRepository;
            this._TemporadaRepository = temporadaService;
            


        }

        public async Task<AlbumDTO> Crear(AlbumDTO modelo)
        {
            var albumCreado = new Albume();
            string filename = "";
            string extension = "";
            string hasha = modelo.IdAlbum.ToString() + DateTime.Now;
            string name = hasha.GetHashCode().ToString();

            modelo.EsActivo = false;
            modelo.Fijar = false;

            try {
               TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.TemporadaActiva();


                if (temporada.Id == 0) {
                    throw new Exception("No se ha podido crear el album, porque no hay ninguna temporada activa.");
                }
               
                rutaServidor = Path.Combine(rutaServidor, modelo.Temporada.Nombre,this.carpetaLocal);
                modelo.Fecha = DateTime.Now;
                if (modelo.imagen == null)
                {
                    throw new Exception("El album debe tener una imagen de portada.");
                }
                extension = Path.GetExtension(modelo.imagen.FileName);
                filename = name + extension;
                modelo.Portada = filename;
                albumCreado = await _albumRepository.Crear(modelo.ToModelo());
            }

            catch (Exception e)
            {
                throw new Exception("No se ha podido crear el album en la BBDD", e);
            }
           
            
           

           

            if (albumCreado.IdAlbum == 0)
            {
                throw new TaskCanceledException("No se pudo crear el album "+albumCreado.Nombre);


            }
            string carpetaAlbum = albumCreado.IdAlbum + "\\";

            if (!Directory.Exists(rutaServidor))
            {
                Directory.CreateDirectory(rutaServidor);
            }

            if (!Directory.Exists(Path.Combine(rutaServidor, carpetaAlbum)))
            {
                Directory.CreateDirectory(Path.Combine(rutaServidor, carpetaAlbum));
            }

            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);

            AccionFile accion = new AccionFile();
            accion.accion = "Guardar";
           
            accion.rutaDesten =  name+extension;
            modelo.Portada = name + extension;

            accion.thumbnail = true;
            accion.file = modelo.imagen;
            accion.rutaDesten = rutaImagen+name + extension;
            accion.rutaDestenThumbnail = rutaImagen + name + "_thumbnail" + extension;
            modelo.Portada = name + "_thumbnail" + extension;
            
            this._ArchivosRepository.Ejecutar(accion);
            /*try
            {

                using (FileStream newFile = System.IO.File.Create(rutaImagen + "portada10" + extension))
                {
                    modelo.imagen.CopyTo(newFile);


                    albumCreado.Portada = filename;
                    albumCreado.Nombre = modelo.Nombre;
                    newFile.Flush();
                   

                }
                this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + filename, "jpg");
                if (File.Exists(rutaImagen + "portada10" + extension)) //SI la ruta existe
                {
                    try
                    {
                        File.Delete(rutaImagen + "portada10" + extension); //Se borra la imagen del directorio

                    }
                    catch (Exception ex)
                    {
                        //Do something
                    }

                }
            }
            catch (Exception e)
            {
                throw new TaskCanceledException(e.Message);
            }*/
            



            try
            {
                AlbumDTO album = AlbumDTO.ToDto(albumCreado);
                album.Temporada = modelo.Temporada;
                album.IdAlbum = albumCreado.IdAlbum;
                album.Portada = modelo.Portada;
                
                album   = await _FotosAlbumRepository.CrearFoto(album, modelo.fotos);
                return album;
            }


            
            catch (Exception e)
            {
                throw new TaskCanceledException(e.Message);
            }

           




           




        }

        public async Task<bool> Editar(AlbumDTO modelo)
        {

            string filename = "";
            string extension = "";
            string hasha = modelo.IdAlbum.ToString() + DateTime.Now;
            string name = hasha.GetHashCode().ToString();
            if (modelo.imagen != null)
            {
                extension = Path.GetExtension(modelo.imagen.FileName);
                modelo.Portada = name + extension;
            }
                
            
            try
            {
               
                TemporadaDTO temporada = (TemporadaDTO)await this._TemporadaRepository.Ver(modelo.Temporada.Id);


                if (temporada.Id == 0)
                {
                    throw new Exception("No se ha podido crear el album, porque no hay ninguna temporada activa.");
                }

                Albume album = await _albumRepository.ObtenerUnModelo(a => a.IdAlbum == modelo.IdAlbum);

                if (album.IdAlbum== 0)
                {
                    throw new Exception("El album a actualizar no existe en el BBDD");
                }

                rutaServidor = Path.Combine(rutaServidor, modelo.Temporada.Nombre, this.carpetaLocal);

                //var albumEncontrado = await _albumRepository.Obtener(u => u.IdAlbum == modelo.IdAlbum);
                
                var res = await _albumRepository.Editar(modelo.ToModelo());



                if (res == true)
                {

                    if (modelo.imagen != null)
                    {


                        string carpetaAlbum = modelo.IdAlbum + "\\";
                        string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                        string filepath = Path.Combine(rutaImagen, modelo.Portada);
                        extension = Path.GetExtension(modelo.imagen.FileName);


                        try { 
                            AccionFile accion = new AccionFile();
                            accion.accion = "Update";
                            accion.thumbnail = false;
                            accion.rutaOrigen = rutaImagen +album.Portada;
                            
                            accion.rutaDesten = rutaImagen + name + extension;
                            //accion.rutaDestenThumbnail = rutaImagen + name + "_thumbnail" + extension; ;
                           
                            //accion.sizeThumbnail = 600;
                            accion.file = modelo.imagen;

                            this._ArchivosRepository.Ejecutar(accion);
                        } catch(Exception ex) { throw new Exception(ex.Message); }


                    }

                    //Procedemos a modificar las fotos, obtenemos todas la fotos del IDAlbum y la lista de las fotos a mantenener en la BBDD, comprobamos si la lista de fotos de la BBDD y la lista de fotos del cliente no se ha modificado.
                    
                            List<FotoAlbumDTO> listaFotos = await _FotosAlbumRepository.VerAlbum(modelo.IdAlbum);

                    if (listaFotos.Count != modelo.FotosAlbum.Count)
                    {
                        var encontrado = false;
                        var rest = false;
                        for (int i = 0; i < listaFotos.Count; i++)
                        {
                            encontrado = false;
                            for (int j = 0; j < modelo.FotosAlbum.Count; j++)
                            {

                                if (listaFotos[i].IdFoto == modelo.FotosAlbum[j].IdFoto)
                                {
                                    encontrado = true;
                                    continue;
                                    
                                }


                            }

                            if (encontrado == false)
                            {
                                rest = await _FotosAlbumRepository.Eliminar(listaFotos[i].IdFoto);

                            }

;
                        }
                    }
                   
                    //Procedemos a añadir las nuevas fotos
                            try
                            {
                                        if (modelo.fotos != null) {

                                                 AlbumDTO album0  = await _FotosAlbumRepository.CrearFoto(modelo, modelo.fotos);
                                        }
                              

                            }
                            catch (Exception ex) { }




                        
                    

                }
                else {
                    return false;
                }

                return true;

            }

            catch (Exception e) { throw; }
        }

        

        //OBTIENE TODOS LOS DATOS DE UN ALBUM
        public async Task<AlbumDTO> VerAlbum(int id)
        {
            try {
                
                var albumEncontrado = await _albumRepository.Obtener(u => u.IdAlbum == id);
                AlbumDTO album =AlbumDTO.ToDto(albumEncontrado);

                album.Temporada = await this._TemporadaRepository.Ver((int)albumEncontrado.Temporada);
               album.FotosAlbum = await _FotosAlbumRepository.VerAlbum(id);
                return album;
              
                             

                

            } catch (Exception e) { throw; }
        }

        //OBTIENE UN ALBUM ACTIVO  CON DATOS CORTOS SE UTILIZA EN ADMIN -> CREAR Y EDITAR NOTICIAS
        public async Task<AlbumDTOSmall> VerAlbumSmall(int id)
        {
            try
            {

                var albumEncontrado = await _albumRepository.Obtener(u => u.IdAlbum == id && u.EsActivo == true);
                if (albumEncontrado == null) {
                    return null;
                    throw new Exception("El album no se encuentra encuentra en la BBDD ó tiene estado No activado");
                }
                AlbumDTOSmall album =(AlbumDTOSmall) AlbumDTOSmall.ToDto(albumEncontrado);

                album.Temporada = await this._TemporadaRepository.Ver((int)albumEncontrado.Temporada);
                //album.FotosAlbum = await _FotosAlbumRepository.VerAlbum(id);
                return album;





            }
            catch (Exception e) { throw; }
        }

        //ELIMINAMOS UN ALBUM POR EL ID DE ALBUM
        public async Task<bool> EliminarAlbum(int id)
        {
            try
            {
               
                AlbumDTO album = await this.VerAlbum(id);

                var albumEncontrado = await _albumRepository.Obtener(u => u.IdAlbum == id);
                album.Temporada = await this._TemporadaRepository.Ver((int)albumEncontrado.Temporada);

                try {
                    if(!await _FotosAlbumRepository.EliminarFotosAlbum(album)){
                        return false;
                    }
                   


                    rutaServidor = Path.Combine(rutaServidor, album.Temporada.Nombre, this.carpetaLocal);
                    string carpetaAlbum = album.IdAlbum + "\\";
                    string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                 
                    string filepath = Path.Combine(rutaImagen, album.Portada);
                   
                    
                        try
                        {
                        AccionFile accion = new AccionFile();
                        accion.accion = "Eliminar";
                        accion.rutaOrigen = rutaImagen+album.Portada;
                        accion.thumbnail = false;
                        this._ArchivosRepository.Ejecutar(accion);

                        }
                        catch (Exception ex)
                        {
                            //Do something
                        }
                    

                   



                    if (await _albumRepository.Eliminar(albumEncontrado)) {

                        //rutaServidor = Path.Combine(rutaServidor, album.Temporada.Nombre, this.carpetaLocal);
                        string dirAlbum = album.IdAlbum + "";
                        string rutaalbum = Path.Combine(rutaServidor, dirAlbum);
                        if (Directory.Exists(rutaalbum)) //SI la ruta existe
                        {
                            try
                            {
                                Directory.Delete(rutaalbum, true); //Se borra el directorio

                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Error a la hora de eliminar la estructura del album.");
                            }
                        }
                        return true;
                    }
                   



                }
                catch (Exception ex) { 
                
                
                }



                






            }
            catch (Exception e) { throw; }

            return false;
        }


        public void Dimension(AlbumDTO modelo){

            string carpetaAlbum = modelo.IdAlbum + "\\";
            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
            string filepath = Path.Combine(rutaImagen, modelo.Portada);
            this._ArchivosRepository.RedimensionarImagen120(filepath, "portada150.png", "jpg");
        
        }

        //OBTIENE EL LISTADO COMPLETO DE TODOS LOS DATOS DE ALBUMES ORDENADO POR FECHA (NO UTILIZADO)
        public async Task<List<AlbumDTO>> Lista()
        {
            try
            {
                List<AlbumDTO> albumes = new List<AlbumDTO>();
                var listaAlbumes = await _albumRepository.Consultar();

                listaAlbumes = listaAlbumes.OrderByDescending(d => d.Fecha);

                foreach(Albume item in listaAlbumes) {

                    AlbumDTO album = AlbumDTO.ToDto(item);
                    album.Temporada= await this._TemporadaRepository.Ver((int)item.Temporada);
                    List<FotoAlbumDTO> albumes1 = await this._FotosAlbumRepository.VerAlbum(album.IdAlbum);
                        album.Numero_Fotos = albumes1.Count;
                    
                    albumes.Add(album);
                    
                }

                

                /*for (int i= 0; i< albumes.Count; i++) {
                    albumes[i].FotosAlbum = await _FotosAlbumRepository.VerAlbum(albumes[i].IdAlbum);
                }*/

                return albumes;

            }
            catch
            {
                throw;

            }
        }
        //MUESTRA LA LISTA DE ALBUMSMALL POR EL NOMBRE DE TEMPORADA AMDMIN -> EDITAR NOTICIAS O CREAR
        public async Task<List<AlbumDTOSmall>> Lista(string nombre)
        {
            try
            {
                TemporadaDTOSmall temporada = await this._TemporadaRepository.ObtenerTemporadaPorNombre(nombre);
                List<AlbumDTOSmall> albumes = new List<AlbumDTOSmall>();
                var listaAlbumes = await _albumRepository.Consultar(t =>t.Temporada == temporada.Id && t.EsActivo == true );

                listaAlbumes = listaAlbumes.OrderByDescending(d => d.Fecha);

                foreach (Albume item in listaAlbumes)
                {

                    AlbumDTOSmall album = AlbumDTOSmall.ToDto(item);
                    album.Temporada = await this._TemporadaRepository.Ver((int)item.Temporada);
                    List<FotoAlbumDTO> albumes1 = await this._FotosAlbumRepository.VerAlbum(album.IdAlbum);
                    album.Numero_Fotos = albumes1.Count;

                    albumes.Add(album);

                }



                /*for (int i= 0; i< albumes.Count; i++) {
                    albumes[i].FotosAlbum = await _FotosAlbumRepository.VerAlbum(albumes[i].IdAlbum);
                }*/

                return albumes;

            }
            catch
            {
                throw;

            }
        }

        //MUESTRA TODOS LOS ALBUMES TANTO ACTIVO COMO NO ACTIVO Y FIJADO O NO, CON TODOS LOS DATOS SIN LAS FOTOS ALBUMES
        public async Task<List<AlbumDTO>> Lista_Full(string nombre)
        {
            try
            {
                TemporadaDTO temporada = await this._TemporadaRepository.ObtenerTemporadaPorNombre1(nombre);
                List<AlbumDTO> albumes = new List<AlbumDTO>();
                var listaAlbumes = await _albumRepository.Consultar(t => t.Temporada == temporada.Id);

                listaAlbumes = listaAlbumes.OrderByDescending(d => d.Fecha);

                foreach (Albume item in listaAlbumes)
                {

                    AlbumDTO album = AlbumDTO.ToDto(item);
                    album.Temporada = await this._TemporadaRepository.Ver((int)item.Temporada);
                    List<FotoAlbumDTO> albumes1 = await this._FotosAlbumRepository.VerAlbum(album.IdAlbum);
                    album.Numero_Fotos = albumes1.Count;

                    albumes.Add(album);

                }



                /*for (int i= 0; i< albumes.Count; i++) {
                    albumes[i].FotosAlbum = await _FotosAlbumRepository.VerAlbum(albumes[i].IdAlbum);
                }*/

                return albumes;

            }
            catch
            {
                throw;

            }
        }




        //OBTIENE EL LISTADO DE LOS ALBUMES, QUE TENGAN TEMPORADA ACTIVA ORDENADO POR FECHA VISUAL-> MULTIMEDIA
        public async Task<List<AlbumDTOSmall>> ListaSmall()
        {
            try
            {
                TemporadaDTOSmall temporada = await this._TemporadaRepository.TemporadaActiva();
                var listaAlbumes = await _albumRepository.Consultar(t => t.Temporada == temporada.Id);

                listaAlbumes= listaAlbumes.OrderByDescending(d => d.Fecha);
                List<AlbumDTOSmall> albumes = new List<AlbumDTOSmall>();
                foreach (Albume item in listaAlbumes)
                {

                    AlbumDTOSmall album = AlbumDTOSmall.ToDto(item);
                    album.Temporada = await this._TemporadaRepository.Ver((int)item.Temporada);
                    

                    albumes.Add(album);

                }
                

                

                return albumes;

            }
            catch
            { 
                throw;

            }
        }

        //OBTIENE EL LISTADO DE LOS ALBUMES, QUE TENGAN TEMPORADA ACTIVA ORDENADO POR FECHA VISUAL-> MULTIMEDIA
        public async Task<List<AlbumDTOSmall>> ListaSmallFijo()
        {
            try
            {
                TemporadaDTOSmall temporada = await this._TemporadaRepository.TemporadaActiva();
                var listaAlbumes = await _albumRepository.Consultar(t => t.Temporada == temporada.Id && t.EsActivo == true && t.Fijar ==true);

                listaAlbumes = listaAlbumes.OrderByDescending(d => d.Fecha);
                List<AlbumDTOSmall> albumes = new List<AlbumDTOSmall>();
                foreach (Albume item in listaAlbumes)
                {

                    AlbumDTOSmall album = AlbumDTOSmall.ToDto(item);
                    //album.Temporada = await this._TemporadaRepository.Ver((int)item.Temporada);


                    albumes.Add(album);

                }




                return albumes;

            }
            catch
            {
                throw;

            }
        }
    }

   
}
