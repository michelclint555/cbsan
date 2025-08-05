using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class TecnicoDTO
    {
        public int Id { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string? Nombre { get; set; }

        public string? Apellidos { get; set; }

        public DateOnly? FechaNac { get; set; }

        public string? Foto { get; set; }

        public string? ThumbnailImageSrc { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool? EsActivo { get; set; }

        public TemporadaDTO? Temporada { get; set; }
        public IFormFile? imagen { get; set; }

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public static TecnicoDTO ToDTO(Tecnico model)
        {
            return new TecnicoDTO
            {
                Id = model.Id,
                FechaCreacion = model.FechaCreacion,
                Nombre = model.Nombre,
                Apellidos = model.Apellidos,
                FechaNac = model.FechaNac,
                Foto = model.Foto,
                ThumbnailImageSrc = model.ThumbnailImageSrc,
                FechaModificacion = model.FechaModificacion,
                EsActivo = model.EsActivo,
                
                Telefono = model.Telefono,
                Correo = model.Correo
            };
        }

        public static Tecnico ToModel(TecnicoDTO dto)
        {
            return new Tecnico
            {
                Id = dto.Id,
                FechaCreacion = dto.FechaCreacion ?? DateTime.UtcNow,
                Nombre = dto.Nombre ?? string.Empty,
                Apellidos = dto.Apellidos ?? string.Empty,
                FechaNac = dto.FechaNac ?? DateOnly.MinValue,
                Foto = dto.Foto ?? string.Empty,
                ThumbnailImageSrc = dto.ThumbnailImageSrc ?? string.Empty,
                FechaModificacion = dto.FechaModificacion ?? DateTime.UtcNow,
                EsActivo = dto.EsActivo ?? true,
                Temporada = dto.Temporada.Id,
                Telefono = dto.Telefono ?? string.Empty,
                Correo = dto.Correo ?? string.Empty
            };
        }

        

    }

  
}
