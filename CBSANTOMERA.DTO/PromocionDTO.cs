
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
   public class PromocionDTOSmall  
    {

        public int Id { get; set; }

        public string Enlace { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string Subtitulo { get; set; } = null!;

        public string TituloEnlace { get; set; } = null!;

        public string? Thumbnail { get; set; }

        public string Portada { get; set; } = null!;
        public int? Orden { get; set; }

        









    }

    public  class PromocionDTO: PromocionDTOSmall
    {

        public int Id { get; set; }

        public string Enlace { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string Subtitulo { get; set; } = null!;

        public string TituloEnlace { get; set; } = null!;

        public string? Thumbnail { get; set; }

        public string Portada { get; set; } = null!;

        public bool? Activo { get; set; }

        public bool? Fijar { get; set; }

        public int? Orden { get; set; }

        public int? Temporada { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public required IFormFile? imagen { get; set; } = null;

    }


}
