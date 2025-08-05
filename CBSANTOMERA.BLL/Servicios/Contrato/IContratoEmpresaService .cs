using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface IContratoEmpresaService
    {
        Task<List<ContratoEmpresaDTO>> ListaCompleta();
        Task<List<EmpresaDTOSmall>> Lista();
        Task<ContratoEmpresaDTO> Crear(ContratoEmpresaDTO modelo);
        Task<ContratoEmpresaDTO> Ver(int id);
        Task<ContratoEmpresaDTOSmall> ContratoEmpresaSmall(int id);
        Task<bool> Editar(ContratoEmpresaDTO modelo);
        //void Dimension(AlbumDTO album);
        Task<bool> Eliminar(int id);
        Task<List<EmpresaDTOSmall>> Lista(int temporada);
        Task<List<ContratoEmpresaDTO>> ListaCompleta(int temporada);


        }
}
