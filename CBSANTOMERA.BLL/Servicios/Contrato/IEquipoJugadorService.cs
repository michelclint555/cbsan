using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IEquipoJugadorService
    {
        Task<List<EquipoJugadorDTO>> Lista();
        Task<EquipoJugadorDTO> Crear(EquipoJugadorDTO modelo);
        Task<bool> Editar(EquipoJugadorDTO modelo);
        Task<bool> Eliminar(EquipoDTO equipo, EquipoJugadorDTO jugador);
        Task<List<EquipoJugadorDTO>> ListaJugadoresUnEquipo(int idEquipo);
        Task<List<EquipoJugadorDTO>> ListaTodosJugadores();
        Task<List<EquipoJugadorDTO>> ListaJugadoresEquipoNoEstan(int idEquipo);
        //Task<EquipoDTO> ADDUpdateDeleteJugadoresEquipo(EquipoDTO equipo);
        Task<EquipoDTO> SincronizarJugadoresEquipo(EquipoDTO equipo);
    }
}
