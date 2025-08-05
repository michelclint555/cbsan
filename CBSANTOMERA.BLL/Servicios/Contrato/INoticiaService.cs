using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface INoticiaService
    {
        Task<List<NoticiasDTO>> Lista();
        Task<NoticiasDTO> VerNoticia(int id);
        Task<NoticiasDTO> Crear(NoticiasDTO noticia);
        Task<bool> Editar(NoticiasDTO modelo);
        Task<bool> Eliminar(int id);
        Task<bool> EliminarAlbum(NoticiasDTO noticia);
        Task<TipoNoticium> VerTipoNoticia(int id);
        Task<List<TipoNoticium>> ListaTipo();
        Task<List<NoticiasDTOSmall>> ListaInicio();
       Task<List<NoticiasDTO>> Lista_Noticias_Empresas();
        Task<List<NoticiasDTO>> Lista_Noticias_Activas();
        Task<NoticiasDTO> VerNoticiaEmpresa(string nombre);
    }
}
