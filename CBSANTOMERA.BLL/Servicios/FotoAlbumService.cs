//using Aspose.Imaging.FileFormats.Tiff.TiffTagTypes;
using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class FotoAlbumService : IFotoAlbumService
    {

        private readonly IGenericRepository<FotosAlbum> _FotosAlbumRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private readonly IArchivosService _ArchivosRepository;
        private readonly ITemporadaService temporadaService;

        public FotoAlbumService(IGenericRepository<FotosAlbum> FotosAlbumRepository, IMapper mapper, IArchivosService _ArchivosRepository, ITemporadaService temporadaService)
        {
            this._FotosAlbumRepository = FotosAlbumRepository;
            this.temporadaService = temporadaService;
            this._mapper = mapper;
            this._ArchivosRepository = _ArchivosRepository;
        }
        public async Task<AlbumDTO> CrearFoto(AlbumDTO album, List<IFormFile> fotos)
        {


           


            for (int i= 0; i<fotos.Count(); i++) {

                FotoAlbumDTO foto = new FotoAlbumDTO();
                foto.Descripcion = "foto" + i;
                foto.IdAlbum = album.IdAlbum;
                string hasha = album.IdAlbum.ToString() + i;
                string name = hasha.GetHashCode().ToString();

                TemporadaDTO temporada = await this.temporadaService.Ver(album.Temporada.Id);

                string extension = Path.GetExtension(fotos[i].FileName);
                string filename = name + extension;
                foto.ThumbnailImageSrc = name + "_thumbnail" + extension;
                foto.imagen = filename;
                foto.Fecha = album.Fecha;
                var foto2 = _mapper.Map<FotosAlbum>(foto);
                foto2.Temporada = temporada.Id;
                foto2.FechaModificacion = DateTime.Now;
                foto2.FechaRegistro = DateTime.Now;
                foto2.EsActivo = true;
                var fotoCreado = await _FotosAlbumRepository.Crear(foto2);
                album.FotosAlbum.Add(_mapper.Map<FotoAlbumDTO>(fotoCreado));

                AccionFile accion = new AccionFile();
                accion.file = fotos[i];
                accion.accion = "guardar";
                string carpetaAlbum = album.IdAlbum + "\\";
                string path = Path.Combine(rutaServidor, album.Temporada.Nombre, "Albumes", carpetaAlbum);
                string ruta = album.ObtenerRuta();
                accion.rutaDesten = path+filename;
                accion.rutaDestenThumbnail = path+name+ "_thumbnail"+extension;
               
                accion.thumbnail = true;
                //this.rutaServidor = @"wwwroot\archivos\Temporadas\";
                //accion.rutaDesten = this.rutaServidor + ruta + filename;
                accion.sizeThumbnail = 500;

                this._ArchivosRepository.Ejecutar(accion);
                /*string carpetaAlbum = fotoCreado.IdAlbum + "\\";
                string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                try
                {

                    using (FileStream newFile = System.IO.File.Create(rutaImagen + filename))
                    {
                        fotos[i].CopyTo(newFile);
                        
                        newFile.Flush();
                       


                    }
                    this._ArchivosRepository.RedimensionarImagen120(rutaImagen + filename, rutaImagen+foto.ThumbnailImageSrc, "jpg");


                }
                catch (Exception e)
                {
                    throw new TaskCanceledException(e.Message);
                }*/

                
            }

            return album;
        }

        public async Task<bool> Editar(ClubDTO modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarFotosAlbum(AlbumDTO album) //Elimina todas las fotos del album
        {

            foreach (FotoAlbumDTO foto in album.FotosAlbum ) {
                if (!await this.Eliminar(foto.IdFoto))
                {
                    return false;
                }
               
            }
            return true;    
        }

            public async Task<bool> Eliminar(int id) //Elimina una foto
        {


            try { 


                var fotoEncontrado = await _FotosAlbumRepository.Obtener(u => u.IdFoto == id);
                if (fotoEncontrado != null) {

                    AccionFile accion = new AccionFile();
                    
                    accion.accion = "Eliminar";
                    string carpetaAlbum = fotoEncontrado.IdAlbum + "\\";
                    TemporadaDTO temporada = await this.temporadaService.Ver((int)fotoEncontrado.Temporada);
                    string path = Path.Combine(rutaServidor, temporada.Nombre, "Albumes", carpetaAlbum);
                    //string ruta = album.ObtenerRuta();
                    accion.rutaOrigen = path + fotoEncontrado.Imagen;
                    accion.rutaOrigenThumbnail = path + fotoEncontrado.ThumbnailImageSrc;

                    accion.thumbnail = true;
                    //this.rutaServidor = @"wwwroot\archivos\Temporadas\";
                    //accion.rutaDesten = this.rutaServidor + ruta + filename;
                    //accion.sizeThumbnail = 250;

                    this._ArchivosRepository.Ejecutar(accion);
                   /* string carpetaAlbum = fotoEncontrado.IdAlbum + "\\";
                    string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                    string filepath = Path.Combine(rutaImagen, fotoEncontrado.Imagen);
                    var res = await _FotosAlbumRepository.Eliminar(fotoEncontrado);

                    if (res == true) {
                        if (File.Exists(filepath))
                        {
                            try
                            {
                                File.Delete(filepath);
                                filepath = Path.Combine(rutaImagen, fotoEncontrado.ThumbnailImageSrc);
                                File.Delete(filepath);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                //Do something
                            }
                        }



                    }*/
                }
                var res = await _FotosAlbumRepository.Eliminar(fotoEncontrado);
                return res;
               
            }

            catch (Exception e) { throw; }
           
        }

        public Task<List<FotoAlbumDTO>> Lista()
        {
            throw new NotImplementedException();
        }

        public async Task<List<FotoAlbumDTO>> VerAlbum(int Idalbum)
        {
            try {

                var fotos = await _FotosAlbumRepository.Consultar(p => p.IdAlbum == Idalbum);

                List<FotoAlbumDTO> lista = _mapper.Map<List<FotoAlbumDTO>>(fotos);
                return lista;

            }
            catch(Exception) { throw;
            }
        }


        string ComputeFourDigitStringHash(string cadena)
        {
           
            int hash = cadena.GetHashCode() % 10000;
            return hash.ToString("0000");
        }

        public Task<bool> Editar(FotoJugadorEquipoDTO modelo)
        {
            throw new NotImplementedException();
        }
    }
}
