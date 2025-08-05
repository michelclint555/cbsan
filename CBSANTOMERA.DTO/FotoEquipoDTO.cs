//using CBSANTOMERA.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class FotoEquipoDTO
    {
        public int Id { get; set; }

        public int IdEquipo { get; set; }

        public string? Url { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string? Foto { get; set; }
        public string ThumbnailImageSrc { get; set; }
        

        
    }
}
