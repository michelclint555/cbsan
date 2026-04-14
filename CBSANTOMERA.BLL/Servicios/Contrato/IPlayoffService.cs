using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IPlayoffService
    {
        Task GenerarPlayoffsAsync(
            int competicionId,
            int faseOrigenId,
            int topEquipos,
            int partidosPorSerie, // 1, 3, 5
            bool idaVuelta);
        Task IniciarPlayoffAsync(int competicionId);
        Task GenerarSiguienteRondaAsync(int fasePlayoffId);
    }
}
