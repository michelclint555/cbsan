using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class TecnicoEquipoService : ITecnicoEquipoService
    {
        private readonly IGenericRepository<TecnicoEquipo> _equipoTecnicoRepository;
        private readonly ITecnicoService _tecnicoService;
        private readonly IEquipoService _equipoService;
        private readonly IFotoTecnicoEquipoService _fotoTecnicoEquipoService;
        //  private readonly IGenericRepository<FotoJugadorEquipo> _fotoJugadorequipoRepository;
        // private readonly IGenericRepository<Equipo> _equipoRepository;

        // private readonly IGenericRepository<Club> _clubRepository;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";

        public TecnicoEquipoService(ITecnicoService tecnicoService, IFotoTecnicoEquipoService fotoTecnicoEquipoService, IEquipoService _equipoService, IGenericRepository<TecnicoEquipo> equipoTecnicoRepository, IMapper mapper)
        {

            this._equipoService = _equipoService;
            //_jugadorRepository = jugadorRepository;
            this._fotoTecnicoEquipoService = fotoTecnicoEquipoService;
            this._tecnicoService = tecnicoService;
            this._equipoTecnicoRepository = equipoTecnicoRepository;
            _mapper = mapper;
            
        }

        public async Task<TecnicoEquipoDTO> Crear(TecnicoEquipoDTO modelo)
        {
            try
            {

                TecnicoEquipo j = _mapper.Map<TecnicoEquipo>(modelo);
                j.Funcion = modelo.Funcion;
                j.IdEquipo = modelo.IdEquipo;
                j.IdTecnico = modelo.tecnico.Id;
                j.Id = 0;
                //j.IdEquipoNavigation.IdEquipo = modelo.IdEquipo;
                //j.IdTecnicoNavigation.Id = modelo.tecnico.Id;
                var listaEquipoTecnicos= await _equipoTecnicoRepository.Consultar((i) => i.IdEquipo == modelo.IdEquipo);
                List<TecnicoEquipo> Lista = listaEquipoTecnicos.ToList();
                foreach (var item in Lista)
                {
                    if (item.IdTecnico == modelo.tecnico.Id && item.IdEquipo == modelo.IdEquipo&& item.Funcion== modelo.Funcion) {
                        throw new TaskCanceledException("El tecnico ya se encuentra en este equipo");
                    }
                }


                var equipoJugadorCreado = await this._equipoTecnicoRepository.Crear(j);
                modelo.Id = equipoJugadorCreado.Id;

                if (equipoJugadorCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }
                
                if (modelo.listaFotos.Count != 0) {


                    if (modelo.listaFotos.Count == 1 && modelo.listaFotos[0].imagen == null)
                    {
                        modelo.listaFotos.Clear();
                    }
                    else {
                        await this._fotoTecnicoEquipoService.CrearFotos(modelo, modelo.listaFotos);

                    }
                  



                }

                if (modelo.listaFotos.Count == 0)
                {
                    FotoTecnicoEquipoDTO foto = new FotoTecnicoEquipoDTO();
                    foto.Id = 0;
                    foto.IdTecnico = modelo.Id;
                    foto.Tecnico = modelo.tecnico.Id;
                    foto.IdEquipo = modelo.IdEquipo;
                    foto.Tecnico = modelo.tecnico.Id;
                    
                   foto  =await this._fotoTecnicoEquipoService.CrearFotoDefault(foto);
                    modelo.listaFotos.Add(foto);
                }

                return modelo;

            }
            catch
            {
                throw;

            }
        }






        public async Task<bool> Eliminar(EquipoDTO equipo, TecnicoEquipoDTO tecnico)
        {
            try
            {
                //var equipoEncontrado = await _equipoJugadorRepository.Obtener(p => p.Id == id);
                //if (equipoEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                if(await this._fotoTecnicoEquipoService.EliminarFotoSTecnico(tecnico.listaFotos, equipo.IdEquipo)){

                    TecnicoEquipo j = new TecnicoEquipo();
                    j.Id = tecnico.Id;
                    j.IdEquipo = tecnico.IdEquipo;
                    j.Funcion = tecnico.Funcion;
                    j.IdTecnico = tecnico.Id;


                    if (await this._equipoTecnicoRepository.Eliminar(j))
                    {
                        return true;

                    }
                    return false ;
                
                }
                
               
                
                return false;

            }
            catch
            {
                throw;

            }
        }


        public async Task<List<TecnicoEquipoDTO>> ListaTecnicosUnEquipo(int idEquipo)
        {
            try
            {
                var listaEquipoJugadores = await _equipoTecnicoRepository.Consultar((i) => i.IdEquipo == idEquipo);

                List<TecnicoEquipoDTO> listaDTO = new List<TecnicoEquipoDTO>();
                List<TecnicoEquipo> Lista = listaEquipoJugadores.ToList();

                foreach (var item in listaEquipoJugadores)
                {
                    TecnicoEquipoDTO EJDTO = new TecnicoEquipoDTO();


                    TecnicoDTO JDTO = await _tecnicoService.BuscarTecnico((int)item.IdTecnico);
                    EJDTO.tecnico = JDTO;


                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.IdEquipo = (int)item.IdEquipo;
                    EJDTO.Funcion = item.Funcion;
                    EJDTO.Id = (int)item.Id;
                    EJDTO.listaFotos = await this._fotoTecnicoEquipoService.BuscarFotoSTecnicoEquipo(EJDTO);
                    listaDTO.Add(EJDTO);
                }
                
                   



                

                return listaDTO;
            }
            catch {
                throw;
            }



        }


        public async Task<List<TecnicoEquipoDTO>> ListaTodosTecnicos()
        {
            try
            {
                List<TecnicoDTO> listaTecnicos = await this._tecnicoService.Lista();

                if (listaTecnicos == null) {

                }

                List<TecnicoEquipoDTO> listaDTO = new List<TecnicoEquipoDTO>();
                List<TecnicoDTO> Lista = this._mapper.Map<List<TecnicoDTO>>(listaTecnicos);

                Lista.ForEach((item) =>
                {
                    TecnicoEquipoDTO EJDTO = new TecnicoEquipoDTO();
                    //JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);
                    EJDTO.tecnico = item;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.IdEquipo = 0;
                    EJDTO.Funcion = "";
                    listaDTO.Add(EJDTO);



                });

                return listaDTO;
            }
            catch
            {
                throw;
            }



        }


        public async Task<List<TecnicoEquipoDTO>> ListaTecnicosEquipoNoEstan(int idEquipo)
        {
            try {
                List<TecnicoEquipoDTO> lista = await this.ListaTecnicosUnEquipo(idEquipo);
                List<TecnicoEquipoDTO> listaTodos = await this.ListaTodosTecnicos();




                List<TecnicoEquipoDTO> listaDTO = this.ListaTecnicosEquipoNoEstan2(listaTodos, lista);




                return listaDTO;
            }
            catch
            {
                throw;
            }

        }

        private List<TecnicoEquipoDTO> ListaTecnicosEquipoNoEstan2(List<TecnicoEquipoDTO> tecnicos, List<TecnicoEquipoDTO> tecnicos2) {
            foreach (TecnicoEquipoDTO item in tecnicos2)
            {
                for (int i = 0; i < tecnicos.Count; i++)
                {
                    if (item.tecnico.Id== tecnicos[i].tecnico.Id) {
                        tecnicos.RemoveAt(i);
                    }
                }
            }

            return tecnicos;

        }





        public async Task<List<TecnicoEquipoDTO>> ListaTecnicosEquipo() {
            try
            {
                var listaEquipoTecnicos= await _equipoTecnicoRepository.Consultar();
                List<TecnicoEquipoDTO> listaDTO = new List<TecnicoEquipoDTO>();
                List<TecnicoEquipo> Lista = listaEquipoTecnicos.ToList();

                Lista.ForEach(async item=>
                {
                    TecnicoEquipoDTO EJDTO = null;
                    TecnicoDTO JDTO = await _tecnicoService.BuscarTecnico(item.Id);
                    EJDTO.tecnico= JDTO;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.IdEquipo = (int)item.IdEquipo;
                    EJDTO.Funcion = item.Funcion;
                    listaDTO.Add(EJDTO);



                });


                return listaDTO;

            }
            catch
            {
                throw;

            }

        }

        public async Task<List<TecnicoEquipoDTO>> Lista()
        {
            try
            {
                var listaEquipoJugadores = await _equipoTecnicoRepository.Consultar();
                List<TecnicoEquipoDTO> listaDTO = new List<TecnicoEquipoDTO>();
                List<TecnicoEquipo> Lista = listaEquipoJugadores.ToList();

                Lista.ForEach(async item=>
                {
                    TecnicoEquipoDTO EJDTO = null;
                    TecnicoDTO JDTO = await _tecnicoService.BuscarTecnico(item.Id);
                    EJDTO.tecnico = JDTO;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);

                    EJDTO.listaFotos = await this._fotoTecnicoEquipoService.BuscarFotoSTecnicoEquipo(EJDTO);
                    EJDTO.IdEquipo = (int)item.IdEquipo;
                    EJDTO.Funcion = item.Funcion;
                    listaDTO.Add(EJDTO);



                });


                return listaDTO;

            }
            catch
            {
                throw;

            }
        }






        public async Task<EquipoDTO> ADDUpdateDeleteTecnicosEquipo(EquipoDTO equipo)
        {
            //EquipoDTO equipo1 = await this._equipoService.BuscarEquipo(equipo.IdEquipo);

            //Lista de todos los tecnicos
            List<TecnicoEquipoDTO> listaDTO = await this.ListaTecnicosUnEquipo(equipo.IdEquipo);
            List<TecnicoEquipoDTO> listaADD = new List<TecnicoEquipoDTO>();
            List<TecnicoEquipoDTO> listaRemove = await this.ListaTecnicosUnEquipo(equipo.IdEquipo); 

            var encontrado = false;
            var rest = false;

            if (listaDTO.Count != 0)
            {
                for (int i = 0; i < equipo.TecnicoEquipos.Count; i++)
                {


                    encontrado = false;
                    for (int j = 0; j < listaDTO.Count; j++)
                    {
                        encontrado = false;
                        if (equipo.TecnicoEquipos[i].tecnico.Id == listaDTO[j].tecnico.Id && equipo.TecnicoEquipos[i].IdEquipo== listaDTO[j].IdEquipo)
                        {
                            encontrado = true;

                            //jugadores mpdificados?
                            if (equipo.TecnicoEquipos[i].Funcion != listaDTO[j].Funcion)
                            {


                                try
                                {

                                    listaDTO[j].Funcion = equipo.TecnicoEquipos[i].Funcion;

                                    /*EquipoJugador jugador = new EquipoJugador();
                                    jugador.Id = equipo.EquipoJugadores[i].Id;
                                    jugador.IdEquipo= equipo.EquipoJugadores[i].Equipo;
                                    jugador.IdJugador= equipo.EquipoJugadores[i].Jugador.IdJugador;
                                    jugador.Dorsal= equipo.EquipoJugadores[i].Dorsal;*/
                                    
                                    
                                    var jugadorModelo = _mapper.Map<TecnicoEquipo>(listaDTO[j]);
                                    jugadorModelo.IdTecnico = listaDTO[j].tecnico.Id;
                                    /*jugadorModelo.IdEquipo = listaDTO[j].IdEquipo;
                                    jugadorModelo.IdTecnico = listaDTO[j].tecnico.Id;*/


                                    bool respuesta = await this._equipoTecnicoRepository.Editar(jugadorModelo);

                                    //bool respuesta = await _equipoRepository.Editar(equipoEncontrado);
                                    //if (!respuesta)
                                    //{
                                    //  throw new TaskCanceledException("No se pudo editar");
                                    //}

                                    foreach (TecnicoEquipoDTO item in listaRemove)
                                    {
                                        if (item.tecnico.Id == listaDTO[j].tecnico.Id)
                                        {
                                            var t = listaRemove.Remove(item);
                                            break;
                                        }
                                    }

                                }
                                catch (Exception ex) { }




                            }
                            else {
                                foreach (TecnicoEquipoDTO item in listaRemove) {
                                    if (item.tecnico.Id == listaDTO[j].tecnico.Id) {
                                        var t =  listaRemove.Remove(item);
                                        break;
                                    }
                                }
                               
                            }

                            if (equipo.TecnicoEquipos[i].listaFotos.Count != 0)
                            {

                               /* foreach (FotoJugadorEquipoDTO foto in equipo.EquipoJugadores[i].fotos)
                                {*/
                                    if (equipo.TecnicoEquipos[i].listaFotos[0].imagen != null)
                                    {
                                        try
                                        {
                                       

                                            if (listaDTO[j].listaFotos[0].Id== 0)
                                            {
                                                var te = _mapper.Map<Tecnico>(equipo.TecnicoEquipos[i].tecnico);

                                                var res = await this._fotoTecnicoEquipoService.CrearFoto(listaDTO[j].listaFotos[0], te);
                                                if (res != null)
                                                {
                                                    var t = 10;
                                                }
                                            }
                                            else
                                            {
                                            //listaDTO[j].listaFotos[0].imagen = equipo.TecnicoEquipos[i].listaFotos[0].imagen;
                                            var res = await this._fotoTecnicoEquipoService.Editar(listaDTO[j].listaFotos[0]);

                                            }

                                        }
                                        catch (Exception ex) { }

                                        //Creamos la imagen y borramos la anterior

                                    }
                                else
                                {
                                    var te = _mapper.Map<Tecnico>(equipo.TecnicoEquipos[i].tecnico);

                                    var res = await this._fotoTecnicoEquipoService.CrearFoto(equipo.TecnicoEquipos[i].listaFotos[0], te);
                                }
                            }
                           /* }*/


                        }
                        if (encontrado == true)
                        {
                            encontrado = true;
                            break;
                            
                        }


                    }


                    //sabemos que es un jugadorEquipo ya creado, ergo solo se podra modificar (dorsal e imagen) o dejar como esta
                    if (encontrado == false)
                    {
                        //rest = equipo.EquipoJugadores.Remove(equipo.EquipoJugadores[i]);
                        listaADD.Add(equipo.TecnicoEquipos[i]);
                        continue;

                    }






                    //ir eliminando de la lista los jugadores segun se vayan eliminando y añadiendo a la BBDD los que queden al final del recorrido seran los que no  esta y por lo tanto hay que eliminar



                }



                try
                {


                    if ( listaRemove.Count != 0) //?? 
                    {



                        foreach (TecnicoEquipoDTO item in listaRemove)
                        {


                            encontrado = false;
                          


                                

                                    await this.Eliminar(equipo, item);
                                    
                                

                        }
                    }
                    //crear todos los jugadores



                }
                catch (Exception ex) { }




                try
                {


                    if (listaADD.Count != 0) //?? 
                    {



                        foreach (TecnicoEquipoDTO item in listaADD)
                        {



                           await this.Crear(item);





                        }
                    }
                    //crear todos los jugadores



                }
                catch (Exception ex) { }

                return equipo;
            }
            else {

                foreach (TecnicoEquipoDTO item in equipo.TecnicoEquipos)
                {



                   await  this.Crear(item);





                }

            }
            return equipo;
        }

   /*     public async Task<bool> Editar(FotoJugadorEquipoDTO modelo)
        {
            try
            {
                var equipoModelo = _mapper.Map<EquipoJugador>(modelo);
                var equipoEncontrado = await _equipoJugadorRepository.Obtener(u => u.IdEquipo == equipoModelo.IdEquipo);

                if (equipoEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                equipoEncontrado.IdJugador = equipoModelo.IdJugador;
                equipoEncontrado.Id = equipoModelo.Id;
                equipoEncontrado.IdEquipo = equipoModelo.IdEquipo;
                equipoEncontrado.Dorsal = equipoModelo.Dorsal;
               

                bool respuesta = await _equipoJugadorRepository.Editar(equipoEncontrado);
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
        }*/

        public async Task<bool> Editar(TecnicoEquipoDTO modelo)
        {
            try
            {
                var equipoModelo = _mapper.Map<TecnicoEquipo>(modelo);
                var equipoEncontrado = await _equipoTecnicoRepository.Obtener(u => u.IdEquipo == equipoModelo.IdEquipo);

                if (equipoEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                equipoEncontrado.IdTecnico = equipoModelo.IdTecnico;
                equipoEncontrado.Id = equipoModelo.Id;
                equipoEncontrado.IdEquipo = equipoModelo.IdEquipo;
                equipoEncontrado.Funcion = equipoModelo.Funcion;


                bool respuesta = await _equipoTecnicoRepository.Editar(equipoEncontrado);
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
    }
}
