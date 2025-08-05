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
    public interface IFotoTecnicoEquipoService
    {
        Task<List<FotoTecnicoEquipoDTO>> Lista();
       // Task<FotoJugadorEquipoDTO> Crear(FotoJugadorEquipoDTO modelo, EquipoDTO equipo);
        Task<bool> Editar(FotoTecnicoEquipoDTO modelo);
        Task<TecnicoEquipoDTO> CrearFotos(TecnicoEquipoDTO tecnico, List<FotoTecnicoEquipoDTO> fotos);
        Task<FotoTecnicoEquipoDTO> CrearFoto(FotoTecnicoEquipoDTO modelo, Tecnico tecnico);
        Task<bool> Eliminar(int id);
        Task<FotoTecnicoEquipoDTO> BuscarFotoTecnicoEquipo(int id);
        Task<bool> CrearFotoSTecnico(List<FotoTecnicoEquipoDTO> modelos, int equipo);
        Task<bool> EliminarFotoSTecnico(List<FotoTecnicoEquipoDTO> modelos, int idequipo);
        Task<List<FotoTecnicoEquipoDTO>> BuscarFotoSTecnicoEquipo(TecnicoEquipoDTO tecnico); //OBTENGO FOTOS DE UN JUGADOR/EQUIPOS
        Task<FotoTecnicoEquipoDTO> CrearFotoDefault(FotoTecnicoEquipoDTO modelo);
    }
}
