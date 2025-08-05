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
using System.Security.Cryptography;
using System.IO;

namespace CBSANTOMERA.BLL.Servicios
{
    public class ClubService : IClubService
    {

        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IMapper _mapper;
        private readonly string  rutaServidor = @"wwwroot\archivos\Clubes\";

       
        //private readonly IEquipoService  _EquiposRepository;
        private readonly IArchivosService _ArchivosRepository;
       
        public ClubService(IGenericRepository<Club> productoRepository, IMapper mapper, /*IEquipoService _EquiposRepository,*/ IArchivosService _ArchivosRepository)
        {
            _clubRepository = productoRepository;
            _mapper = mapper;
          //this. _EquiposRepository = _EquiposRepository;
            this._ArchivosRepository = _ArchivosRepository;
        }

        public async Task<ClubDTO> Crear(ClubDTO modelo)
        {
            try
            {

                if (modelo.imagen == null) {
                    throw new TaskCanceledException("No se pudo crear la imagen, falta el logo del club");
                }
                string hasha = modelo.ToString() + DateTime.Now;
                string name = hasha.GetHashCode().ToString();
                string extension = Path.GetExtension(modelo.imagen.FileName);
                string filename = name +extension;
                modelo.EsActivo = false;
                
                modelo.Foto = filename;
                
                var clubCreado = await _clubRepository.Crear(_mapper.Map<Club>(modelo));
                
                string carpetaClub = clubCreado.IdClub +"\\";

                if (clubCreado.IdClub == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");


                }

                if (!Directory.Exists(rutaServidor))
                {
                    Directory.CreateDirectory(rutaServidor);
                }

                if (!Directory.Exists(rutaServidor + carpetaClub))
                {
                    Directory.CreateDirectory(rutaServidor+ carpetaClub);
                }
                string rutaImagen = Path.Combine(rutaServidor, carpetaClub);

                AccionFile accion = new AccionFile();
                accion.accion = "Guardar";
                accion.thumbnail = false; ;
                accion.file = modelo.imagen;
                accion.rutaDesten = rutaImagen + filename;
                this._ArchivosRepository.Ejecutar(accion);

              
                

                return _mapper.Map<ClubDTO>(clubCreado);

            }
            catch
            {
                throw;

            }
        }

       

        public async Task<bool> Editar(ClubDTO modelo)
        {
            try
            {


                var Encontrado = await _clubRepository.Obtener(u => u.IdClub == modelo.IdClub);
               
                string hasha = modelo.ToString() + DateTime.Now;
                string name = hasha.GetHashCode().ToString();
                string filename = "";
                string filename1 = modelo.Foto;
                if (modelo.imagen != null) {
                    string extension = Path.GetExtension(modelo.imagen.FileName);
                    filename = name + extension;
                    modelo.Foto = filename;

                }
                
                var res = await _clubRepository.Editar(_mapper.Map<Club>(modelo));



                if (res == true)
                {

                    if (modelo.imagen != null)
                    {



                        string carpetaAlbum = modelo.IdClub + "\\";
                        string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);
                        string filepath = Path.Combine(rutaImagen, modelo.Foto);
                        string extension = Path.GetExtension(modelo.imagen.FileName);
                        AccionFile accion = new AccionFile();
                        if (filename1 == null || filename1 == "")
                        {
                            accion.accion = "Guardar";
                            
                        }
                        else {
                            accion.accion = "Actualizar";
                            accion.rutaOrigen = Path.Combine(rutaImagen, filename1);
                        }

                        //accion.accion = "Actualizar";
                        accion.thumbnail = false; ;
                        accion.file = modelo.imagen;
                        accion.rutaDesten = filepath;
                        //accion.rutaOrigen = Path.Combine(rutaImagen, filename1); 

                        this._ArchivosRepository.Ejecutar(accion);


                       /* if (File.Exists(filepath)) //SI la ruta existe
                        {
                            try
                            {
                                File.Delete(filepath); //Se borra la imagen del directorio


                            }
                            catch (Exception ex)
                            {
                                //Do something
                            }

                            try
                            {

                                using (FileStream newFile = System.IO.File.Create(rutaImagen + "portada10" + extension))
                                {
                                    modelo.imagen.CopyTo(newFile);



                                    newFile.Flush();


                                }
                                this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + modelo.Nombre, "png");
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
                            }

                            //this._ArchivosRepository.RedimensionarImagen120(filepath, rutaImagen + modelo.Portada);
                        }
                        else //Si la ruta no existe creamos la imagen por si se da el caso
                        {
                            this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + modelo.Foto, "png");

                        }*/




                    }
                }
            }

            //Procedemos a modificar las fotos, obtenemos todas la fotos del IDAlbum y la lista de las fotos a eliminar, comprobamos si la lista de fotos de la BBDD y la lista de fotos del cliente no se ha modificado.



            catch (Exception e) { throw; }

            return true;
        
    }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var clubEncontrado = await _clubRepository.Obtener(p => p.IdClub == id);
                string carpetaClub =  clubEncontrado.IdClub + "\\";
                if (clubEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                bool respuesta = await _clubRepository.Eliminar(clubEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo Eliminar de la base de datos");
                }
                try
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(this.rutaServidor + carpetaClub);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    Directory.Delete(this.rutaServidor + carpetaClub, true);
                    

                }
                catch (Exception e)
                {
                    throw new TaskCanceledException("No se pudo eliminar la carpeta");
                }
                return respuesta;

            }
            catch
            {
                throw;

            }
        }


        public async Task<ClubDTO> VerClub(int id)
        {
            try
            {

                var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == id);
                ClubDTO club = _mapper.Map<ClubDTO>(clubEncontrado);



                //club.ListaEquipos = await _EquiposRepository.ListaEquiposClub(id);
                return club;





            }
            catch (Exception e) { throw; }
        }

        public async Task<List<ClubDTO>> Lista()
        {
            try
            {
                var listaClubes = await _clubRepository.Consultar();



                return _mapper.Map<List<ClubDTO>>(listaClubes.ToList());

            }
            catch
            {
                throw;

            }
        }
    }
}
