using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class TemporadaDTOSmall
    {
        public int Id { get; set; }

        public string Nombre { get; set; }


        public bool? Activo { get; set; }

        public string? source { get; set; }
        public bool? Visible { get; set; }

        public static TemporadaDTOSmall ToDto(Temporada modelo)
        {

            TemporadaDTOSmall temporada = new TemporadaDTOSmall();

            temporada.Id = modelo.Id;
            temporada.Nombre = modelo.Nombre;
            temporada.Activo = modelo.Activo;
            temporada.source = modelo.Source;
            temporada.Visible = modelo.Visible;

            return temporada;
        }


    }

    public class TemporadaDTO: TemporadaDTOSmall
    {
      

       

        public DateOnly Inicio { get; set; }

        public DateOnly Fin { get; set; }

       

        public DateTime? FechaModificacion { get; set; }

        public DateTime? FechaRegistro { get; set; }
       


        public static TemporadaDTO ToDto(Temporada modelo)
        {

            TemporadaDTO temporada = new TemporadaDTO();
            temporada.Id=modelo.Id;
            temporada.Nombre=modelo.Nombre;
            temporada.source = modelo.Source;
            temporada.Activo=modelo.Activo;
            temporada.FechaRegistro = modelo.FechaRegistro;
            temporada.FechaModificacion= modelo.FechaModificacion;
            temporada.Inicio = modelo.Inicio;
            temporada.Fin = modelo.Fin;
            temporada.Visible=modelo.Visible;
            
            

            return temporada;
        }

        public static TemporadaDTOSmall ToSmallDTO(TemporadaDTO modelo)
        {

            TemporadaDTOSmall temporada = new TemporadaDTOSmall();

            temporada.Id = modelo.Id;
            temporada.Nombre = modelo.Nombre;
            temporada.Activo = modelo.Activo;
            //temporada.source = modelo.Source;
            temporada.Visible = modelo.Visible;

            return temporada;
        }

        public Temporada ToModelo()
        {

            Temporada modelo = new Temporada();
            modelo.Id = this.Id;
            modelo.Nombre = this.Nombre;
            modelo.Source = this.source;
            modelo.Activo = this.Activo;
            modelo.FechaRegistro = this.FechaRegistro;
            modelo.FechaModificacion = this.FechaModificacion;
            modelo.Inicio = this.Inicio;
            modelo.Fin = this.Fin;
            modelo.Visible = this.Visible;

            return modelo;


        }
    }
}
