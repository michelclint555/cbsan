using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
   public interface IFotoAlbumService
    {
        Task<List<FotoAlbumDTO>> Lista();
        Task<List<FotoAlbumDTO>> VerAlbum(int Idalbum);
        Task<AlbumDTO> CrearFoto(AlbumDTO album, List<IFormFile> fotosAlbum);
        Task<bool> Editar(FotoJugadorEquipoDTO modelo);
        Task<bool> Eliminar(int id);
        Task<bool> EliminarFotosAlbum(AlbumDTO album);
    }
}
