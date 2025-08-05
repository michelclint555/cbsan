using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IClubService
    {
        Task<List<ClubDTO>> Lista();
        Task<ClubDTO> Crear(ClubDTO modelo);
        Task<bool> Editar(ClubDTO modelo);
        Task<bool> Eliminar(int id);
        Task<ClubDTO> VerClub(int id);
    }
}
