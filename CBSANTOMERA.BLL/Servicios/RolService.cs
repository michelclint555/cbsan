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
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try {
                var listaRoles = await _rolRepository.Consultar();
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());   
            
            } catch {
                throw;
            
            }
        }


        public async Task<RolDTO> Ver(int id)
        {
            try
            {
                var rol = await _rolRepository.ObtenerUnModelo(r=> r.Id == id);
                return _mapper.Map<RolDTO>(rol);

            }
            catch
            {
                throw;

            }
        }
    }
}
