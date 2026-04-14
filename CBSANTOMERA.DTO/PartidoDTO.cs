using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class PartidoDTO
    {
       
            public int Id { get; set; }

            public DateTime Fecha { get; set; }

            public string Ubicacion { get; set; } = null!;

            public string Estado { get; set; } = null!;

            public int PuntosLocal { get; set; }
            public int PuntosVisitante { get; set; }

            public EquipoDTO Local { get; set; } = null!;
            public EquipoDTO Visitante { get; set; } = null!;

            public CompeticionDTO Competicion { get; set; } = null!;
            public FaseCompeticionDTO? Fase { get; set; }
            public JornadaDTOSimple Jornada { get; set; } = null!;
            public TemporadaDTO? Temporada { get; set; }

            public DateTime? FechaCreacion { get; set; }
            public DateTime? FechaModificacion { get; set; }

        public Partido ToModel()
        {
            return new Partido
            {
                Id = this.Id,
                Fecha = this.Fecha,
                Ubicacion = this.Ubicacion,
                Estado = this.Estado,
                PuntosLocal = this.PuntosLocal,
                PuntosVisitante = this.PuntosVisitante,

                Local = this.Local.IdEquipo,
                Visitante = this.Visitante.IdEquipo,

               
             
                Jornada = this.Jornada.Id,
               

                FechaCreacion = this.FechaCreacion,
                FechaModificacion = this.FechaModificacion
            };
        }

        public static PartidoDTO ToDTO(Partido modelo)
        {
            return new PartidoDTO
            {
                Id = modelo.Id,
                Fecha = modelo.Fecha,
                Ubicacion = modelo.Ubicacion,
                Estado = modelo.Estado,
                PuntosLocal = modelo.PuntosLocal,
                PuntosVisitante = modelo.PuntosVisitante,

              //  Local = EquipoDTO.EquipoToDTO(modelo.Local),
               // Visitante = EquipoDTO.EquipoToDTO(modelo.VisitanteNavigation),

              
               
                   

                Jornada = new JornadaDTOSimple
                {
                    Id = modelo.Jornada ?? 0,
                   // Numero = modelo.JornadaNavigation?.Numero ?? 0,
                    //Estado = modelo.JornadaNavigation?.Estado
                },

                FechaCreacion = modelo.FechaCreacion,
                FechaModificacion = modelo.FechaModificacion
            };
        }
    }
}

    //}
    public class PartidoDTOSimple
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; } = null!;

        public string Ubicacion { get; set; } = null!;

       // public EquipoDTOSimple Local { get; set; } = null!;
       // public EquipoDTOSimple Visitante { get; set; } = null!;

        public int PuntosLocal { get; set; }
        public int PuntosVisitante { get; set; }
    }



//}


