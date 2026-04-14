using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ICompeticionService
    {

        Task<List<CompeticionDTO>> Lista();
        Task<List<CompeticionDTO>> Lista(int idTemporada);
        Task<List<CompeticionDTO>> Equipo_Temporada(int idEquipo);
        Task<List<CompeticionDTO>> Lista(int idEquipo, int temporada);
        Task<CompeticionDTO> VerCompeticion(int id);
        Task<bool> EditarCompeticion(CompeticionDTO modelo);
        Task<CompeticionDTO> CrearCompeticion(CompeticionDTO modelo);
        Task<bool> EliminarCompeticion(int id);
        Task<List<List<JornadaDTOSimple>>> IniciarCompeticion(CompeticionDTO modelo);
        Task<bool> GuardarCalendario(CompeticionDTO modelo);
        Task IniciarCompeticionAsync(int competicionId);
        Task ResetearFasesAsync(int competicionId);


    }
}
