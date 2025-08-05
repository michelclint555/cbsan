using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IAlbumService
    {
        Task<List<AlbumDTO>> Lista();
        Task<List<AlbumDTOSmall>> ListaSmall();
        Task<List<AlbumDTO>> Lista_Full(string nombre);
        Task<List<AlbumDTOSmall>> Lista(string nombre);
        Task<AlbumDTO> Crear(AlbumDTO modelo);
        Task<AlbumDTO> VerAlbum(int id);
        Task<AlbumDTOSmall> VerAlbumSmall(int id);
        Task<bool> Editar(AlbumDTO modelo);
        void Dimension(AlbumDTO album);
        Task<bool> EliminarAlbum(int id);

        Task<List<AlbumDTOSmall>> ListaSmallFijo();

        }
}
