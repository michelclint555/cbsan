using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSANTOMERA.DTO;

//using CBSANTOMERA.Model;
using Microsoft.AspNetCore.Http;

namespace CBSANTOMERA.DTO
{
    public class ClubDTO
    {
        public int IdClub { get; set; }

        public string? Foto { get; set; }

        public string? Nombre { get; set; }

        public string? Alias { get; set; }


        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        public bool? EsActivo { get; set; }

        public IFormFile? imagen { get; set; }
        //public virtual ICollection<EquipoDTO> ListaEquipos { get; set; } = new List<EquipoDTO>();
        public  List<EquipoDTO> ListaEquipos{ get; set; } = new List<EquipoDTO>();


    }
}
