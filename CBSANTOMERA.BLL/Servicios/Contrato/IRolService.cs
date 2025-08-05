using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> Lista();
        Task<RolDTO> Ver(int id);
    }
}
