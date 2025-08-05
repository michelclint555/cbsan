using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IEquipoCompeticionService { 
        Task<bool> Editar(EquipoCompeticionDTO modelo);
        Task<bool> Crear(EquipoCompeticionDTO modelo);
        Task<bool> Eliminar(int idEquipo, int idCompeticion);
        Task <List<EquipoCompeticionDTO>> Listar();
        Task<List<EquipoCompeticionDTO>> Listar(int idCategoria, int idClub,  int idCompeticion);
        Task<List<EquipoCompeticionDTO>> Listar(int idCompeticion);
        Task<bool> EliminarEquiposCompeticion(int idCompeticion);

    }
}
