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
        public int Id { get; set; }
        public int Numero { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class JornadaDTO
    {
        public int Id { get; set; }

        public int Numero { get; set; }

        public string? Estado { get; set; }

        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public CompeticionDTO? Competicion { get; set; }
        public FaseCompeticionDTO? Fase { get; set; }

        public List<PartidoDTO>? Partidos { get; set; }

        public Jornada ToModel()
        {
            return new Jornada
            {
                Id = this.Id,
                Numero = this.Numero,
                Estado = this.Estado,
                FechaInicio = this.FechaInicio,
                FechaFin = this.FechaFin,
                FechaCreacion = this.FechaCreacion,
                FechaModificacion = this.FechaModificacion,
                Competicion = this.Competicion?.Id,
                Fase = this.Fase?.Id
            };
        }

        public static JornadaDTO ToDTO(Jornada modelo)
        {
            return new JornadaDTO
            {
               // Id = modelo.Id,
                Numero = modelo.Numero ?? 0,
                Estado = modelo.Estado,
                FechaInicio = modelo.FechaInicio,
                FechaFin = modelo.FechaFin,
                FechaCreacion = modelo.FechaCreacion,
                FechaModificacion = modelo.FechaModificacion,

              //  Competicion = modelo.CompeticionNavigation != null
                 //  ? CompeticionDTO.ToDTO(modelo.CompeticionNavigation)
                  //  : null,

               // Fase = modelo.FaseNavigation != null
                  //  ? FaseCompeticionDTO.ToDTO(modelo.FaseNavigation)
                 //   : null,

               // Partidos = modelo.Partidos != null
                //    ? modelo.Partidos.Select(PartidoDTO.ToDTO).ToList()
                 //   : new List<PartidoDTO>()
            };
        }
    }
}
