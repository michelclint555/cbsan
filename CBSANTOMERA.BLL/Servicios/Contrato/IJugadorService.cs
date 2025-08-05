using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IJugadorService
    {
        Task<List<JugadorDTO>> Lista();
        Task<JugadorDTO> Crear(JugadorDTO modelo);
        Task<bool> Editar(JugadorDTO modelo);
        Task<bool> Eliminar(int id);
        Task<JugadorDTO> BuscarJugador(int id);
    }
}
