using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class FotoTecnicoEquipoService : IFotoTecnicoEquipoService
    {

        private readonly IGenericRepository<FotoTecnico> _fotoTecnicoEquipoRepository;
        private readonly IEquipoService _equipoService;
        private readonly ITecnicoService _tecnicoService;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";
        private readonly string mirutatecnicos = @"wwwroot\archivos\Tecnicos\";
        private readonly string mirutaequiposplantilla = @"wwwroot\archivos\MisEquipos\Plantilla\";
        private readonly IArchivosService _ArchivosRepository;
        public FotoTecnicoEquipoService(IGenericRepository<FotoTecnico> fotoTecnicoEquipoRepository, IEquipoService equipoService, ITecnicoService tecnicoService,  IMapper mapper, IArchivosService _ArchivosRepository)
        {
            this._ArchivosRepository = _ArchivosRepository;
            this._fotoTecnicoEquipoRepository = fotoTecnicoEquipoRepository;
            this._tecnicoService = tecnicoService;
            _equipoService = equipoService; 
            _mapper = mapper;
           
        }
        public async Task<FotoTecnicoEquipoDTO> BuscarFotoTecnicoEquipo(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var lista = await _fotoTecnicoEquipoRepository.Obtener(p => p.Id == id);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
               FotoTecnicoEquipoDTO fotoTecnico= _mapper.Map<FotoTecnicoEquipoDTO>(lista);

                try
                {
                    if (fotoTecnico==null)
                    {
                        throw new Exception("La foto del tecnico no se encuentra en la base de datos");
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return fotoTecnico;

            }
            catch
            {
                throw;

            }
        }

        




        public async Task<List<FotoTecnicoEquipoDTO>> BuscarFotoSTecnicoEquipo(TecnicoEquipoDTO tecnico)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var lista = await _fotoTecnicoEquipoRepository.Consultar(p => p.IdTecnico == tecnico.Id);
               //var lista= await _fotoJugadoEquipoRepository.Consultar();

                

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<FotoTecnicoEquipoDTO> listaFotos = _mapper.Map<List<FotoTecnicoEquipoDTO>>(lista.ToList());
                List<FotoTecnicoEquipoDTO> listaFotos2 = new List<FotoTecnicoEquipoDTO>();
               listaFotos = listaFotos.FindAll(p => p.IdTecnico== tecnico.Id);
                foreach (var item in listaFotos)
                {
                    if (item.IdTecnico == tecnico.Id ) {
                        listaFotos2.Add(item);
                    
                    }
                }

               
                return listaFotos2;

            }
            catch
            {
                throw new Exception("No se ha podido consultar las fotos del tecnico");

            }
        }


        public async Task<bool> EliminarFotoSTecnico(List<FotoTecnicoEquipoDTO> modelos, int idequipo) {
            var listaequipo = await _equipoService.BuscarEquipo(idequipo);
            EquipoDTO listaEquipo = _mapper.Map<EquipoDTO>(listaequipo);



            foreach (FotoTecnicoEquipoDTO item in modelos)
            {
                try
                {

                    if (await this.Eliminar(item.Id) == true)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    throw new TaskCanceledException("No se pudo eliminar la foto");
                    break;
                    return false;

                }


            }

            return true;
        }





        public async Task<TecnicoEquipoDTO> CrearFotos(TecnicoEquipoDTO tecnico, List<FotoTecnicoEquipoDTO> fotos)
        {


           // TecnicoDTO equipo = await this._tecnicoService.BuscarTecnico(tecnico.Id);

            /*if (equipo == null) {
                return null;
            }*/


            for (int i = 0; i < fotos.Count(); i++)
            {

                FotoTecnicoEquipoDTO foto = new FotoTecnicoEquipoDTO();
                foto = fotos[i];
                
                foto.IdTecnico = tecnico.Id;
                string hasha =tecnico.Id.ToString() + i;
                string name = hasha.GetHashCode().ToString();


                string extension = Path.GetExtension(fotos[i].imagen.FileName);
                string filename = name + extension;
                foto.ThumbnailImageSrc = name + "_thumbnail.jpg";
                foto.Url = filename;
                //foto.Fecha = album.Fecha;
                //var j = _mapper.Map<Jugador>(jugador);

                Tecnico t = new Tecnico();
                t.Id= tecnico.Id;

                foto.IdTecnico = tecnico.Id;


                if (foto.imagen != null) {
                    foto.Tecnico = tecnico.tecnico.Id;
                    var fotoCreado = await CrearFoto(foto, t);
                   // jugador.fotos.Add(_mapper.Map<FotoJugadorEquipoDTO>(fotoCreado));
                }
               

               
               

            }

            return tecnico;
        }




        public async Task<bool> CrearFotoSTecnico(List<FotoTecnicoEquipoDTO> modelos, int idequipo) {


           var listaequipo = await _equipoService.BuscarEquipo(idequipo);
            EquipoDTO listaEquipo = _mapper.Map<EquipoDTO>(listaequipo);
            


            foreach (FotoTecnicoEquipoDTO item in modelos) {
                try {
                    //this.CrearFotos(item, listaEquipo);
                }
                catch (Exception e) {
                    throw new TaskCanceledException("No se pudo crear la foto");
                    break;
                    return false;

                }
                

            }

            return true;
        }

        public async Task<FotoTecnicoEquipoDTO> CrearFoto(FotoTecnicoEquipoDTO modelo, Tecnico tecnico)
        {
            try
            {
                FotoTecnico equi = _mapper.Map<FotoTecnico>(modelo);
                var fotoCreado = new FotoTecnico();
                if (modelo.imagen == null)
                {

                    equi.ThumbnailImageSrc = "ImageLocked2.png";
                    equi.Url = "ImageLocked2.png";
                    
                    equi.Id = 0;
                    equi.Tecnico= modelo.Tecnico;
                    //equi.Tecni = modelo.E;
                    fotoCreado = await _fotoTecnicoEquipoRepository.Crear(equi);



                    if (fotoCreado.Id == 0)
                    {
                        throw new TaskCanceledException("No se pudo crear la foto del jugador " + modelo.Tecnico);


                    }
                    return modelo;

                }

                string extension = Path.GetExtension(modelo.imagen.FileName);

                string hasha = tecnico.Id.ToString() + tecnico.ToString();
                string name = hasha.GetHashCode().ToString();
                //var listaequipo = await _equipoService.BuscarEquipo(modelo.IdEquipo);

                //Equipo equipo = _mapper.Map<Equipo>(listaequipo);

                //
                //var clubEncontrado = await _clubRepository.Obtener(p => p.IdClub ==_Miclub);
                //if (clubEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }


                string carpetaequipos = mirutaequipos;


              
                string filename = name + extension;
                equi.ThumbnailImageSrc = name + "_thumbnail.jpg";
                equi.Url = filename;
                equi.Tecnico = modelo.Tecnico;
                
                equi.Id = 0;
                //equi. = modelo.;
               // equi. = modelo.EquipoJugador;
                //equi.IdEquipoNavigation = equipo;
                //equipo.EquipoJugadores.Add(_mapper.Map<FotoJugadorEquipoDTO>(fotoCreado));
                string Equipo= modelo.IdEquipo + "\\";
               
                try {

                   fotoCreado = await _fotoTecnicoEquipoRepository.Crear(equi);



                }
                catch(Exception ex) { 
                
                }  

              

                if (fotoCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la foto del jugador " + modelo.IdTecnico);


                }


                if (!Directory.Exists(carpetaequipos))
                {
                    Directory.CreateDirectory(carpetaequipos);
                }

                //carpetaequipos = carpetaequipos + "\\" + equipo.IdEquipo;

                if (!Directory.Exists(carpetaequipos + Equipo))
                {
                    Directory.CreateDirectory(carpetaequipos+ Equipo);
                }

                string rutaImagen = Path.Combine(carpetaequipos, Equipo);

                if (modelo.imagen != null)
                {
                    try
                    {

                      
                        using (FileStream newFile = System.IO.File.Create(rutaImagen + equi.Url))
                        {
                            modelo.imagen.CopyTo(newFile);
                            newFile.Flush();

                        }
                        this._ArchivosRepository.RedimensionarImagen120(rutaImagen + equi.Url, rutaImagen + equi.ThumbnailImageSrc, "jpg");

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
                        throw new TaskCanceledException("No se pudo Crear el archivo");
                    }
                }
                
                return modelo;
            }
            catch
            {
                throw;

            }
        }


        public async Task<FotoTecnicoEquipoDTO> CrearFotoDefault(FotoTecnicoEquipoDTO modelo) {
            var fotoCreado = new FotoTecnico();
            fotoCreado.Url = "ImageLocked_coach.png";
            fotoCreado.ThumbnailImageSrc = "ImageLocked_coach.png";
            modelo.Url = "ImageLocked_coach.png";
            modelo.ThumbnailImageSrc = "ImageLocked_coach.png";
            FotoTecnico equi = _mapper.Map<FotoTecnico>(modelo);
            string Equipo = modelo.IdEquipo + "\\";
            try
            {

                fotoCreado = await _fotoTecnicoEquipoRepository.Crear(equi);

                if (fotoCreado.Id == 0)
                {
                    throw new TaskCanceledException();
                }
                else {
                    modelo.Id = fotoCreado.Id;


                    try
                    {

                        if (!Directory.Exists(mirutaequipos))
                        {
                            Directory.CreateDirectory(mirutaequipos);
                        }

                        //carpetaequipos = carpetaequipos + "\\" + equipo.IdEquipo;

                        if (!Directory.Exists(mirutaequipos + Equipo))
                        {
                            Directory.CreateDirectory(mirutaequipos + Equipo);
                        }



                        File.Copy(this.mirutaequipos + modelo.Url, this.mirutaequipos+Equipo+modelo.Url, true);

                        
                     

                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se pudo Crear el archivo");
                    }



                    return modelo;
                }

            }
            catch (Exception ex)
            {
                throw new TaskCanceledException();
                //Do something
            }


        }



        public async Task<bool> Editar(FotoTecnicoEquipoDTO modelo)
        {
            try
            {


                



                var Modelo = _mapper.Map<FotoTecnico>(modelo);
                var equipoEncontrado = await _fotoTecnicoEquipoRepository.Obtener(u => u.Id == modelo.Id);


                if (modelo.imagen != null)
                {
                    string carpetaJugador = modelo.IdEquipo + "\\";
                    string hasha = modelo.Tecnico.ToString();
                    string name = hasha.GetHashCode().ToString();


                    string extension = Path.GetExtension(modelo.imagen.FileName);
                    string filename = name + extension;
                    modelo.ThumbnailImageSrc = name + "_thumbnail.jpg";
                    modelo.Url = filename;
                    string rutaImagen = Path.Combine(mirutaequipos, carpetaJugador);
                    string filepath = Path.Combine(rutaImagen, modelo.Url);
                    if (modelo.Url != "ImageLocked_coach.png")
                    {

                        


                    }
                    else
                    {
                        
                    }







                    if (File.Exists(filepath)) //SI la ruta existe
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

                            using (FileStream newFile = System.IO.File.Create(rutaImagen + modelo.Url))
                            {
                                modelo.imagen.CopyTo(newFile);



                                newFile.Flush();


                            }
                            this._ArchivosRepository.RedimensionarImagen120(rutaImagen + modelo.Url, rutaImagen + modelo.ThumbnailImageSrc, "jpg");
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
                        try
                        {

                            using (FileStream newFile = System.IO.File.Create(rutaImagen + modelo.Url))
                            {
                                modelo.imagen.CopyTo(newFile);



                                newFile.Flush();


                            }
                            //this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + modelo.ThumbnailImageSrc, "jpg");
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

                    }

                    equipoEncontrado.Url = modelo.Url;
                    equipoEncontrado.ThumbnailImageSrc = modelo.ThumbnailImageSrc;
                    await this._fotoTecnicoEquipoRepository.Editar(equipoEncontrado);
                    return true;

                }
            }
            catch
            {
                throw;

            }
            return false;
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var equipoEncontrado = await _fotoTecnicoEquipoRepository.Obtener(p => p.Id == id);
                if (equipoEncontrado == null) { throw new TaskCanceledException("La foto no existe"); }
                bool respuesta = await _fotoTecnicoEquipoRepository.Eliminar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar la foto");
                }
                else
                {
                    string carpetaequipos = mirutaequipos;
                    string Equipo = equipoEncontrado.Id + "\\";
                    string rutaImagen = Path.Combine(carpetaequipos, Equipo);
                   // var ruta1 = rutaImagen+ equipoEncontrado.Foto;
                    var ruta2 = rutaImagen + equipoEncontrado.ThumbnailImageSrc;
                   // var res1 = this._ArchivosRepository.EliminarArchivo(ruta1);
                    var res2 = this._ArchivosRepository.EliminarArchivo(ruta2);

                   /* if (res1 == true && res2 == true)
                    {
                        return true;
                    }*/

                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<AlbumDTO> VerFotosTecnico(int idjugador, int idequipo)
        {
            try
            {

                var albumEncontrado = await this._fotoTecnicoEquipoRepository.Obtener(u => u.IdTecnico == idequipo);
                AlbumDTO album = _mapper.Map<AlbumDTO>(albumEncontrado);


                //album.FotosAlbum = await _FotosAlbumRepository.VerAlbum(id);
                return album;





            }
            catch (Exception e) { throw; }
        }



        public async Task<List<FotoTecnicoEquipoDTO>> Lista()
        {
            try
            {
                var listaFotosTecnicoEquipo = await _fotoTecnicoEquipoRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<FotoTecnicoEquipoDTO>>(listaFotosTecnicoEquipo.ToList());

            }
            catch
            {
                throw;

            }
        }

       
    }
}
