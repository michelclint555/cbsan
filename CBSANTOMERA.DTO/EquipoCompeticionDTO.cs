using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class EquipoCompeticionDTO
    {
        public EquipoDTO Equipo { get; set; }

        public int Competicion { get; set; }
        public Boolean Incluido{ get; set; }//Si se encuentra en la competicion
    }
}
