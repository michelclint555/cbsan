using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ITecnicoService
    {
        Task<List<TecnicoDTO>> Lista();
        Task<TecnicoDTO> Crear(TecnicoDTO modelo);
        Task<bool> Editar(TecnicoDTO modelo);
        Task<bool> Eliminar(int id);
        Task<TecnicoDTO> BuscarTecnico(int id);
      
       
        

    }
}
