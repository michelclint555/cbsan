using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class CategoriaJugadorDTOSmall
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public bool? EsActivo { get; set; }

        public string? Sexo { get; set; }

        public int? PrimerAnio { get; set; }

        public int? SegundoAnio { get; set; }

     
    }


    public class CategoriaJugadorDTO : CategoriaJugadorDTOSmall
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public bool? EsActivo { get; set; }

        public string? Sexo { get; set; }

        public int? PrimerAnio { get; set; }

        public int? SegundoAnio { get; set; }

        public static implicit operator CategoriaJugadorDTO(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
