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
    public class FotoJugadorEquipoService : IFotoJugadorEquipoService
    {

        private readonly IGenericRepository<FotoJugadorEquipo> _fotoJugadoEquipoRepository;
        private readonly IEquipoService _equipoService;
        private readonly IJugadorService _jugadorService;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";
        private readonly string mirutaequiposplantilla = @"wwwroot\archivos\MisEquipos\Plantilla\";
        private readonly IArchivosService _ArchivosRepository;
        public FotoJugadorEquipoService(IGenericRepository<FotoJugadorEquipo> fotoJugadorEquipoRepository, IEquipoService equipoService, IJugadorService jugadorService,  IMapper mapper, IArchivosService _ArchivosRepository)
        {
            this._ArchivosRepository = _ArchivosRepository;
            this._fotoJugadoEquipoRepository = fotoJugadorEquipoRepository;
            this._jugadorService = jugadorService;
            _equipoService = equipoService; 
            _mapper = mapper;
           
        }
        public async Task<FotoJugadorEquipoDTO> BuscarFotoJugadorEquipo(int id)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var lista = await _fotoJugadoEquipoRepository.Obtener(p => p.Id == id);

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
               FotoJugadorEquipoDTO fotoJugador = _mapper.Map<FotoJugadorEquipoDTO>(lista);

                try
                {
                    if (fotoJugador ==null)
                    {
                        throw new Exception("La foto del  jugador no se encuentra en la base de datos");
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                return fotoJugador;

            }
            catch
            {
                throw;

            }
        }

        




        public async Task<List<FotoJugadorEquipoDTO>> BuscarFotoSJugadorEquipo(EquipoJugadorDTO jugador)
        {
            try
            {
                //var clubEncontrado = await _clubRepository.Obtener(u => u.IdClub == _Miclub);
                var lista = await _fotoJugadoEquipoRepository.Consultar(p => p.EquipoJugador == jugador.Id);
               //var lista= await _fotoJugadoEquipoRepository.Consultar();

                

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                List<FotoJugadorEquipoDTO> listaFotos = _mapper.Map<List<FotoJugadorEquipoDTO>>(lista.ToList());
                List<FotoJugadorEquipoDTO> listaFotos2 = new List<FotoJugadorEquipoDTO>();
               listaFotos = listaFotos.FindAll(p => p.EquipoJugador == jugador.Id);
                foreach (var item in listaFotos)
                {
                    if (item.EquipoJugador == jugador.Id) {
                        listaFotos2.Add(item);
                    
                    }
                }

               
                return listaFotos2;

            }
            catch
            {
                throw new Exception("No se ha podido consultar las fotos del jugador");

            }
        }


        public async Task<bool> EliminarFotoSJugador(List<FotoJugadorEquipoDTO> modelos, int idequipo) {
            var listaequipo = await _equipoService.BuscarEquipo(idequipo);
            EquipoDTO listaEquipo = _mapper.Map<EquipoDTO>(listaequipo);



            foreach (FotoJugadorEquipoDTO item in modelos)
            {
                try
                {

                    if (await this.Eliminar(item.Id) == true)
                    {
                        continue;
                    }
                    else {
                        
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





        public async Task<EquipoJugadorDTO> CrearFotos(EquipoJugadorDTO jugador, List<FotoJugadorEquipoDTO> fotos)
        {


            JugadorDTO equipo = await this._jugadorService.BuscarJugador(jugador.Jugador.IdJugador);

            if (equipo == null) {
                return null;
            }

            List<FotoJugadorEquipoDTO> fotos0 = await this.BuscarFotoSJugadorEquipo(jugador);
            if (fotos0.Count() == 1 && fotos.Count() == 0 ) {
                //EDITAR EL REGISTRO FOTO DE LA BBDD

                
                if (await this.Editar(fotos[0])) {
                    throw new TaskCanceledException();
                }
            }
            if (fotos0.Count() == 0 && fotos.Count() == 1)
            {
               
                    var fotoCreado = await CrearFoto(fotos[0], JugadorDTO.ToModel(equipo));
                    // jugador.fotos.Add(_mapper.Map<FotoJugadorEquipoDTO>(fotoCreado));
                
               
            }

            if (fotos0.Count() >1 && fotos.Count() == 1)
            {
                //ELIMINAR TODO Y CREAR UNO NUEVO
                await this.EliminarFotoSJugador(fotos0, jugador.Equipo);
                var fotoCreado = await CrearFoto(fotos[0], JugadorDTO.ToModel(equipo));

            }

           /* for (int i = 0; i < fotos.Count(); i++)
            {

                FotoJugadorEquipoDTO foto = new FotoJugadorEquipoDTO();
                foto = fotos[i];
                
                foto.IdEquipo = jugador.Equipo;
                string hasha =jugador.Id.ToString() + i;
                string name = hasha.GetHashCode().ToString();


                string extension = Path.GetExtension(fotos[i].imagen.FileName);
                string filename = name + extension;
                foto.ThumbnailImageSrc = name + "_thumbnail.jpg";
                foto.Foto = filename;
                //foto.Fecha = album.Fecha;
                //var j = _mapper.Map<Jugador>(jugador);

                Jugador j = new Jugador();
                j.IdJugador = jugador.Id;

                foto.EquipoJugador = jugador.Id;


                if (foto.imagen != null) {
                    var fotoCreado = await CrearFoto(foto, j);
                   // jugador.fotos.Add(_mapper.Map<FotoJugadorEquipoDTO>(fotoCreado));
                }
                else{
                    //fotoCreado = await _fotoJugadoEquipoRepository.Crear(jugador.fotos.);
                }

               
                /*string carpetaJugador = jugador.Equipo + "\\";
                string rutaImagen = Path.Combine(mirutaequipos, carpetaJugador);
                try
                {

                    using (FileStream newFile = System.IO.File.Create(rutaImagen + filename))
                    {
                        fotos[i].imagen.CopyTo(newFile);

                        newFile.Flush();



                    }
                    this._ArchivosRepository.RedimensionarImagen120(rutaImagen + filename, rutaImagen + foto.ThumbnailImageSrc, "jpg");


                }
                catch (Exception e)
                {
                    throw new TaskCanceledException(e.Message);
                }
                

            }*/

            return jugador;
        }




        public async Task<bool> CrearFotoSJugador(List<FotoJugadorEquipoDTO> modelos, int idequipo) {


           var listaequipo = await _equipoService.BuscarEquipo(idequipo);
            EquipoDTO listaEquipo = _mapper.Map<EquipoDTO>(listaequipo);
            


            foreach (FotoJugadorEquipoDTO item in modelos) {
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

        public async Task<FotoJugadorEquipoDTO> CrearFoto(FotoJugadorEquipoDTO modelo, Jugador jugador)
        {
            try
            {
                FotoJugadorEquipo equi = _mapper.Map<FotoJugadorEquipo>(modelo);
                var fotoCreado = new FotoJugadorEquipo();

                if (modelo.imagen == null) {
                 
                    equi.ThumbnailImageSrc ="ImageLocked2.png";
                    equi.Foto = "ImageLocked2.png";
                    equi.UrlFoto = "ImageLocked2.png";
                    equi.Id = 0;
                    equi.Jugador = modelo.Jugador;
                    equi.EquipoJugador = modelo.EquipoJugador;
                    fotoCreado = await _fotoJugadoEquipoRepository.Crear(equi);



                    if (fotoCreado.Id == 0)
                    {
                        throw new TaskCanceledException("No se pudo crear la foto del jugador " + modelo.Jugador);


                    }
                    return modelo;

                }
                string extension = Path.GetExtension(modelo.imagen.FileName);

                string hasha = jugador.IdJugador.ToString() + jugador.ToString();
                string name = hasha.GetHashCode().ToString();
                //var listaequipo = await _equipoService.BuscarEquipo(modelo.IdEquipo);

                //Equipo equipo = _mapper.Map<Equipo>(listaequipo);

                //
                //var clubEncontrado = await _clubRepository.Obtener(p => p.IdClub ==_Miclub);
                //if (clubEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }


                string carpetaequipos = mirutaequipos;


               
                string filename = name + extension;
                equi.ThumbnailImageSrc = name + "_thumbnail.jpg";
                equi.Foto = filename;
                equi.UrlFoto = filename;
                equi.Id = 0;
                equi.Jugador = modelo.Jugador;
                equi.EquipoJugador = modelo.EquipoJugador;
                //equi.IdEquipoNavigation = equipo;
                //equipo.EquipoJugadores.Add(_mapper.Map<FotoJugadorEquipoDTO>(fotoCreado));
                string Equipo= modelo.IdEquipo + "\\";
                

                try {

                   fotoCreado = await _fotoJugadoEquipoRepository.Crear(equi);



                }
                catch(Exception ex) { 
                
                }  

              

                if (fotoCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la foto del jugador " + modelo.Jugador);


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


                        using (FileStream newFile = System.IO.File.Create(rutaImagen + equi.Foto))
                        {
                            modelo.imagen.CopyTo(newFile);
                            newFile.Flush();

                        }
                        this._ArchivosRepository.RedimensionarImagen120(rutaImagen + equi.Foto, rutaImagen + equi.ThumbnailImageSrc, "jpg");

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
                else {

                    try
                    {
                        

                        using (FileStream newFile = System.IO.File.OpenRead(carpetaequipos + "ImageLocked.png"))
                        {
                           File.Copy(carpetaequipos + "ImageLocked.png", rutaImagen + equi.Foto);
                            newFile.Flush();

                        }
                        this._ArchivosRepository.RedimensionarImagen120(rutaImagen + equi.Foto, rutaImagen + equi.ThumbnailImageSrc, "jpg");

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

        
        

        public async Task<bool> Editar(FotoJugadorEquipoDTO modelo)
        {
            try
            {
                var Modelo = _mapper.Map<FotoJugadorEquipo>(modelo);
                var equipoEncontrado = await _fotoJugadoEquipoRepository.Obtener(u => u.Id == modelo.Id);

                if (modelo.imagen != null)
                {
                    string carpetaJugador = modelo.Jugador + "\\";
                    string rutaImagen = Path.Combine(mirutaequipos, carpetaJugador);
                    string filepath = Path.Combine(rutaImagen, modelo.Foto);
                    string extension = Path.GetExtension(modelo.imagen.FileName);


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

                            using (FileStream newFile = System.IO.File.Create(rutaImagen + modelo.Foto))
                            {
                                modelo.imagen.CopyTo(newFile);



                                newFile.Flush();


                            }
                            this._ArchivosRepository.RedimensionarImagen120(rutaImagen + "portada10" + extension, rutaImagen + modelo.ThumbnailImageSrc, "jpg");
                          /*  if (File.Exists(rutaImagen + "portada10" + extension)) //SI la ruta existe
                            {
                                try
                                {
                                    File.Delete(rutaImagen + "portada10" + extension); //Se borra la imagen del directorio

                                }
                                catch (Exception ex)
                                {
                                    //Do something
                                }

                            }*/


                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException(e.Message);
                        }

                        //this._ArchivosRepository.RedimensionarImagen120(filepath, rutaImagen + modelo.Portada);
                    }
                    else //Si la ruta no existe creamos la imagen por si se da el caso
                    {
                        this._ArchivosRepository.RedimensionarImagen120(filepath, "portada10.jpg", "jpg");

                    }


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
                var equipoEncontrado = await _fotoJugadoEquipoRepository.Obtener(p => p.Id == id);
                if (equipoEncontrado == null) { throw new TaskCanceledException("La foto no existe"); }
                bool respuesta = await _fotoJugadoEquipoRepository.Eliminar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo eliminar la foto");
                }
                else
                {
                    string carpetaequipos = mirutaequipos;
                    string Equipo = equipoEncontrado.IdEquipo + "\\";
                    string rutaImagen = Path.Combine(carpetaequipos, Equipo);
                    var ruta1 = rutaImagen+ equipoEncontrado.Foto;
                    var ruta2 = rutaImagen + equipoEncontrado.ThumbnailImageSrc;
                    var res1 = this._ArchivosRepository.EliminarArchivo(ruta1);
                    var res2 = this._ArchivosRepository.EliminarArchivo(ruta2);

                    if (res1 == true && res2 == true)
                    {
                        return true;
                    }

                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<AlbumDTO> VerFotosJugador(int idjugador, int idequipo)
        {
            try
            {

                var albumEncontrado = await this._fotoJugadoEquipoRepository.Obtener(u => u.IdEquipo == idequipo);
                AlbumDTO album = _mapper.Map<AlbumDTO>(albumEncontrado);


                //album.FotosAlbum = await _FotosAlbumRepository.VerAlbum(id);
                return album;





            }
            catch (Exception e) { throw; }
        }



        public async Task<List<FotoJugadorEquipoDTO>> Lista()
        {
            try
            {
                var listaFotosJugadorEquipo = await _fotoJugadoEquipoRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<FotoJugadorEquipoDTO>>(listaFotosJugadorEquipo.ToList());

            }
            catch
            {
                throw;

            }
        }

        
    }
}
