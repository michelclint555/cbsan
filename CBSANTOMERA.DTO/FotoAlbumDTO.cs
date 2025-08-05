
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class FotoAlbumDTO
    {
        public int IdFoto { get; set; }

        public string imagen { get; set; }

        public string? Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public int? IdAlbum { get; set; }

        public string ThumbnailImageSrc { get; set; }




        public static List<FotoAlbumDTO> ToDTO(List<FotosAlbum> fotos) {
            List<FotoAlbumDTO> lista = new List<FotoAlbumDTO>();

            foreach (var item in fotos)
            {
                FotoAlbumDTO foto = new FotoAlbumDTO();
                foto.Fecha = item.Fecha;
                foto.IdAlbum = item.IdAlbum;
                foto.IdFoto = item.IdFoto;
                lista.Add(foto);    


            }

            return lista;
        }
        public static implicit operator FotoAlbumDTO(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
