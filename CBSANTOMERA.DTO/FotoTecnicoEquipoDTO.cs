using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class FotoTecnicoEquipoDTO
    {

        public string? ThumbnailImageSrc { get; set; }

        public int Id { get; set; }

        public int IdTecnico { get; set; }
        public int Tecnico { get; set; }
        public int IdEquipo{ get; set; }

        public string? Url { get; set; }
        public IFormFile? imagen { get; set; }

        


    }
}
