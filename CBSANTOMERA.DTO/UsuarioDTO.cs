using System;
using System.Collections.Generic;
namespace CBSANTOMERA.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public RolDTO Rol { get; set; }

        public string? Correo { get; set; }

        public string? Nombre { get; set; }

        public string? Apellidos { get; set; }
    
        public string? Token { get; set; }

        public string? Clave { get; set; }
        public bool? ContrasenaActivada { get; set; }
        public bool? EsActivo { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile? imagen { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? Foto { get; set; }



    }
}
