using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ILigaService
    {

        Task<List<LigaDTO>> Lista();
        //Task<List<LigaDTO>> Lista(int idTemporada);
        //Task<List<CompeticionDTO>> Equipo_Temporada(int idEquipo);
        //Task<List<CompeticionDTO>> Lista(int idEquipo, int temporada);
        Task<List<LigaDTO>> VerLiga(int id);
        Task<bool> Editar(LigaDTO modelo);
        Task<LigaDTO> Crear(LigaDTO modelo);
       
        Task<List<List<JornadaDTOSimple>>> ListaJornadas(CompeticionDTO competicion);
        //Task<List<LigaDTO>> Lista(int idTemporada);
        //Task<List<CompeticionDTO>> Equipo_Temporada(int idEquipo);
        //Task<List<CompeticionDTO>> Lista(int idEquipo, int temporada);
        Task<JornadaDTO> VerJornada(int id);
        Task<bool> Editar(JornadaDTOSimple modelo);
        Task<JornadaDTO> Crear(JornadaDTO modelo);

        Task<bool> EliminarJornadaEntera(int idFase, int competicion);
        Task<bool> EliminarLigaEntera(int idCompeticion);
        Task<bool> InicializarLiga(List<EquipoDTO> equipos, FaseCompeticionDTO fase);
        Task<List<List<JornadaDTOSimple>>> InicializarJornadas(List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase);
        Task<List<List<JornadaDTOSimple>>> InicializarJornadasCalendarioPDF(List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase);
        Task<bool> EliminarJornadaEnteraPorCompeticion(int competicion);


    }
}
