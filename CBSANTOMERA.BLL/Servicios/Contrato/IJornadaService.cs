using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IJornadaService
    {

        Task<JornadaDTO> CrearAsync(JornadaDTO dto);
        Task<JornadaDTO?> ActualizarAsync(int id, JornadaDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<JornadaDTO?> ObtenerPorIdAsync(int id);
        Task<List<JornadaDTO>> ObtenerPorCompeticionAsync(int competicionId);
        Task<List<JornadaDTO>> ObtenerPorFaseAsync(int faseId);
        //Task CerrarJornadaSiProcedeAsync(int jornadaId);
        
        Task<bool> JornadaEstaAbiertaAsync(int jornadaId);
    }
}
