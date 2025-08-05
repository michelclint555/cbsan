
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.BLL.Servicios
{
    public class MenuService : IMenuService
    {

        public readonly IGenericRepository<Usuario> _genericRepository;
        public readonly IGenericRepository<MenuRol> _menuRolRepositorio;
        public readonly IGenericRepository<Menu> _menuRepositorio;
        public readonly IMapper _mapper;
        public async Task<List<MenuDTO>> Lista(int id)
        {
            IQueryable<Usuario> tbUsuario = await _genericRepository.Consultar(u => u.IdUsuario == id);
            IQueryable<MenuRol> tbMenuROl = await _menuRolRepositorio.Consultar();
            IQueryable<Menu> tbMenu = await _menuRepositorio.Consultar();

            try
            {
                IQueryable<Menu>tbresultado = 
                    (from u in tbUsuario join mr in tbMenuROl on u.IdRol equals mr.IdRol
                     join m in tbMenu on mr.IdMenu equals m.Id
                     select m).AsQueryable();
                    var listaMenus = tbresultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaMenus);
    

            }
            catch
            {
                throw;
            }
        }
    }
}
