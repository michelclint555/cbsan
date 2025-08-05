using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IEquipoService
    {
        Task<List<EquipoDTO>> Lista();
        Task<List<EquipoRivalDTO>> ListaRival();
        Task<EquipoDTO> Crear(EquipoDTO modelo);
        Task<EquipoRivalDTO> Crear(EquipoRivalDTO modelo);
        Task<bool> Editar(  EquipoRivalDTO modelo);
        Task<bool> Eliminar(int id);
        Task<List<EquipoDTO>> ListaMisEquipos();
        Task<EquipoDTO> CrearMiEquipo(EquipoDTO modelo);
        Task<EquipoDTO> BuscarEquipo(int id);
        Task<EquipoDTO> CrearFotoEquipo(EquipoDTO equipo);
        Task<EquipoDTO> BuscarEquipoPricipal();
        Task<List<EquipoDTO>> BuscarEquiposCantera();
            Task<List<EquipoDTO>> ListaEquiposClub(int id);
        Task<List<EquipoDTO>> ListaEquiposClub(int id, int temporada);
        Task<List<EquipoDTO>> ListaEquiposClubCategoria(int idClub, int idCategoria );
        Task<List<EquipoDTO>> ListaEquiposMiClub(int temporada);
        /*Task<bool> EliminarMiEquipo(int id);
Task<List<EquipoDTO>> ListaMiEquipo();
Task<EquipoDTO> CrearMiEquipo(EquipoDTO modelo);
Task<bool> EditarMiEquipo(EquipoDTO modelo);*/


    }
}
