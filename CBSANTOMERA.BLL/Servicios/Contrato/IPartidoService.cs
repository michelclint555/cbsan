using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IPartidoService
    {
        Task<PartidoDTO> CrearAsync(PartidoDTO dto);
        Task<PartidoDTO> ActualizarAsync(int id, PartidoDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<PartidoDTO?> ObtenerPorIdAsync(int id);
        Task<List<PartidoDTO>> ObtenerPorJornadaAsync(int jornadaId);
    }
}
