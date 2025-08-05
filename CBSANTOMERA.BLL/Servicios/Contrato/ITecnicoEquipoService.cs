using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ITecnicoEquipoService
    {
        Task<List<TecnicoEquipoDTO>> Lista();
        Task<TecnicoEquipoDTO> Crear(TecnicoEquipoDTO modelo);
        Task<bool> Editar(TecnicoEquipoDTO modelo);
        Task<bool> Eliminar(EquipoDTO equipo, TecnicoEquipoDTO tecnico);
        Task<List<TecnicoEquipoDTO>> ListaTecnicosUnEquipo(int idEquipo);
        Task<List<TecnicoEquipoDTO>> ListaTodosTecnicos();
        Task<List<TecnicoEquipoDTO>> ListaTecnicosEquipoNoEstan(int idEquipo);
        Task<EquipoDTO> ADDUpdateDeleteTecnicosEquipo(EquipoDTO equipo);

    }
}
