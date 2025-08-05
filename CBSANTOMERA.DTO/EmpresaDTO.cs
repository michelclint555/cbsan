using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{

    public class EmpresaDTOSmall
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public string? Logo { get; set; }

        public string? Url { get; set; }


        public static EmpresaDTOSmall ToDtoSmall(Empresa empresa)
        { 
                EmpresaDTOSmall empresa0 = new EmpresaDTOSmall();

                empresa0.Id = empresa.Id;
                empresa0.Nombre = empresa.Nombre;
                empresa0.Url = empresa.Url;
                empresa0.Logo = empresa.Logo;
                empresa0.Tipo = empresa.Tipo;
                

            return empresa0;
        
        }
       



        public Empresa ToModelo()
        { 
            Empresa empresa = new Empresa();
            return empresa;
        }


        }
    public class EmpresaDTO: EmpresaDTOSmall
    {
        public int Id { get; set; }

       

       

       

        public string? Url { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public bool? EsActivo { get; set; }

        public DateTime? FechaModificacion { get; set; }


        public required IFormFile? imagen { get; set; } = null;


    }
}
