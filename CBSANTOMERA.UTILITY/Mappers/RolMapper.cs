using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.UTILITY.Mappers
{
  
    public static class RolMapper
    {
        // Rol → RolDTO
        public static RolDTO ToDTO(Rol model)
        {
            if (model == null) return null;

            return new RolDTO
            {
                Id = model.Id,
                Nombre = model.Nombre
            };
        }

        // RolDTO → Rol
        public static Rol ToModel(RolDTO dto)
        {
            if (dto == null) return null;

            return new Rol
            {
                Id = dto.Id,
                Nombre = dto.Nombre
            };
        }

        // Lista de Roles → Lista de DTOs
        public static List<RolDTO> ToDTOList(IEnumerable<Rol> roles)
        {
            return roles?.Select(r => ToDTO(r)).ToList();
        }

        // Lista de DTOs → Lista de Roles
        public static List<Rol> ToModelList(IEnumerable<RolDTO> dtos)
        {
            return dtos?.Select(r => ToModel(r)).ToList();
        }
    }

}
