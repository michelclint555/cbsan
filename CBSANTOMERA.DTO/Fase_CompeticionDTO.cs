using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class FaseCompeticionDTO
    {
        public int Id { get; set; }

        public CompeticionDTO Competicion { get; set; }

        public string Nombre { get; set; }

        public int? NumEquipos { get; set; }
        public string? Estado { get; set; }
        public int? NumPartidos { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }


        public  FasesCompeticion ToModel()
        {
            return new FasesCompeticion
            {
                Id = this.Id,
                Competicion = this.Competicion.Id,
                Estado = this.Estado,
                Nombre = this.Nombre,
                NumEquipos = this.NumEquipos,
                NumPartidos = this.NumPartidos,
                FechaCreacion = this.FechaCreacion,
                FechaModificacion = this.FechaModificacion,
            };
        }

        public static FaseCompeticionDTO ToDTO(FasesCompeticion modelo)
        {
            return new FaseCompeticionDTO
            {
                Id = modelo.Id,
                Estado=modelo.Estado,
                //Competicion = modelo.Competicion?.ToDTO(),
                Nombre = modelo.Nombre,
                NumEquipos = modelo.NumEquipos,
                NumPartidos = modelo.NumPartidos,
                FechaCreacion = modelo.FechaCreacion,
                FechaModificacion = modelo?.FechaModificacion,


            };
        }


       


    }

   


}
