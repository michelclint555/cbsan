using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IEmpresaService
    {
        Task<List<EmpresaDTO>> ListaCompleta();
        Task<List<EmpresaDTOSmall>> Lista();
        Task<EmpresaDTO> Crear(EmpresaDTO modelo);
        Task<EmpresaDTO> Ver(int id);
        Task<EmpresaDTOSmall> VerSmall(int id);
        Task<bool> Editar(EmpresaDTO modelo);
        //void Dimension(AlbumDTO album);
        Task<bool> Eliminar(int id);



        }
}
