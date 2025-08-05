using CBSANTOMERA.MODEL;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{



    public class JornadaDTOSimple
    {
        public string Estado { get; set; } = null!;
        public EquipoDTOSimple Local { get; set; }

        public EquipoDTOSimple Visitante { get; set; }
        public DateTime Fecha { get; set; }
        
        public int PuntosLocal { get; set; }
        public int Id { get; set; }
        public int PuntosVisitante { get; set; }

        // public int Liga { get; set; }
        public string Ubicacion { get; set; }
        public int Jornada1 { get; set; }

    }
    public class JornadaDTO
    {
        public int Id { get; set; }
        public string Estado { get; set; } = null!;
        public EquipoDTO Local { get; set; }

        public EquipoDTO Visitante { get; set; }

        public string Ubicacion { get; set; }

        public DateTime Fecha { get; set; }

        public int PuntosLocal { get; set; }

        public int PuntosVisitante { get; set; }

        // public int Liga { get; set; }

        public int Jornada1 { get; set; }
        public CompeticionDTO Competicion { get; set; }
        public TemporadaDTO Temporada { get; set; }

        public FaseCompeticionDTO Fase { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public Jornada ToModel()
        {

            Jornada jornada = new Jornada();
            jornada.Id = this.Id;
            jornada.Local = this.Local.IdEquipo;
            jornada.Estado = this.Estado;
            jornada.Visitante = this.Visitante.IdEquipo;
            jornada.Ubicacion = this.Ubicacion;
            jornada.Fecha = this.Fecha;
            jornada.PuntosLocal = this.PuntosLocal;
            jornada.PuntosVisitante = this.PuntosVisitante;
            jornada.Jornada1 = this.Jornada1;
            jornada.Temporada = this.Temporada.Id;
            jornada.Fase = this.Fase.Id;
            jornada.FechaCreacion = this.FechaCreacion;
            jornada.FechaModificacion = this.FechaModificacion;
            jornada.Competicion = this.Competicion.Id;
            return jornada;
            /*return new Jornada
            {
                Id = this.Id,
                Local = this.Local.IdEquipo,
                Visitante = this.Visitante.IdEquipo,
                Ubicacion = this.Ubicacion,
                Fecha = this.Fecha,
                PuntosLocal = this.PuntosLocal,
                PuntosVisitante = this.PuntosVisitante,
                Jornada1 = this.Jornada1,
                Temporada = this.Temporada.Id,
                Fase = this.Fase.Id,
                FechaCreacion = this.FechaCreacion,
                FechaModificacion = this.FechaModificacion
            };*/
        }

        public static JornadaDTO ToDTO(Jornada modelo)
        {
            return new JornadaDTO
            {
                Id = modelo.Id,
                //Local = modelo.Local,
                //Visitante = modelo.Visitante,
                Ubicacion = modelo.Ubicacion,
                Fecha = modelo.Fecha,
                PuntosLocal = modelo.PuntosLocal,
                PuntosVisitante = modelo.PuntosVisitante,
                Jornada1 = modelo.Jornada1,
                //Temporada = modelo.Temporada?.ToDTO(),
                //Fase = modelo.Fase?.ToDTO(),
                FechaCreacion = modelo.FechaCreacion,
                FechaModificacion = modelo.FechaModificacion,
                Estado = modelo.Estado
            };

        }





    }
    
}
