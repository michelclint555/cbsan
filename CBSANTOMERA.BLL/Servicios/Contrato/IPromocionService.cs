using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IPromocionService
    {
        Task<List<PromocionDTO>> ListaCompleta();
        Task<List<PromocionDTOSmall>> Lista();
        Task<PromocionDTO> Crear(PromocionDTO modelo);
        Task<PromocionDTO> Ver(int id);
        Task<bool> Editar(PromocionDTO modelo);
        //void Dimension(AlbumDTO album);
        Task<bool> Eliminar(int id);



        }
}
