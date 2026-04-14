using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class PlayoffService : IPlayoffService
    {
        private readonly IGenericRepository<FasesCompeticion> _faseRepo;
        private readonly IGenericRepository<Jornada> _jornadaRepo;
        private readonly IGenericRepository<Partido> _partidoRepo;
        private readonly IGenericRepository<TipoFase> tipofaseRepo;
        private readonly ILigaService _ligaService;

        public PlayoffService(
            IGenericRepository<FasesCompeticion> faseRepo,
            IGenericRepository<Jornada> jornadaRepo,
            IGenericRepository<Partido> partidoRepo,
            ILigaService ligaService, IGenericRepository<TipoFase> tipofaseRepo)
        {
            _faseRepo = faseRepo;
            _jornadaRepo = jornadaRepo;
            _partidoRepo = partidoRepo;
            _ligaService = ligaService;
            this.tipofaseRepo = tipofaseRepo;
        }

        public async Task GenerarPlayoffsAsync(
     int competicionId,
     int faseOrigenId,
     int topEquipos,
     int partidosPorSerie,
     bool idaVuelta)
        {
            // 1️⃣ Clasificación final
            var clasificacion = await _ligaService
                .ObtenerClasificacionAsync(faseOrigenId);

            var equipos = clasificacion
                .OrderBy(c => c.Posicion)
                .Take(topEquipos)
                .Select(c => c.Equipo.IdEquipo)
                .ToList();

            if (topEquipos < 4 || topEquipos % 2 != 0)
                throw new ArgumentException("topEquipos debe ser par y >= 4");

            bool existePlayoff = (await _faseRepo.Consultar(f =>
    f.Competicion == competicionId &&
    f.FasePadre == faseOrigenId))
    .Any();

            if (existePlayoff)
                throw new InvalidOperationException("Los playoffs ya han sido generados");

           var fase=  await ObtenerTipoSiguiente(topEquipos);
            // 2️⃣ Crear fase playoff
            var fasePlayoff = await _faseRepo.Crear(new FasesCompeticion
            {
                Competicion = competicionId,
              
                Nombre = fase.Nombre,
                TipoFase = fase.Id,
                FasePadre = faseOrigenId,
                Estado = "Pendiente",
                NumPartidos = partidosPorSerie,
                IdaVuelta = idaVuelta,
                FechaCreacion = DateTime.UtcNow
            });

            // 3️⃣ Emparejamientos (1v8, 2v7…)
            int numCruces = equipos.Count / 2;

            for (int i = 0; i < numCruces; i++)
            {
                int local = equipos[i];
                int visitante = equipos[equipos.Count - 1 - i];

                var jornada = await _jornadaRepo.Crear(new Jornada
                {
                    Competicion = competicionId,
                    Fase = fasePlayoff.Id,
                    Numero = i + 1,
                    Estado = "Pendiente",
                    FechaCreacion = DateTime.UtcNow
                });

                await CrearSerieAsync(
                    jornada.Id!.Value,
                    local,
                    visitante,
                    fasePlayoff);
            }
        }



        private async Task<TipoFase> ObtenerTipoSiguiente(int equipos)
        {
            int tipoFaseId = equipos switch
            {
                8 => 2,
                4 => 3,
                2 => 4,
                _ => throw new InvalidOperationException("Número de equipos no válido")
            };

            return await this.tipofaseRepo.ObtenerUnModelo(tf => tf.Id == tipoFaseId);
                
        }



        public async Task GenerarSiguienteRondaAsync(int faseId)
        {
            var fase = await _faseRepo.ObtenerUnModelo(f => f.Id == faseId);
            if (fase == null) return;

            var jornadas = await _jornadaRepo.Consultar(j => j.Fase == faseId);

            var clasificados = await ObtenerGanadoresAsync(jornadas, fase);

            if (clasificados.Count < 2)
                return;

            var tipoNuevaFase = await ObtenerTipoSiguiente(clasificados.Count());

            var nuevaFase = await _faseRepo.Crear(new FasesCompeticion
            {
                Competicion = fase.Competicion,
                Nombre = tipoNuevaFase.Nombre,
                TipoFase = tipoNuevaFase.Id,
                FasePadre = fase.Id,
                NumPartidos = fase.NumPartidos,
                IdaVuelta = fase.IdaVuelta,
                Estado = "Pendiente",
                FechaCreacion = DateTime.UtcNow
            });

            for (int i = 0; i < clasificados.Count() / 2; i++)
            {
                int jornadaId = await CrearJornadaAsync(nuevaFase, i + 1);

                await CrearSerieAsync(
                    jornadaId,
                    clasificados[i],
                    clasificados[clasificados.Count - 1 - i],
                    nuevaFase);
            }
        }


        private async Task<List<int>> ObtenerGanadoresAsync(
    IEnumerable<Jornada> jornadas,
    FasesCompeticion fase)
        {
            List<int> ganadores = new();

            int necesarios = (fase.NumPartidos!.Value / 2) + 1;

            foreach (var jornada in jornadas)
            {
                var partidos = await _partidoRepo.Consultar(p =>
                    p.Jornada == jornada.Id &&
                    p.Estado == "Finalizado");

                var equipos = partidos
                    .SelectMany(p => new[] { p.Local, p.Visitante })
                    .Distinct()
                    .ToList();

                if (equipos.Count != 2)
                    continue;

                int a = equipos[0];
                int b = equipos[1];

                int victoriasA = partidos.Count(p =>
                    (p.Local == a && p.PuntosLocal > p.PuntosVisitante) ||
                    (p.Visitante == a && p.PuntosVisitante > p.PuntosLocal));

                int victoriasB = partidos.Count() - victoriasA;

                if (victoriasA >= necesarios) ganadores.Add(a);
                else if (victoriasB >= necesarios) ganadores.Add(b);
            }

            return ganadores;
        }

        private async Task CrearSerieAsync(
    int jornadaId,
    int equipoA,
    int equipoB,
    FasesCompeticion fase)
        {
            int totalPartidos = fase.NumPartidos ?? 1;

            for (int i = 1; i <= totalPartidos; i++)
            {
                bool invertir = fase.IdaVuelta == true && i % 2 == 0;

                await _partidoRepo.Crear(new Partido
                {
                    Jornada = jornadaId,
                    Local = invertir ? equipoB : equipoA,
                    Visitante = invertir ? equipoA : equipoB,
                    Estado = "Programado",
                    Fecha = DateTime.UtcNow.AddDays(i),
                    FechaCreacion = DateTime.UtcNow
                });
            }
        }

        public async Task IniciarPlayoffAsync(int competicionId)
        {
            // 1️⃣ Obtener fase raíz (playoff inicial)
            var faseInicial = await _faseRepo.ObtenerUnModelo(f =>
                f.Competicion == competicionId &&
                f.FasePadre == null &&
                f.TipoFase != null);

            if (faseInicial == null)
                throw new InvalidOperationException(
                    "No existe una fase de playoffs creada");

            if (faseInicial.Estado == "EnCurso")
                return;

            // 2️⃣ Marcar fase como activa
            faseInicial.Estado = "EnCurso";
            faseInicial.FechaModificacion = DateTime.UtcNow;

            await _faseRepo.Editar(faseInicial);

            // 3️⃣ Activar jornadas
            var jornadas = await _jornadaRepo.Consultar(j =>
                j.Fase == faseInicial.Id);

            foreach (var jornada in jornadas)
            {
                if (jornada.Estado == "Pendiente")
                {
                    jornada.Estado = "EnCurso";
                    jornada.FechaModificacion = DateTime.UtcNow;
                    await _jornadaRepo.Editar(jornada);
                }
            }
        }


        private async Task<int> CrearJornadaAsync(
    FasesCompeticion fase,
    int numero)
        {
            var jornada = new Jornada
            {
                Competicion = fase.Competicion!.Value,
                Fase = fase.Id,
                Numero = numero,
                Estado = "Pendiente",
                FechaInicio = DateTime.UtcNow,
                FechaFin = DateTime.UtcNow.AddDays(1),
                FechaCreacion = DateTime.UtcNow
            };

            var creada = await _jornadaRepo.Crear(jornada);

            return creada.Id!.Value;
        }


    }




}


