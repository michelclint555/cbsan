//using CBSANTOMERA.Model;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class EquipoRivalDTO
    {
        public int IdEquipo { get; set; }

        
        public string Nombre { get; set; }

        public int IdClub { get; set; }
        public int idCategoria { get; set; }
        public int? Temporada { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public Equipo ToModel()
        {
            return new Equipo
            {
                IdEquipo = this.IdEquipo,
                //Principal = this.Principal,
                Nombre = this.Nombre,
                IdClub = this.IdClub,
                IdCategoria = this.idCategoria,
                //Foto = this.Foto,
                EsActivo = this.EsActivo,
                FechaRegistro = this.FechaRegistro ?? DateTime.UtcNow,
                FechaModificacion = this.FechaModificacion,
                Temporada = this.Temporada,



            };
        }

        public static EquipoRivalDTO EquipoToDTO(Equipo equipo)
        {
            return new EquipoRivalDTO
            {
                IdEquipo = equipo.IdEquipo,
                //Principal = (bool)equipo.Principal,
                Nombre = equipo.Nombre,
                IdClub = equipo.IdClub,
                idCategoria = equipo.IdCategoria,


                //Foto = equipo.Foto,
                EsActivo = equipo.EsActivo,
                FechaRegistro = equipo.FechaRegistro,
                FechaModificacion = equipo.FechaModificacion,

                // Puedes mapear listas si es necesario

            };
        }

    }

    public class EquipoDTOSimple
    {
        public string Nombre { get; set; }
        public string logo { get; set; }
        public int IdClub { get; set; }


    }

    public class patrocinadorEquipo
    {
       public int equipo { get; set; }
        public int patrocinador { get; set; }
        public int temporada { get; set; }
        //Comprueba que el club es el nuestro

    }
    public class EquipoDTO
    {
        public int IdEquipo { get; set; }

        public bool Principal { get; set; }
        public string Nombre { get; set; }
        public string? logo { get; set; }
        public ContratoEmpresaDTO? patrocinador { get; set; }
        public int IdClub { get; set; }
        public int idCategoria { get; set; }
        public CategoriaJugadorDTO? Categoria { get; set; }
        public TemporadaDTOSmall temporada { get; set; } 
        public IFormFile? imagen { get; set; }
        public string? Foto { get; set; }
        //public virtual Club? IdClubNavigation { get; set; }
        public List<EquipoJugadorDTO>? EquipoJugadores { get; set; } = new List<EquipoJugadorDTO>();

        public virtual List<FotoEquipoDTO>? FotoEquipos { get; set; } = new List<FotoEquipoDTO>();



        //public virtual CategoriaJugadorDTO? IdCategoriaNavigation { get; set; }



        public virtual List<TecnicoEquipoDTO>? TecnicoEquipos { get; set; } = new List<TecnicoEquipoDTO>();
        
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public static EquipoDTO EquipoToDTO(Equipo equipo)
        {
            return new EquipoDTO
            {
                IdEquipo = equipo.IdEquipo,
                Principal =(bool) equipo.Principal,
                Nombre = equipo.Nombre,
                IdClub = equipo.IdClub,
                idCategoria = equipo.IdCategoria,
               
               
                Foto = equipo.Foto,
                EsActivo = equipo.EsActivo,
                FechaRegistro = equipo.FechaRegistro,
                FechaModificacion = equipo.FechaModificacion,

                // Puedes mapear listas si es necesario
               
            };
        }

        public  Equipo ToModel()
        {
            return new Equipo
            {
                IdEquipo = this.IdEquipo,
                Principal = this.Principal,
                Nombre = this.Nombre,
                IdClub = this.IdClub,
                IdCategoria = this.idCategoria,
                Foto = this.Foto,
                EsActivo = this.EsActivo,
                FechaRegistro = this.FechaRegistro ?? DateTime.UtcNow,
                FechaModificacion = this.FechaModificacion,
                Temporada = this.temporada.Id,


                
            };
        }

    }






}