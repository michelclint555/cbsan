
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
    public class FotoEquipoService : IFotoEquipoService
    {

        private readonly IGenericRepository<FotoEquipo> _FotosAlbumRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Equipos\";
        private readonly IArchivosService _ArchivosRepository;
        private readonly IEquipoService _EquipoService;

        public FotoEquipoService(IGenericRepository<FotoEquipo> FotosEquipoRepository, IMapper mapper, IArchivosService _ArchivosRepository, IEquipoService equipoService)
        {
            this._EquipoService = equipoService;
            this._FotosAlbumRepository = FotosEquipoRepository;
            _mapper = mapper;
            this._ArchivosRepository = _ArchivosRepository;
        }
       public async Task<FotoEquipoDTO> Crear(EquipoDTO equipo, IFormFile foto)
        {


           


          

                FotoEquipoDTO fotoEquipo = new FotoEquipoDTO();

            var fotos = await this._FotosAlbumRepository.Consultar();
            

                fotoEquipo.IdEquipo = equipo.IdEquipo;
                string hasha = equipo.IdClub.ToString() + equipo.IdEquipo.ToString()+ equipo.idCategoria.ToString()+DateTime.Now+ fotos.Count(); 
                string name = hasha.GetHashCode().ToString();   
               
                
                string extension = Path.GetExtension(foto.FileName);
                string filename = name + extension;
                //foto.ThumbnailImageSrc = name + "_thumbnail.jpg";
                fotoEquipo.Foto = filename;
                fotoEquipo.FechaCreacion = DateTime.Now;

                var fotoCreado = await _FotosAlbumRepository.Crear(_mapper.Map<FotoEquipo>(foto));
                fotoEquipo = _mapper.Map<FotoEquipoDTO>(fotoCreado);
            string carpetaAlbum = equipo.IdClub + "\\" + fotoCreado.IdEquipo + "\\";
            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
            string filepath = Path.Combine(rutaImagen, fotoCreado.Foto);
            try
                {

                    using (FileStream newFile = System.IO.File.Create(rutaImagen + filename))
                    {
                        foto.CopyTo(newFile);
                        
                        newFile.Flush();
                       


                    }
                    this._ArchivosRepository.RedimensionarImagen120(rutaImagen + filename, rutaImagen+filename, "jpg");


                }
                catch (Exception e)
                {
                    throw new TaskCanceledException(e.Message);
                }

                
            

            return fotoEquipo;
        }

        public async Task<bool> Editar(EquipoDTO modelo)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarFotosEquipo(EquipoDTO equipo) //Elimina todas las fotos del album
        {

            foreach (FotoEquipoDTO foto in equipo.FotoEquipos) {
                if (!await this.Eliminar(foto.Id))
                {
                    return false;
                }
               
            }
            return true;    
        }

        public async Task<bool> Eliminar(int id) //Elimina una foto
        {
            var fotoEncontrado = await _FotosAlbumRepository.Obtener(u => u.IdEquipo == id);

            try {




                var equipo = await this._EquipoService.BuscarEquipo(fotoEncontrado.IdEquipo);
                if (fotoEncontrado != null) {
                    string carpetaAlbum = equipo.IdClub + "\\"+fotoEncontrado.IdEquipo + "\\";
                    string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                    string filepath = Path.Combine(rutaImagen, fotoEncontrado.Foto);
                    var res = await _FotosAlbumRepository.Eliminar(fotoEncontrado);

                    if (res == true) {
                        if (File.Exists(filepath))
                        {
                            try
                            {
                                File.Delete(filepath);
                                filepath = Path.Combine(rutaImagen, fotoEncontrado.Foto);
                                File.Delete(filepath);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                //Do something
                            }
                        }



                    }
                }

                return false;
               
            }

            catch (Exception e) { throw; }
           
        }

        public Task<List<FotoEquipoDTO>> Lista()
        {
            throw new NotImplementedException();
        }

        public async Task<List<FotoEquipoDTO>> VerFotosEquipo(int id)
        {
            try {

                var fotos = await _FotosAlbumRepository.Consultar(p => p.IdEquipo == id);

                List<FotoEquipoDTO> lista = _mapper.Map<List<FotoEquipoDTO>>(fotos.ToList());
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
    }
}
