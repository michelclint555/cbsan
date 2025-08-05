using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IFaseCompeticionService
    {
        Task<bool> Editar(FaseCompeticionDTO modelo);
        Task<FaseCompeticionDTO> Crear(FaseCompeticionDTO modelo, CompeticionDTO competicion);
        Task<bool> EliminarCompeticion(int id);
        Task <List<FaseCompeticionDTO>> Listar();
        public Task<FaseCompeticionDTO> Ver(int idFase);
        Task<List<FaseCompeticionDTO>> Listar(int idCompeticion);//si es liga devolver listadao de clasificacion
        Task<bool> Eliminar(int id);//Elimina un fase con el id

    }
}
