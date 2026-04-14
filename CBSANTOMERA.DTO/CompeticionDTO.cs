using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class CompeticionDTO
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int? IdTipo { get; set; }
        public string Tipo { get; set; }

        public int? NumEquipos { get; set; }

        public string? Logo { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public TemporadaDTO? Temporada { get; set; }

        public bool? EsActivo { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaModificacion { get; set; }
        public int PartidosPlayoff { get; set; }


        public bool? TienePlayoff { get; set; }
        public int? Categoria { get; set; }

        public string? Estado { get; set; }
        public IFormFile? imagen { get; set; }
        public IFormFile? calendario { get; set; }





        public  static CompeticionDTO ToDTO(Competicione modelo)
        {
            return new CompeticionDTO
            {
                Id = modelo.Id,
                Nombre = modelo.Nombre,
             //  IdTipo = modelo.Idtipo,
                NumEquipos = modelo.NumEquipos,
                Logo = modelo.Logo,
                FechaCreacion = modelo.FechaCreacion,
                //Temporada = modelo.Temporada?.ToDTO(),
                EsActivo = modelo.EsActivo,
                FechaRegistro = modelo.FechaRegistro,
                FechaModificacion = modelo.FechaModificacion,
                Categoria = modelo.Categoria,
                Estado = modelo.Estado,
                PartidosPlayoff = modelo.PartidosPlayoff ?? 0,
                TienePlayoff = modelo.TienePlayoff
                
    };
        }

        public  Competicione ToModel( )
        {
            return new Competicione
            {
                Id = this.Id,
                Nombre = this.Nombre,
               // Idtipo = this.IdTipo,
               // Tipo = this.Tipo,
                NumEquipos = this.NumEquipos,
                Logo = this.Logo,
                FechaCreacion = this.FechaCreacion,
                Temporada = this.Temporada.Id,
                EsActivo = this.EsActivo,
                FechaRegistro = this.FechaRegistro,
                FechaModificacion = this.FechaModificacion,
                Categoria = this.Categoria,
                Estado = this.Estado,
                PartidosPlayoff = this.PartidosPlayoff,
                TienePlayoff = this.TienePlayoff
                
            };
        }


    }

   


}
