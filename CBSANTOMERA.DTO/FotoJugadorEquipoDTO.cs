using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class FotoJugadorEquipoDTO
    {
        public int Id { get; set; }

        public string? Foto { get; set; } = " ";

        public string? UrlFoto { get; set; }
        public IFormFile? imagen { get; set; }
        public bool Principal { get; set; }

        public bool Secundaria { get; set; }
        public int  IdEquipo { get; set; }
        public int? EquipoJugador { get; set; }
        public int Jugador { get; set; }
        public string? ThumbnailImageSrc { get; set; }

    }
}
