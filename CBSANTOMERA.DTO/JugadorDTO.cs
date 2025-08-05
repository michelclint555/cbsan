using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
   public class JugadorDTO: JugadorSmallDTO
    {
        public int IdJugador { get; set; }

       

        public DateOnly? FechaNac { get; set; }

        public string? Sexo { get; set; }

        public string? Dnie { get; set; }

        public string? NombreTutor { get; set; }

        public string? Telefono { get; set; }

        public string? Foto { get; set; }

        public string? TelefonoTutor { get; set; }

        public string? CorreoTutor { get; set; }

        public string? Direccion { get; set; }

        public string? Localidad { get; set; }

        public string? Provincia { get; set; }

        public string? Cp { get; set; }

        public int? Usuario { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool? EsActivo { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string? ThumbnailImageSrc { get; set; }

        public IFormFile? imagen { get; set; }
        public TemporadaDTO? temporada { get; set; }

        public static JugadorDTO ToDto(Jugador modelo)
        {
            return new JugadorDTO
            {
                IdJugador = modelo.IdJugador,
                FechaNac = modelo.FechaNac,
                Nombre = modelo.Nombre,
                Apellidos = modelo.Apellidos,
                Sexo = modelo.Sexo,
                Dnie = modelo.Dnie,
                Telefono = modelo.Telefono,
                NombreTutor = modelo.NombreTutor,
                TelefonoTutor = modelo.TelefonoTutor,
                CorreoTutor = modelo.CorreoTutor,
                EsActivo = modelo.EsActivo,
                FechaCreacion = modelo.FechaCreacion,
                FechaModificacion = DateTime.Now,
                Direccion = modelo.Direccion,
                Localidad = modelo.Localidad,
                Provincia = modelo.Provincia,
                Cp = modelo.Cp,
                Foto = modelo.Foto,
                
                ThumbnailImageSrc = modelo.ThumbnailImageSrc,
                //Temporada = modelo.Temporada,
               
                
            };
        }


        public static Jugador ToModel(JugadorDTO dto)
        {
            return new Jugador
            {
                IdJugador = dto.IdJugador,
                FechaNac = dto.FechaNac,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Sexo = dto.Sexo,
                Dnie = dto.Dnie,
                Telefono = dto.Telefono,
                NombreTutor = dto.NombreTutor,
                CorreoTutor = dto.CorreoTutor,
                TelefonoTutor = dto.TelefonoTutor,
                ThumbnailImageSrc = dto.ThumbnailImageSrc,
                
                Direccion = dto.Direccion,
                Localidad = dto.Localidad,
                Provincia = dto.Provincia,
                Cp = dto.Cp,
                Foto = dto.Foto,
               
                 EsActivo = dto.EsActivo,
                FechaCreacion = dto.FechaCreacion,
              

            };
        }
       
    }


    public class JugadorSmallDTO  
    {
        
        public string Nombre { get; set; }

        public string Apellidos { get; set; }
        //public int? Temporada {  get; set; }

       


    }


}
