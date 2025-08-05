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
    public class EquipoJugadorService : IEquipoJugadorService
    {
        private readonly IGenericRepository<EquipoJugador> _equipoJugadorRepository;
        private readonly IJugadorService _jugadorService;
        private readonly IEquipoService _equipoService;
        private readonly IFotoJugadorEquipoService _fotoJugadorEquipoService;
        //  private readonly IGenericRepository<FotoJugadorEquipo> _fotoJugadorequipoRepository;
        // private readonly IGenericRepository<Equipo> _equipoRepository;

        // private readonly IGenericRepository<Club> _clubRepository;
        private readonly IMapper _mapper;
        private readonly int _Miclub = 38;
        private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";

        public EquipoJugadorService(IJugadorService jugadorService, IFotoJugadorEquipoService fotoJugadorEquipoService, IEquipoService _equipoService, IGenericRepository<EquipoJugador> equipoJugadorRepository, IMapper mapper)
        {

            this._equipoService = _equipoService;
            //_jugadorRepository = jugadorRepository;
            _fotoJugadorEquipoService = fotoJugadorEquipoService;
            _equipoJugadorRepository = equipoJugadorRepository;
            _mapper = mapper;
            this._jugadorService = jugadorService;
        }

        public async Task<EquipoJugadorDTO> Crear(EquipoJugadorDTO modelo)
        {
            try
            {

                EquipoJugador j = _mapper.Map<EquipoJugador>(modelo);
                j.Dorsal = modelo.Dorsal;
                j.IdEquipo = modelo.Equipo;
                j.IdJugador = modelo.Jugador.IdJugador;
                j.Id = 0;
                var listaEquipoJugadores = await _equipoJugadorRepository.Consultar((i) => i.IdEquipo == modelo.Equipo);
                List<EquipoJugador> Lista = listaEquipoJugadores.ToList();
                foreach (var item in Lista)
                {
                    if (item.Id == modelo.Id && item.IdJugador == modelo.Jugador.IdJugador && item.IdEquipo == modelo.Equipo && item.Dorsal == modelo.Dorsal) {
                        throw new TaskCanceledException("El jugador ya se encuentra en este equipo");
                    }
                }


                var equipoJugadorCreado = await _equipoJugadorRepository.Crear(j);
                modelo.Id = equipoJugadorCreado.Id;

                if (equipoJugadorCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }
                
                if (modelo.fotos.Count != 0) {

                   await this._fotoJugadorEquipoService.CrearFotos(modelo, modelo.fotos);



                }

                return modelo;

            }
            catch
            {
                throw;

            }
        }






        public async Task<bool> Eliminar(EquipoDTO equipo, EquipoJugadorDTO jugador)
        {
            try
            {
                //var equipoEncontrado = await _equipoJugadorRepository.Obtener(p => p.Id == id);
                //if (equipoEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                if(await this._fotoJugadorEquipoService.EliminarFotoSJugador(jugador.fotos, equipo.IdEquipo)){

                    EquipoJugador j = new EquipoJugador();
                    j.Id = jugador.Id;
                    j.IdEquipo = jugador.Equipo;
                    j.Dorsal = jugador.Dorsal;
                    j.IdJugador = jugador.Jugador.IdJugador;


                    if (await this._equipoJugadorRepository.Eliminar(j))
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


        public async Task<List<EquipoJugadorDTO>> ListaJugadoresUnEquipo(int idEquipo)
        {
            try
            {
                var listaEquipoJugadores = await _equipoJugadorRepository.Consultar((i) => i.IdEquipo == idEquipo);

                List<EquipoJugadorDTO> listaDTO = new List<EquipoJugadorDTO>();
                List<EquipoJugador> Lista = listaEquipoJugadores.ToList();

                foreach (EquipoJugador equipoJugador in Lista)
                {
                    EquipoJugadorDTO EJDTO = new EquipoJugadorDTO();


                    JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);

                    if (JDTO == null) {
                        continue;
                    }
                    EJDTO.Jugador = JDTO;

                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.Equipo = (int)equipoJugador.IdEquipo;
                    EJDTO.Dorsal = (int)equipoJugador.Dorsal;
                    EJDTO.Id = (int)equipoJugador.Id;
                    EJDTO.fotos = await this._fotoJugadorEquipoService.BuscarFotoSJugadorEquipo(EJDTO);
                    listaDTO.Add(EJDTO);



                }

                return listaDTO;
            }
            catch {
                throw;
            }



        }


        public async Task<List<EquipoJugadorDTO>> ListaTodosJugadores()
        {
            try
            {
                List<JugadorDTO> listaJugadores = await _jugadorService.Lista();

                if (listaJugadores == null) {

                }

                List<EquipoJugadorDTO> listaDTO = new List<EquipoJugadorDTO>();
                List<JugadorDTO> Lista = this._mapper.Map<List<JugadorDTO>>(listaJugadores);

                Lista.ForEach((jugador) =>
                {
                    EquipoJugadorDTO EJDTO = new EquipoJugadorDTO();
                    //JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);
                    EJDTO.Jugador = jugador;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.Equipo = 0;
                    EJDTO.Dorsal = 0;
                    listaDTO.Add(EJDTO);



                });

                return listaDTO;
            }
            catch
            {
                throw;
            }



        }


        public async Task<List<EquipoJugadorDTO>> ListaJugadoresEquipoNoEstan(int idEquipo)
        {
            try {
                List<EquipoJugadorDTO> lista = await this.ListaJugadoresUnEquipo(idEquipo);
                List<EquipoJugadorDTO> listaTodos = await this.ListaTodosJugadores();




                List<EquipoJugadorDTO> listaDTO = this.ListaJugadoresEquipoNoEstan2(listaTodos, lista);




                return listaDTO;
            }
            catch
            {
                throw;
            }

        }

        private List<EquipoJugadorDTO> ListaJugadoresEquipoNoEstan2(List<EquipoJugadorDTO> jugadores, List<EquipoJugadorDTO> jugadores2) {
            foreach (EquipoJugadorDTO jugador in jugadores2)
            {
                for (int i = 0; i < jugadores.Count; i++)
                {
                    if (jugador.Jugador.IdJugador == jugadores[i].Jugador.IdJugador) {
                        jugadores.RemoveAt(i);
                    }
                }
            }

            return jugadores;

        }





        public async Task<List<EquipoJugadorDTO>> ListaJugadoresEquipo() {
            try
            {
                var listaEquipoJugadores = await _equipoJugadorRepository.Consultar();
                List<EquipoJugadorDTO> listaDTO = new List<EquipoJugadorDTO>();
                List<EquipoJugador> Lista = listaEquipoJugadores.ToList();

                Lista.ForEach(async equipoJugador =>
                {
                    EquipoJugadorDTO EJDTO = null;
                    JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);
                    EJDTO.Jugador = JDTO;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);
                    EJDTO.Equipo = (int)equipoJugador.IdEquipo;
                    EJDTO.Dorsal = (int)equipoJugador.Dorsal;
                    listaDTO.Add(EJDTO);



                });


                return listaDTO;

            }
            catch
            {
                throw;

            }

        }

        public async Task<List<EquipoJugadorDTO>> Lista()
        {
            try
            {
                var listaEquipoJugadores = await _equipoJugadorRepository.Consultar();
                List<EquipoJugadorDTO> listaDTO = new List<EquipoJugadorDTO>();
                List<EquipoJugador> Lista = listaEquipoJugadores.ToList();

                foreach (EquipoJugador equipoJugador in Lista)
                {
                    EquipoJugadorDTO EJDTO = new EquipoJugadorDTO() ;
                    JugadorDTO JDTO = await _jugadorService.BuscarJugador(equipoJugador.IdJugador);

                    if (JDTO == null) {
                        continue;
                    }
                    EJDTO.Jugador = JDTO;
                    //EquipoDTO EDTO = await BuscarEquipo(equipoJugador.IdEquipo);

                    EJDTO.fotos = await _fotoJugadorEquipoService.BuscarFotoSJugadorEquipo(EJDTO);
                    EJDTO.Equipo = (int)equipoJugador.IdEquipo;
                    EJDTO.Dorsal = (int)equipoJugador.Dorsal;
                    listaDTO.Add(EJDTO);



                }


                return listaDTO;

            }
            catch
            {
                throw;

            }
        }






        public async Task<EquipoDTO> ADDUpdateDeleteJugadoresEquipo(EquipoDTO equipo)
        {
            //EquipoDTO equipo1 = await this._equipoService.BuscarEquipo(equipo.IdEquipo);


            List<EquipoJugadorDTO> listaDTO = await this.ListaJugadoresUnEquipo(equipo.IdEquipo);
            List<EquipoJugadorDTO> listaADD = new List<EquipoJugadorDTO>();
            List<EquipoJugadorDTO> listaRemove = await this.ListaJugadoresUnEquipo(equipo.IdEquipo); 

            var encontrado = false;
            var rest = false;

            if (listaDTO.Count != 0)
            {
                for (int i = 0; i < equipo.EquipoJugadores.Count; i++)
                {


                    encontrado = false;
                    for (int j = 0; j < listaDTO.Count; j++)
                    {
                        encontrado = false;
                        if (equipo.EquipoJugadores[i].Id == listaDTO[j].Id && equipo.EquipoJugadores[i].Jugador.IdJugador == listaDTO[j].Jugador.IdJugador)
                        {
                            encontrado = true;

                            //jugadores mpdificados?
                            if (equipo.EquipoJugadores[i].Dorsal != listaDTO[j].Dorsal)
                            {


                                try
                                {

                                    listaDTO[j].Dorsal = equipo.EquipoJugadores[i].Dorsal;

                                    /*EquipoJugador jugador = new EquipoJugador();
                                    jugador.Id = equipo.EquipoJugadores[i].Id;
                                    jugador.IdEquipo= equipo.EquipoJugadores[i].Equipo;
                                    jugador.IdJugador= equipo.EquipoJugadores[i].Jugador.IdJugador;
                                    jugador.Dorsal= equipo.EquipoJugadores[i].Dorsal;*/
                                    var jugadorModelo = _mapper.Map<EquipoJugador>(listaDTO[j]);
                                    jugadorModelo.IdEquipo = listaDTO[j].Equipo;
                                    jugadorModelo.IdJugador = listaDTO[j].Jugador.IdJugador;


                                    bool respuesta = await this._equipoJugadorRepository.Editar(jugadorModelo);

                                    //bool respuesta = await _equipoRepository.Editar(equipoEncontrado);
                                    //if (!respuesta)
                                    //{
                                    //  throw new TaskCanceledException("No se pudo editar");
                                    //}



                                }
                                catch (Exception ex) { }




                            }
                            else {
                                foreach (EquipoJugadorDTO item in listaRemove) {
                                    if (item.Id == listaDTO[j].Id) {
                                        var t =  listaRemove.Remove(item);
                                        break;
                                    }
                                }
                               
                            }

                            if (equipo.EquipoJugadores[i].fotos.Count != 0)
                            {

                               /* foreach (FotoJugadorEquipoDTO foto in equipo.EquipoJugadores[i].fotos)
                                {*/
                                    if (equipo.EquipoJugadores[i].fotos[0].imagen != null)
                                    {
                                        try
                                        {


                                            if (equipo.EquipoJugadores[i].fotos[0].Id == 0)
                                            {
                                                var te = _mapper.Map<Jugador>(equipo.EquipoJugadores[i].Jugador);

                                                var res = await this._fotoJugadorEquipoService.CrearFoto(equipo.EquipoJugadores[i].fotos[0], te);
                                                if (res != null)
                                                {
                                                    var t = 10;
                                                }
                                            }
                                            else
                                            {
                                                var res = await this._fotoJugadorEquipoService.Editar(equipo.EquipoJugadores[i].fotos[0]);

                                            }

                                        }
                                        catch (Exception ex) { }

                                        //Creamos la imagen y borramos la anterior

                                    }
                                    else {
                                        var te = _mapper.Map<Jugador>(equipo.EquipoJugadores[i].Jugador);

                                        var res = await this._fotoJugadorEquipoService.CrearFoto(equipo.EquipoJugadores[i].fotos[0], te);
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
                        listaADD.Add(equipo.EquipoJugadores[i]);
                        continue;

                    }






                    //ir eliminando de la lista los jugadores segun se vayan eliminando y añadiendo a la BBDD los que queden al final del recorrido seran los que no  esta y por lo tanto hay que eliminar



                }



                try
                {


                    if ( listaRemove.Count != 0) //?? 
                    {



                        foreach (EquipoJugadorDTO item in listaRemove)
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



                        foreach (EquipoJugadorDTO item in equipo.EquipoJugadores)
                        {



                            this.Crear(item);





                        }
                    }
                    //crear todos los jugadores



                }
                catch (Exception ex) { }

                return equipo;
            }
            else {

                foreach (EquipoJugadorDTO item in equipo.EquipoJugadores)
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

        public async Task<bool> Editar(EquipoJugadorDTO modelo)
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
        }
    }
}
