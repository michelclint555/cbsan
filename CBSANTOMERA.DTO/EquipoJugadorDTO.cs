using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class EquipoJugadorDTO
    {
        public int Id { get; set; }
        public int Equipo { get; set; }
       // public virtual ICollection<string>? fotos { get; set; } = new List<string>();
        public  JugadorDTO Jugador{ get; set; }
        public virtual List<FotoJugadorEquipoDTO> fotos { get; set; } = new List<FotoJugadorEquipoDTO>();

       // public  IFormFile? imagen { get; set; } 
        public int Dorsal { get; set; }


        public EquipoJugadorDTO()
        {
            this.Id = 0;
            this.Equipo = 0;
            this.Jugador = new JugadorDTO();
            this.Dorsal = 0;
            this.fotos = new List<FotoJugadorEquipoDTO>();
           // this.imagen = null;
        }

    }


}
