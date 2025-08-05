using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class TecnicoEquipoDTO
    {

        public int Id { get; set; }

        public int IdEquipo { get; set; }

        public TecnicoDTO tecnico { get; set; }

        public string Funcion { get; set; }
        public virtual List<FotoTecnicoEquipoDTO> listaFotos { get; set; } = new List<FotoTecnicoEquipoDTO>();
    }
}
