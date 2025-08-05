using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IFotoJugadorEquipoService
    {
        Task<List<FotoJugadorEquipoDTO>> Lista();
       // Task<FotoJugadorEquipoDTO> Crear(FotoJugadorEquipoDTO modelo, EquipoDTO equipo);
        Task<bool> Editar(FotoJugadorEquipoDTO modelo);
        Task<EquipoJugadorDTO> CrearFotos(EquipoJugadorDTO jugador, List<FotoJugadorEquipoDTO> fotos);
        Task<FotoJugadorEquipoDTO> CrearFoto(FotoJugadorEquipoDTO modelo, Jugador jugador);
        Task<bool> Eliminar(int id);
        Task<FotoJugadorEquipoDTO> BuscarFotoJugadorEquipo(int id);
        Task<bool> CrearFotoSJugador(List<FotoJugadorEquipoDTO> modelos, int equipo);
        Task<bool> EliminarFotoSJugador(List<FotoJugadorEquipoDTO> modelos, int idequipo);
        Task<List<FotoJugadorEquipoDTO>> BuscarFotoSJugadorEquipo(EquipoJugadorDTO jugador); //OBTENGO FOTOS DE UN JUGADOR/EQUIPOS
    }
}
