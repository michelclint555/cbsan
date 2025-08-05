using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
   public interface IFotoEquipoService
    {
        Task<List<FotoEquipoDTO>> Lista();
        Task<List<FotoEquipoDTO>> VerFotosEquipo(int Idequipo);
        //Task<EquipoDTO> Crear(EquipoDTO equipo, List<IFormFile> fotosAlbum);
        Task<bool> Editar(EquipoDTO modelo);
        Task<bool> Eliminar(int id);
        Task<bool> EliminarFotosEquipo(EquipoDTO equipo);
    }
}
