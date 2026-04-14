using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ICierreCompeticionService
    {
        Task PartidoFinalizadoAsync(int partidoId);
        Task JornadaActualizadaAsync(int jornadaId);
    }

}
