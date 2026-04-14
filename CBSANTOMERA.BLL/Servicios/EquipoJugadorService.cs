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

                FotoJugadorEquipoDTO modelo1 = new FotoJugadorEquipoDTO();
                modelo1.Jugador = modelo.Jugador.IdJugador;
                modelo1.IdEquipo = modelo.Equipo;
                modelo1.EquipoJugador = modelo.Id;
                var te = _mapper.Map<Jugador>(modelo.Jugador);
                var res = await this._fotoJugadorEquipoService.CrearFoto(modelo1, te);
                if (res != null)
                {
                    var t = 10;
                }
                /* if (modelo.fotos.Count != 0) {

                    await this._fotoJugadorEquipoService.CrearFoto(modelo, modelo.fotos);
                     var te = _mapper.Map<Jugador>(modelo.Jugador);

                     var res = await this._fotoJugadorEquipoService.CrearFoto(modelo.fotos[0], te);


                 }*/

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






        public async Task<EquipoDTO> SincronizarJugadoresEquipo(EquipoDTO equipo)
        {
            // 1. Lista actual en BD
            var jugadoresBD = await this.ListaJugadoresUnEquipo(equipo.IdEquipo);

            // Índices para comparación rápida
            var dicBD = jugadoresBD.ToDictionary(j => j.Jugador.IdJugador);
            var dicRequest = equipo.EquipoJugadores.ToDictionary(j => j.Jugador.IdJugador);

            // 2. Jugadores eliminados
            var eliminados = jugadoresBD
                .Where(j => !dicRequest.ContainsKey(j.Jugador.IdJugador))
                .ToList();

            // 3. Jugadores nuevos
            var nuevos = equipo.EquipoJugadores
                .Where(j => !dicBD.ContainsKey(j.Jugador.IdJugador))
                .ToList();

            // 4. Jugadores comunes (posibles actualizaciones)
            var comunes = equipo.EquipoJugadores
                .Where(j => dicBD.ContainsKey(j.Jugador.IdJugador))
                .ToList();

            // ---------------------------------------
            // ELIMINAR JUGADORES QUE YA NO ESTÁN
            // ---------------------------------------
            foreach (var jugador in eliminados)
            {
                // Eliminar fotos asociadas
                var fotos = await _fotoJugadorEquipoService.BuscarFotoSJugadorEquipo(jugador);
                foreach (var f in fotos)
                    await _fotoJugadorEquipoService.Eliminar(f.Id);

                // Eliminar jugador
                await Eliminar(equipo, jugador);
            }

            // ---------------------------------------
            // CREAR NUEVOS JUGADORES
            // ---------------------------------------
            foreach (var jugadorNuevo in nuevos)
            {
                await Crear(jugadorNuevo);

                // Crear foto si viene o crear foto vacía
               // await _fotoJugadorEquipoService.CrearFoto(,jugadorNuevo);
            }

            // ---------------------------------------
            // ACTUALIZAR JUGADORES EXISTENTES
            // ---------------------------------------
            foreach (var jugadorReq in comunes)
            {
                var jugadorBD = dicBD[jugadorReq.Jugador.IdJugador];

                // Actualizar dorsal si cambió
                if (jugadorReq.Dorsal != jugadorBD.Dorsal)
                {
                    var model = _mapper.Map<EquipoJugador>(jugadorReq);
                    model.IdEquipo = jugadorReq.Equipo;
                    model.IdJugador = jugadorReq.Jugador.IdJugador;

                    await _equipoJugadorRepository.Editar(model);
                }

                // Actualizar foto si es necesario
                await _fotoJugadorEquipoService.Editar(jugadorReq.fotos[0]);
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
