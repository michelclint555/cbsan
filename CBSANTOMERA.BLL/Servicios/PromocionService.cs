
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

namespace CBSANTOMERA.BLL.Servicios
{
    public class PromocionService : IPromocionService
    {

        private readonly IGenericRepository<Promocione> _Repository;
        //private readonly IFotoPromocionService _FotosAlbumRepository;
        private readonly IArchivosService _ArchivosRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Novedades\";

        public PromocionService(IGenericRepository<Promocione> Repository, IMapper mapper,  IArchivosService _ArchivosRepository)
        {
            this._ArchivosRepository = _ArchivosRepository;
            _Repository = Repository;
            _mapper = mapper;
           


        }

        public async Task<PromocionDTO> Crear(PromocionDTO modelo)
        {
            var modeloCreado = new Promocione();
            modelo.Portada = "";
            string name = "";
            string filename = "";
            string extension = "";


            try {
               
                
                modelo.FechaRegistro = DateTime.Now;
                
                extension = Path.GetExtension(modelo.imagen.FileName);
                
                modelo.Fijar = true;
                modelo.Orden = 0;
                Promocione promocion = _mapper.Map<Promocione>(modelo);
                modeloCreado = await _Repository.Crear(promocion);

                string hasha = modeloCreado.Id.ToString();
                name = hasha.GetHashCode().ToString();
                filename = name + extension;
                modelo.Portada = filename;
                modelo.Thumbnail = name + "_thumbnail"+extension;
            }

            catch (Exception e)
            {
                throw new Exception("No se ha podido crear la promoción en la BBDD", e);
            }




            string carpetaAlbum = "";

            if (modeloCreado.Id == 0)
            {
                throw new TaskCanceledException("No se pudo crear la promoción"+ modeloCreado.Titulo);


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


            try
            {

                using (FileStream newFile = System.IO.File.Create(rutaImagen + filename))
                {
                    modelo.imagen.CopyTo(newFile);


                    modeloCreado.Portada = filename;
                    modeloCreado.Titulo = modelo.Titulo;
                    modeloCreado.Thumbnail = name+"_thumbnail"+extension;
                    newFile.Flush();
                   

                }
                this._ArchivosRepository.RedimensionarImagen120(rutaImagen + filename, rutaImagen +modeloCreado.Thumbnail , "jpeg");
                modelo.Id = modeloCreado.Id;
                modelo.imagen = null;
                var res = await _Repository.Editar(_mapper.Map<Promocione>(modelo));

            }
            catch (Exception e)
            {
                throw new TaskCanceledException("Error al crear las imagen de la promoción.");
            }

            return _mapper.Map<PromocionDTO>(modeloCreado);
        }

        public async Task<bool> Editar(PromocionDTO modelo)
        {
            try

            {

                string name = "";
                var modeloEncontrado = await _Repository.Obtener(u => u.Id == modelo.Id);
                

                
               
              


                if (modelo != null)
                {

                    if (modelo.imagen != null)
                    {
                        string hasha = modelo.Id.ToString()+DateTime.Now;
                        name = hasha.GetHashCode().ToString();
                        string extension = Path.GetExtension(modelo.imagen.FileName);
                        string thumbnail = name+"_thumbnail" + extension;
                        string filename = name + extension;
                        
                        string carpeta = "";
                        string rutaImagen = Path.Combine(rutaServidor, carpeta);
                        string filepath = Path.Combine(rutaImagen, modelo.Portada);                       
                        //thumbnail =  Path.GetFileName (modelo.imagen.FileName);
                        //thumbnail = Path.GetFileName(modelo.Thumbnail) + extension;
                        string filepaththumbnail = Path.Combine(rutaImagen, thumbnail);
                        string filepathNewImage= Path.Combine(rutaImagen, filename);

                        AccionFile accion = new AccionFile();
                        accion.accion = "Actualizar";
                        accion.rutaOrigen = Path.Combine(rutaImagen, modelo.Portada); 
                        accion.rutaDesten = Path.Combine(rutaImagen, filename);
                        accion.rutaOrigenThumbnail = Path.Combine(rutaImagen, modelo.Thumbnail);
                        accion.rutaDestenThumbnail = Path.Combine(rutaImagen, thumbnail);
                        accion.thumbnail = true;
                        accion.file = modelo.imagen;
                        modelo.Portada = filename;
                        modelo.Thumbnail = thumbnail;
                        accion.sizeThumbnail = 300; 
                        this._ArchivosRepository.Ejecutar(accion);
                        
                      /*  if (File.Exists(filepath)) //SI la ruta existe
                        {
                            try
                            {
                                File.Delete(filepath); //Se borra la imagen del directorio
                                

                            }
                            catch (Exception ex)
                            {
                                throw new TaskCanceledException("Error al borrar la imagen.");
                            }

                            try
                            {

                                using (FileStream newFile = System.IO.File.Create(rutaImagen + "portada10" + extension))
                                {
                                    modelo.imagen.CopyTo(newFile);


                                    
                                    newFile.Flush();


                                }
                                this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + modelo.Portada, "jpg");
                                if (File.Exists(rutaImagen + "portada10" + extension)) //SI la ruta existe
                                {
                                    try
                                    {
                                        File.Delete(rutaImagen + "portada10" + extension); //Se borra la imagen del directorio

                                    }
                                    catch (Exception ex)
                                    {
                                        throw new TaskCanceledException("Error al borrar la imagen.");
                                    }

                                }


                            }
                            catch (Exception e)
                            {
                                throw new TaskCanceledException("Error al copiar o redimensionar la portada.");
                            }

                            //this._ArchivosRepository.RedimensionarImagen120(filepath, rutaImagen + modelo.Portada);
                        }
                        else //Si la ruta no existe creamos la imagen por si se da el caso
                        {
                            this._ArchivosRepository.RedimensionarImagen120(filepath, "portada10.jpg", "jpg");

                        }*/


                    }

                    //Procedemos a modificar las fotos, obtenemos todas la fotos del IDAlbum y la lista de las fotos a mantenener en la BBDD, comprobamos si la lista de fotos de la BBDD y la lista de fotos del cliente no se ha modificado.



                    //Procedemos a añadir las nuevas fotos






                    return await _Repository.Editar(_mapper.Map<Promocione>(modelo));

                }
                else {
                    return false;
                }

                return true;

            }

            catch (Exception e) {

                throw new TaskCanceledException("Error al editar la promoción en la BBDD.");
            }
        }

        


        public async Task<PromocionDTO> Ver(int id)
        {
            try {
                
                var encontrado = await _Repository.Obtener(u => u.Id == id);
                PromocionDTO modelo= _mapper.Map<PromocionDTO>(encontrado);
              
                return modelo;
              
                             

                

            } catch (Exception e) {
                throw new TaskCanceledException("Error al consultar la promoción en la BBDD.");
            }
        }


        public async Task<bool> Eliminar(int id)
        {
            try
            {

                PromocionDTO modelo= await this.Ver(id);
                var encontrado = await _Repository.Obtener(u => u.Id == id);

                try {
       
                    string carpeta= "";
                    string rutaImagen = Path.Combine(rutaServidor, carpeta);
                    string filepath = Path.Combine(rutaImagen, modelo.Portada);

                    AccionFile accion = new AccionFile();
                    accion.accion = "Eliminar";
                    accion.rutaOrigen = Path.Combine(rutaImagen, modelo.Portada);
                   
                    accion.rutaOrigenThumbnail = Path.Combine(rutaImagen, modelo.Thumbnail);
                    accion.thumbnail = true;
                  
                    

                    this._ArchivosRepository.Ejecutar(accion);





                    if (await _Repository.Eliminar(encontrado)) {

                        
                        
                        return true;
                    }
                   



                }
                catch (Exception ex) { 
                
                
                }



                






            }
            catch (Exception e) { throw; }

            return false;
        }


        

        public async Task<List<PromocionDTO>> ListaCompleta()
        {
            try
            {
                var lista= await _Repository.Consultar();

               lista= lista.OrderByDescending(d => d.FechaRegistro);


                List<PromocionDTO> modelos=  _mapper.Map<List<PromocionDTO>>(lista.ToList());

                

                return modelos;

            }
            catch
            { 
                throw;

            }
        }


        public async Task<List<PromocionDTOSmall>> Lista()
        {
            try
            {
                var lista = await _Repository.Consultar(m=> m.Fijar == true && m.Orden != 0 && m.Orden != null);

                lista = lista.OrderBy(d => d.Orden);


                List<PromocionDTOSmall> modelos = _mapper.Map<List<PromocionDTOSmall>>(lista.ToList());



                return modelos;

            }
            catch
            {
                throw;

            }
        }

        string ComputeFourDigitStringHash(string cadena)
        {

            int hash = cadena.GetHashCode() % 10000;
            return hash.ToString("0000");
        }

    }

   
}
