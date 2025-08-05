using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class LigaDTO
    {
        public int Id { get; set; }

        public int? Competicion { get; set; }

        public int? Puntos { get; set; }

        public int? PartidosJugados { get; set; }

        public int? PartidosGanados { get; set; }

        public int? PartidosJugadosLocal { get; set; }

        public int? PartidosPerdidos { get; set; }

        public int? PartidosJugadosVisitante { get; set; }

        public int? PartidosGanadosVisitante { get; set; }

        public int? PuntosTotalesVisitante { get; set; }

        public int? PuntosTotalesLocal { get; set; }

        public int? PartidosGanadosLocal { get; set; }

        public int? PatidosPerdidosVisitante { get; set; }

        public int? PartidosPerdidosLocal { get; set; }

        public int? Racha { get; set; }

        public int? DifetenciaPuntos { get; set; }

        public EquipoDTO Equipo { get; set; }

        public int? Posicion { get; set; }

        public int? Fase { get; set; }

        public Liga ToModel()
        {
            return new Liga
            {
                Id = this.Id,
                Competicion = this.Competicion,
                Puntos = this.Puntos,
                PartidosJugados = this.PartidosJugados,
                PartidosGanados = this.PartidosGanados,
                PartidosJugadosLocal = this.PartidosJugadosLocal,
                PartidosPerdidos = this.PartidosPerdidos,
                PartidosJugadosVisitante = this.PartidosJugadosVisitante,
                PartidosGanadosVisitante = this.PartidosGanadosVisitante,
                PuntosTotalesVisitante = this.PuntosTotalesVisitante,
                PuntosTotalesLocal = this.PuntosTotalesLocal,
                PartidosGanadosLocal = this.PartidosGanadosLocal,
                PatidosPerdidosVisitante = this.PatidosPerdidosVisitante,
                PartidosPerdidosLocal = this.PartidosPerdidosLocal,
                Racha = this.Racha,
                DifetenciaPuntos = this.DifetenciaPuntos,
                Equipo = this.Equipo.IdEquipo,
                Posicion = this.Posicion,
                Fase = this.Fase
            };
        }

        public static LigaDTO ToDTO(Liga modelo)
        {
            return new LigaDTO
            {
                Id = modelo.Id,
                Competicion = modelo.Competicion,
                Puntos = modelo.Puntos,
                PartidosJugados = modelo.PartidosJugados,
                PartidosGanados = modelo.PartidosGanados,
                PartidosJugadosLocal = modelo.PartidosJugadosLocal,
                PartidosPerdidos = modelo.PartidosPerdidos,
                PartidosJugadosVisitante = modelo.PartidosJugadosVisitante,
                PartidosGanadosVisitante = modelo.PartidosGanadosVisitante,
                PuntosTotalesVisitante = modelo.PuntosTotalesVisitante,
                PuntosTotalesLocal = modelo.PuntosTotalesLocal,
                PartidosGanadosLocal = modelo.PartidosGanadosLocal,
                PatidosPerdidosVisitante = modelo.PatidosPerdidosVisitante,
                PartidosPerdidosLocal = modelo.PartidosPerdidosLocal,
                Racha = modelo.Racha,
                DifetenciaPuntos = modelo.DifetenciaPuntos,
                //Equipo = this.Equipo,
                Posicion = modelo.Posicion,
                Fase = modelo.Fase
            };
        }

        public void Inicializar()
        {
            this.Puntos ??= 0;
            this.PartidosJugados ??= 0;
            this.PartidosGanados ??= 0;
            this.PartidosJugadosLocal ??= 0;
            this.PartidosPerdidos ??= 0;
            this.PartidosJugadosVisitante ??= 0;
            this.PartidosGanadosVisitante ??= 0;
            this.PuntosTotalesVisitante ??= 0;
            this.PuntosTotalesLocal ??= 0;
            this.PartidosGanadosLocal ??= 0;
            this.PatidosPerdidosVisitante ??= 0;
            this.PartidosPerdidosLocal ??= 0;
            this.Racha ??= 0;
            this.DifetenciaPuntos ??= 0;
            //this.Equipo ??= 0;
            this.Posicion ??= 0;
            this.Fase ??= 0;
        }
    }
}
