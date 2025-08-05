using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class ContratoEmpresaDTO
    {

        
        public int Id { get; set; }

        public string Tipo { get; set; } = null!;

        public int? Contribucion { get; set; }

        public string Condiciones { get; set; } = null!;

        public int? Noticia { get; set; } 

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public EmpresaDTOSmall Empresa { get; set; }

        public TemporadaDTO Temporada { get; set; }

        public string? Url { get; set; }


        public static ContratoEmpresaDTO ToDto(ContratoEmpresa empresa){
        
            ContratoEmpresaDTO contratoEmpresaDTO = new ContratoEmpresaDTO();   

            contratoEmpresaDTO.Id = empresa.Id;
            contratoEmpresaDTO.Noticia = empresa.Noticia;   
            contratoEmpresaDTO.FechaModificacion = empresa.FechaModificacion;
            contratoEmpresaDTO.Empresa = new EmpresaDTOSmall();
            contratoEmpresaDTO.Empresa.Id = empresa.Empresa;
            contratoEmpresaDTO.FechaCreacion = empresa.FechaCreacion;
            contratoEmpresaDTO.Temporada = new TemporadaDTO();
            contratoEmpresaDTO.Temporada.Id = empresa.Temporada;
            contratoEmpresaDTO.Condiciones = empresa.Condiciones;
            contratoEmpresaDTO.Contribucion = empresa.Contribucion;
            contratoEmpresaDTO.Tipo = empresa.Tipo;
            contratoEmpresaDTO.Url = empresa.Url;   

            return contratoEmpresaDTO;
        }

        public ContratoEmpresa ToModelo ()
        {

            ContratoEmpresa empresa= new ContratoEmpresa();

             empresa.Id = this.Id;
            empresa.Url = this.Url; 
            empresa.Noticia = this.Noticia;
            empresa.FechaCreacion = this.FechaCreacion;
            empresa.Temporada = this.Temporada.Id;
            empresa.Empresa = this.Empresa.Id;
            empresa.FechaModificacion = this.FechaModificacion;
            empresa.Tipo = this.Tipo;
            empresa.Condiciones = this.Condiciones;
            empresa.Contribucion = this.Contribucion;
            return empresa;

            
        }
    }

    public class ContratoEmpresaDTOSmall 
    {
       

        public string Tipo { get; set; } = null!;

       

       
        

        public int? Noticia { get; set; }

        

       public string LogoEmpresa { get; set; }

        public int? Empresa { get; set; }

        public int Temporada { get; set; }
    }
}
