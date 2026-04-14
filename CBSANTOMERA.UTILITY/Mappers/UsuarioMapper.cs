using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.UTILITY.Mappers
{
    public static class UsuarioMapper
    {
        // UsuarioDTO → Usuario
        public static Usuario ToModel(UsuarioDTO dto)
        {
            if (dto == null) return null;

            return new Usuario
            {
                IdUsuario = dto.IdUsuario,
                IdRol = dto.Rol?.Id ?? 0,
                Correo = dto.Correo,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Clave = dto.Clave,
                EsActivo = dto.EsActivo,
                ContrasenaActivada = dto.ContrasenaActivada,
                FechaRegistro = dto.FechaRegistro,
                FechaModificacion = dto.FechaModificacion,
                Foto = dto.Foto,
                Token = dto.Token
            };
        }

        // Usuario → UsuarioDTO
        public static UsuarioDTO ToDTO(Usuario model, Rol rol = null)
        {
            if (model == null) return null;

            return new UsuarioDTO
            {
                IdUsuario = model.IdUsuario,
                Rol = rol != null ? RolMapper.ToDTO(rol) : new RolDTO { Id = model.IdRol },
                Correo = model.Correo,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                Clave = model.Clave,
                EsActivo = model.EsActivo,
                ContrasenaActivada = model.ContrasenaActivada,
                FechaRegistro = model.FechaRegistro,
                FechaModificacion = model.FechaModificacion,
                Foto = model.Foto,
                Token = model.Token
            };
        }

        // Lista de Usuario → Lista de UsuarioDTO
        public static List<UsuarioDTO> ToDTOList(IEnumerable<Usuario> usuarios, IEnumerable<Rol> roles = null)
        {
            return usuarios.Select(u =>
            {
                Rol userRol = roles?.FirstOrDefault(r => r.Id == u.IdRol);
                return ToDTO(u, userRol);
            }).ToList();
        }
    }
}



