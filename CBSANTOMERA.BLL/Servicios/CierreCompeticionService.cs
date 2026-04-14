using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class CierreCompeticionService : ICierreCompeticionService
    {
        private readonly IGenericRepository<Partido> _partidoRepo;
        private readonly IPartidoService _partidoService;
        private readonly IGenericRepository<Jornada> _jornadaRepo;
        private readonly IGenericRepository<FasesCompeticion> _faseRepo;
        private readonly IFaseCompeticionService _faseService;
        private readonly IPlayoffService _playoffService;
        private readonly IMapper _mapper;

        public CierreCompeticionService(
            IGenericRepository<Partido> partidoRepo,
            IGenericRepository<Jornada> jornadaRepo,
            IGenericRepository<FasesCompeticion> faseRepo,
            IFaseCompeticionService faseService,
            IPartidoService partidoService,  IMapper mapper, IPlayoffService playoffService)
        {
            _partidoRepo = partidoRepo;
            _jornadaRepo = jornadaRepo;
            _faseRepo = faseRepo;
            _faseService = faseService;
            _partidoService = partidoService;
            this._mapper = mapper;
            _playoffService = playoffService;
        }

        public async Task PartidoFinalizadoAsync(int partidoId)
        {
            var partido = await _partidoRepo.Obtener(p => p.Id == partidoId);
            if (partido == null) return;

            var partidosJornada = await _partidoRepo.Consultar(p => p.Jornada == partido.Jornada);

            bool jornadaCompleta = partidosJornada.All(p => p.Estado == "Finalizado");
            if (!jornadaCompleta) return;

            await JornadaActualizadaAsync(partido.Jornada ?? 0);
        }


        public async Task JornadaActualizadaAsync(int jornadaId)
        {
            var jornada = await _jornadaRepo.Obtener(j => j.Id == jornadaId);
            if (jornada == null || jornada.Estado == "Cerrada") return;

            jornada.Estado = "Cerrada";
            jornada.FechaModificacion = DateTime.UtcNow;
            await _jornadaRepo.Editar(jornada);

            if (!jornada.Fase.HasValue) return;

            var fase = await _faseRepo.ObtenerUnModelo(f => f.Id == jornada.Fase);
            if (fase == null) return;

            var fasesCompeticion = await _faseRepo.Consultar(f => f.Competicion == fase.Competicion);
            bool faseCompleta = fasesCompeticion.All(f => f.Estado == "Cerrada");

            if (faseCompleta)
            {
                await _faseService.CerrarFaseAsync(jornada.Fase.Value);
            }

            // Si es fase de playoffs, generamos la siguiente ronda
            /*if (fase.TipoFase != TipoFase.Liga)
            {
                await _playoffService.GenerarSiguienteRondaAsync(fase.Id);
            }*/

            // Si la fase final es cerrada, se cierra la competición
            var competicion = await _faseRepo.ObtenerUnModelo(f => f.Id == fase.Competicion);
            if (competicion != null && competicion.Estado != "Finalizada")
            {
                bool competicionCerrada = fasesCompeticion.All(f => f.Estado == "Cerrada");
                if (competicionCerrada)
                {
                    competicion.Estado = "Finalizada";
                    await _faseRepo.Editar(competicion);
                }
            }
        }



        public async Task GenerarCalendarioLigaAsync(
         int competicionId,
         int faseId,
         List<int> equiposIds,
         DateTime fechaInicio,
         int numVueltas,
         int diasEntreJornadas = 7)
        {
            // 🔒 Validación: evitar duplicados
            var existentes = await this._jornadaRepo.Consultar(j =>
                j.Competicion == competicionId &&
                j.Fase == faseId);

            if (existentes.Any())
                throw new InvalidOperationException(
                    "La competición ya tiene calendario generado");

            int jornadaGlobal = 1;

            for (int vuelta = 1; vuelta <= numVueltas; vuelta++)
            {
                var equipos = new List<int>(equiposIds);

                if (equipos.Count % 2 != 0)
                    equipos.Add(-1); // descanso

                int numEquipos = equipos.Count;
                int jornadasTotales = numEquipos - 1;
                int partidosPorJornada = numEquipos / 2;

                for (int j = 1; j <= jornadasTotales; j++)
                {
                    var jornada = new Jornada
                    {
                        Competicion = competicionId,
                        Fase = faseId,
                        Numero = jornadaGlobal++,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaInicio.AddDays(1),
                        Estado = "Pendiente",
                        FechaCreacion = DateTime.UtcNow
                    };

                    await this._jornadaRepo.Crear(jornada);

                    for (int i = 0; i < partidosPorJornada; i++)
                    {
                        int local = equipos[i];
                        int visitante = equipos[numEquipos - 1 - i];

                        if (local == -1 || visitante == -1)
                            continue;

                        if (vuelta % 2 == 0)
                            (local, visitante) = (visitante, local);

                        var partido = new Partido
                        {
                            Local = local,
                            Visitante = visitante,
                            Jornada = jornada.Id!.Value,
                            Fecha = fechaInicio,
                            Estado = "Programado",
                            Ubicacion = "Por asignar",
                            PuntosLocal = 0,
                            PuntosVisitante = 0,
                            FechaCreacion = DateTime.UtcNow
                        };
                        await _partidoService.CrearAsync(await this._partidoService.CrearAsync(this._mapper.Map<PartidoDTO>(partido)));
                    }

                    // 🔄 Algoritmo Berger
                    int ultimo = equipos[^1];
                    equipos.RemoveAt(equipos.Count - 1);
                    equipos.Insert(1, ultimo);

                    fechaInicio = fechaInicio.AddDays(diasEntreJornadas);
                }
            }
        }


        public async Task CerrarJornadaSiProcedeAsync(int jornadaId)
        {
            var jornada = await this._jornadaRepo.ObtenerUnModelo(j => j.Id == jornadaId);

            if (jornada == null)
                throw new Exception("La jornada no existe");

            if (jornada.Estado == "Cerrada")
                return;

            // 1️⃣ Obtener partidos de la jornada
            var partidos = await this._partidoRepo.Consultar(p=> p.Jornada == jornadaId);

            bool todasFinalizadas = partidos.All(p => p.Estado == "Finalizado");

            if (!todasFinalizadas)
                return;

            // 2️⃣ Cerrar jornada
            jornada.Estado = "Cerrada";
            jornada.FechaModificacion = DateTime.UtcNow;

            await this._jornadaRepo.Editar(jornada);

            // 3️⃣ ¿Todas las jornadas de la fase están cerradas?
            if (jornada.Fase.HasValue)
            {
                var jornadasFase = await this._jornadaRepo.Consultar(j =>
                    j.Fase == jornada.Fase.Value);

                bool faseFinalizada = jornadasFase.All(j => j.Estado == "Cerrada");

                if (faseFinalizada)
                {
                    await this._faseService.CerrarFaseAsync(jornada.Fase.Value);
                }
            }
        }
    }

}
