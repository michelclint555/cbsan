using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ICategoriaJugadorService
    {
        Task<List<CategoriaJugadorDTO>> Lista();
        Task<CategoriaJugadorDTO> Crear(CategoriaJugadorDTO modelo);
        Task<bool> Editar(CategoriaJugadorDTO modelo);
        Task<bool> Eliminar(int id);
        Task<CategoriaJugadorDTO> Obtener(int id);
    }
}
