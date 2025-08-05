using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CBSANTOMERA.BLL.Servicios
{
    public class NoticiaService : INoticiaService
    {


        private readonly IGenericRepository<Noticia> _noticiaRepository;
        private readonly IGenericRepository<TipoNoticium> _TipoNoticiaRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Noticias\";

        private readonly ITemporadaService _TemporadaRepository;

        private readonly IArchivosService _ArchivosRepository;
        private readonly IAlbumService _AlbumRepository;

        public NoticiaService(IGenericRepository<Noticia> noticiaRepository, IMapper mapper, IAlbumService _AlbumRepository, IArchivosService _ArchivosRepository, ITemporadaService temporadaRepository, IGenericRepository<TipoNoticium> tipoNoticiaRepository)
        {
            _noticiaRepository = noticiaRepository;
            _mapper = mapper;
            this._AlbumRepository = _AlbumRepository;
            this._ArchivosRepository = _ArchivosRepository;
            _TemporadaRepository = temporadaRepository;
            _TipoNoticiaRepository = tipoNoticiaRepository;
        }

        public async Task  <NoticiasDTO> Crear(NoticiasDTO noticia)

            
        {
            var noticiaCreado = new Noticia() ;
            string hasha = noticia.ToString() + DateTime.Now;
            string name = hasha.GetHashCode().ToString();
            string filename = "";
            string extension = "";
            string filenamethumbnail = "";

            try
            {
                if (noticia.imagen == null)
                {

                    throw new Exception("La noticia no tiene portada, selecciona una portada");
                }

            }
            catch (Exception ex) { throw new Exception("La noticia no tiene portada, selecciona una portada", ex); }

            try {
                
               
                noticiaCreado.Fecha = DateTime.Now;
                   
                extension = Path.GetExtension(noticia.imagen.FileName);
                filename = name + extension; ;
                filenamethumbnail = name + "_thumbnail" + extension;
                noticiaCreado.Portada = filename;

                if (noticia.Album != null)
                {

                    noticiaCreado.Album = (int)noticia.Album.IdAlbum;
                   
                }

                noticiaCreado.ThumbnailImageSrc = filenamethumbnail;
               
                noticiaCreado.Titulo = noticia.Titulo;
                noticiaCreado.Subtitulo = noticia.Subtitulo;

                
                noticiaCreado.TipoNoticia = noticia.TipoNoticia;
                

                noticiaCreado.Contenido = noticia.Contenido;
                noticiaCreado.Nuevo = true;
                noticiaCreado.EsActivo = false;
                noticiaCreado.Fijar = false;
                noticiaCreado.FechaModificacion = DateTime.Now;
                var noticias0 = await _noticiaRepository.Crear(noticiaCreado);
                noticiaCreado.IdNoticia = noticias0.IdNoticia;


            } catch (Exception e) {
                throw new Exception("No se ha podido crear la noticia en la BBDD", e);
            }

            string carpetaAlbum = noticiaCreado.IdNoticia + "\\";
            if (noticiaCreado.IdNoticia == 0)
            {
                throw new TaskCanceledException("No se pudo crear la noticia" + noticiaCreado.Titulo);

            }

            if (!Directory.Exists(rutaServidor))
            {
                Directory.CreateDirectory(rutaServidor);
            }

            if (!Directory.Exists(rutaServidor + carpetaAlbum))
            {
                Directory.CreateDirectory(rutaServidor + carpetaAlbum);
            }

            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);

            AccionFile accion = new AccionFile();
            accion.accion = "Guardar";
            accion.rutaDesten = rutaImagen+filename;
            accion.thumbnail = true;
            accion.file = noticia.imagen;
            accion.rutaDestenThumbnail = rutaImagen + filenamethumbnail;
            accion.sizeThumbnail = 500;
            this._ArchivosRepository.Ejecutar(accion);



            /*try
            {

                using (FileStream newFile = System.IO.File.Create(rutaImagen + "portada10" + extension))
                {
                    noticia.imagen.CopyTo(newFile);


                    noticiaCreado.Portada = filename;
                   
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


            return NoticiasDTO.ToDto(noticiaCreado) ;
        }





       


public async  Task<bool> Editar(NoticiasDTO modelo)
        {
            NoticiasDTO noticia0 = await this.VerNoticia(modelo.IdNoticia);
            if (noticia0 == null) {
                return false;
            }
            
            Noticia noticia = modelo.ToModelo();
            if (modelo.Album == null || modelo.Album.IdAlbum == 0 || modelo.Album.IdAlbum == null)
            {
                noticia.Album = null;
            }
            else {
                noticia.Album = modelo.Album.IdAlbum;
            }
            
            noticia.FechaModificacion = DateTime.Now;
            noticia.Fecha = noticia0.Fecha;
            noticia.Portada = noticia0.Portada;
            noticia.Nuevo = noticia0.Nuevo;
            noticia.ThumbnailImageSrc = noticia0.ThumbnailImageSrc;

            if (modelo.imagen != null) {

                string carpetaAlbum = modelo.IdNoticia + "\\";
                if (!Directory.Exists(rutaServidor))
                {
                    Directory.CreateDirectory(rutaServidor);
                }

                if (!Directory.Exists(rutaServidor + carpetaAlbum))
                {
                    Directory.CreateDirectory(rutaServidor + carpetaAlbum);
                }

                string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                AccionFile accionFile = new AccionFile();
                accionFile.accion = "Actualizar";
                accionFile.thumbnail = true;
                accionFile.rutaOrigen = rutaImagen+noticia0.Portada;
                accionFile.rutaDesten = rutaImagen + noticia0.Portada;
                accionFile.rutaDestenThumbnail = rutaImagen + noticia0.ThumbnailImageSrc;
                accionFile.rutaOrigenThumbnail = rutaImagen + noticia0.ThumbnailImageSrc;
                accionFile.file = modelo.imagen;
                accionFile.sizeThumbnail = 500;
                this._ArchivosRepository.Ejecutar(accionFile);  

            }

            

            var res =  await _noticiaRepository.Editar(noticia);
            return res;
        }

        public async Task<bool> Eliminar(int id)
        {
            NoticiasDTO noticia = new NoticiasDTO();
            noticia = await this.VerNoticia(id);
            if (noticia.IdNoticia < 0) {
                return false;
            }

            AccionFile accion = new AccionFile();
            accion.accion = "Eliminar";
            string carpetaAlbum = noticia.IdNoticia + "\\";
            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
            accion.rutaOrigen = rutaImagen+noticia.Portada;
            accion.rutaOrigenThumbnail = rutaImagen + noticia.ThumbnailImageSrc;
            accion.thumbnail = true;
            
            if ( this._ArchivosRepository.Ejecutar(accion)) {

                string ruta= Path.Combine(rutaServidor, carpetaAlbum);
                if (Directory.Exists(ruta)) //SI la ruta existe
                {
                    try
                    {
                        Directory.Delete(ruta); //Se borra la imagen del directorio

                    }
                    catch (Exception ex)
                    {
                        //Do something
                    }
                }
                
            }
            Noticia not = noticia.ToModelo();
            return await this._noticiaRepository.Eliminar(not); 
        }

        public Task<bool> EliminarAlbum(NoticiasDTO noticia)
        {
            throw new NotImplementedException();
        }

        public async Task<List<NoticiasDTO>> Lista()
        {
            try
            {
                var listaNoticias = await _noticiaRepository.Consultar();
                listaNoticias = listaNoticias.OrderByDescending(m=> m.Fecha);
                List<NoticiasDTO>lista = new List<NoticiasDTO>();

                foreach (var item in listaNoticias)
                {

                    if (item.Nuevo == null)
                    {
                        TimeSpan tiempoDiferencia = DateTime.Now - item.Fecha;
                        var dias = tiempoDiferencia.Days;
                        if (dias<3)
                        {
                            item.Nuevo = true;
                            await _noticiaRepository.Editar(item);

                        }
                        else
                        {
                            item.Nuevo = false;
                            await _noticiaRepository.Editar(item);
                        }
                    }

                    NoticiasDTO noticia = NoticiasDTO.ToDto(item);
                    if (item.Album != null && item.Album != 0)
                    {
                        noticia.Album = new AlbumDTO();
                        noticia.Album.IdAlbum =(int) item.Album;
                        //noticia.Album = await this._AlbumRepository.VerAlbum((int)item.Album);
                    }
                    lista.Add(noticia);
                }


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return lista;

            }
            catch
            {
                throw;

            }
        }



        public async Task<List<NoticiasDTO>> Lista_Noticias_Activas()//para la parte usuario
        {
            try
            {
                var listaNoticias = await _noticiaRepository.Consultar(n=> n.EsActivo == true);
                listaNoticias = listaNoticias.OrderByDescending(m => m.Fecha);
                List<NoticiasDTO> lista = new List<NoticiasDTO>();

                foreach (var item in listaNoticias)
                {

                    if (item.Nuevo == null)
                    {
                        TimeSpan tiempoDiferencia = DateTime.Now - item.Fecha;
                        var dias = tiempoDiferencia.Days;
                        if (dias < 3)
                        {
                            item.Nuevo = true;
                            await _noticiaRepository.Editar(item);

                        }
                        else
                        {
                            item.Nuevo = false;
                            await _noticiaRepository.Editar(item);
                        }
                    }

                    NoticiasDTO noticia = NoticiasDTO.ToDto(item);
                    if (item.Album != null && item.Album != 0)
                    {
                        noticia.Album = new AlbumDTO();
                        noticia.Album.IdAlbum = (int)item.Album;
                        //noticia.Album = await this._AlbumRepository.VerAlbum((int)item.Album);
                    }
                    lista.Add(noticia);
                }


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return lista;

            }
            catch
            {
                throw;

            }
        }





        public async Task<List<NoticiasDTO>> Lista_Noticias_Empresas()
        {
            try
            {
                var listaNoticias = await _noticiaRepository.Consultar(n=> n.TipoNoticia == "Empresa");
                listaNoticias = listaNoticias.OrderByDescending(m => m.Fecha);
                List<NoticiasDTO> lista = new List<NoticiasDTO>();

                foreach (var item in listaNoticias)
                {

                    if (item.Nuevo == null)
                    {
                        TimeSpan tiempoDiferencia = DateTime.Now - item.Fecha;
                        var dias = tiempoDiferencia.Days;
                        if (dias < 3)
                        {
                            item.Nuevo = true;
                            await _noticiaRepository.Editar(item);

                        }
                        else
                        {
                            item.Nuevo = false;
                            await _noticiaRepository.Editar(item);
                        }
                    }

                    NoticiasDTO noticia = NoticiasDTO.ToDto(item);
                    if (item.Album != null && item.Album != 0)
                    {
                        noticia.Album = new AlbumDTO();
                        noticia.Album.IdAlbum = (int)item.Album;
                        //noticia.Album = await this._AlbumRepository.VerAlbum((int)item.Album);
                    }
                    lista.Add(noticia);
                }


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return lista;

            }
            catch
            {
                throw;

            }
        }


        public async Task<List<NoticiasDTOSmall>> ListaInicio()
        {
            try
            {
                var listaNoticias = await _noticiaRepository.Consultar(m=> m.EsActivo == true && m.Fijar == true);
                listaNoticias = listaNoticias.OrderByDescending(m => m.Fecha);
                List<NoticiasDTOSmall> lista = new List<NoticiasDTOSmall>();

                foreach (var item in listaNoticias)
                {

                    if (item.Nuevo == null)
                    {
                        TimeSpan tiempoDiferencia = DateTime.Now - item.Fecha;
                        var dias = tiempoDiferencia.Days;
                        if (dias < 3)
                        {
                            item.Nuevo = true;
                            await _noticiaRepository.Editar(item);

                        }
                        else
                        {
                            item.Nuevo = false;
                            await _noticiaRepository.Editar(item);
                        }
                    }

                    NoticiasDTOSmall noticia = NoticiasDTOSmall.ToDto(item);
                   
                    lista.Add(noticia);
                }


                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return lista;

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<TipoNoticium>> ListaTipo()
        {
            try
            {
                var listaNoticias = await _TipoNoticiaRepository.Consultar();
                listaNoticias = listaNoticias.OrderByDescending(m => m.Id);
                List<TipoNoticium> lista = new List<TipoNoticium>();
                lista = listaNoticias.OrderByDescending(m => m.Id).ToList();





                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return lista;

            }
            catch
            {
                throw;

            }
        }

        public async Task<TipoNoticium> VerTipoNoticia(int id)
        {
            try
            {
                var noticia1 = await this._TipoNoticiaRepository .ObtenerUnModelo((m) => m.Id == id);
                
                if (noticia1 != null )
                {
                    return noticia1;

                }
                else
                {
                    return null;
                }
                

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<NoticiasDTO> VerNoticia(int id)
        {
            try {
                var noticia1 = await this._noticiaRepository.ObtenerUnModelo((m)=> m.IdNoticia == id);
                NoticiasDTO noticia = NoticiasDTO.ToDto(noticia1) ;
                if (noticia1.Album != null && noticia1.Album != 0)
                {
                    AlbumDTOSmall album = await this._AlbumRepository.VerAlbumSmall((int)noticia1.Album);

                    if (album == null) {
                        return noticia;
                    }

                    noticia.Album = album.ToAlbumDTO();
                    noticia.Album.Temporada = await this._TemporadaRepository.Ver((int)album.Temporada.Id);


                }
                else {
                    return noticia;
                }
                return noticia;

            }
            catch (Exception ex) {
                throw;
            }
        }


        public async Task<NoticiasDTO> VerNoticiaEmpresa(string nombre)
        {
            try
            {
                var noticia1 = await this._noticiaRepository.ObtenerUnModelo((m) => m.Titulo == nombre && m.EsActivo == true && m.TipoNoticia == "Empresa");

                if (noticia1 == null) {
                    throw new Exception("No existe ninguna noticia para este patrocinador");
                }
                NoticiasDTO noticia = NoticiasDTO.ToDto(noticia1);
                if (noticia1.Album != null && noticia1.Album != 0)
                {
                    AlbumDTOSmall album = await this._AlbumRepository.VerAlbumSmall((int)noticia1.Album);

                    if (album == null)
                    {
                        return noticia;
                    }

                    noticia.Album = album.ToAlbumDTO();
                    noticia.Album.Temporada = await this._TemporadaRepository.Ver((int)album.Temporada.Id);


                }
                else
                {
                    return noticia;
                }
                return noticia;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
