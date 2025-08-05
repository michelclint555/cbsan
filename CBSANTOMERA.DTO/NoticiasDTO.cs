using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{


    public class NoticiasDTOSmall
    {

        public int IdNoticia { get; set; }


        public string Titulo { get; set; } = null!;
        public string? Subtitulo { get; set; }
        public string Contenido { get; set; } = null!;

        public string Portada { get; set; } = null!;

        public string TipoNoticia { get; set; } = null!;

      

        public DateTime Fecha { get; set; }

        public string ThumbnailImageSrc { get; set; } = null!;
 
        public bool? Nuevo { get; set; }

        public Noticia ToModelo()
        {

            Noticia modelo = new Noticia();
            modelo.IdNoticia = this.IdNoticia;
            modelo.Titulo= this.Titulo;
            modelo.Subtitulo = this.Subtitulo;
            modelo.Fecha = this.Fecha;
            modelo.Contenido = this.Contenido;
            modelo.Portada = this.Portada;
            modelo.Nuevo = this.Nuevo;
            modelo.ThumbnailImageSrc = this.ThumbnailImageSrc;
            modelo.TipoNoticia = this.TipoNoticia;



            return modelo;


        }

        public static NoticiasDTOSmall ToDto(Noticia objecto)
        {
            NoticiasDTOSmall modelo = new NoticiasDTOSmall();

            
            modelo.IdNoticia = objecto.IdNoticia;
            modelo.Titulo = objecto.Titulo;
            modelo.Subtitulo = objecto.Subtitulo;
            modelo.Fecha = objecto.Fecha;
            modelo.Contenido = objecto.Contenido;
            modelo.Portada = objecto.Portada;
            modelo.Nuevo = objecto.Nuevo;
            modelo.ThumbnailImageSrc = objecto.ThumbnailImageSrc;
            modelo.TipoNoticia = objecto.TipoNoticia;

            return modelo;
        }

    }
    public class NoticiasDTO: NoticiasDTOSmall
    {
        
        public int IdNoticia { get; set; }
       

        public string Titulo { get; set; } = null!;
        public string? Subtitulo { get; set; }
        public string Contenido { get; set; } = null!;

        public string Portada { get; set; } = null!;

        public string TipoNoticia { get; set; } = null!;

        public AlbumDTO? Album { get; set; }

        public DateTime Fecha { get; set; }

        public string ThumbnailImageSrc { get; set; } = null!;
        public  IFormFile? imagen { get; set; } = null;


        public DateTime? FechaModificacion { get; set; }

        public bool? EsActivo { get; set; }

        public bool? Fijar { get; set; }

        public bool? Nuevo { get; set; }

        public Noticia ToModelo()
        {

            Noticia modelo = new Noticia();
            modelo.IdNoticia = this.IdNoticia;
            modelo.Titulo = this.Titulo;
            modelo.Subtitulo = this.Subtitulo;
            modelo.Fecha = this.Fecha;
            modelo.Contenido = this.Contenido;
            modelo.Portada = this.Portada;
            //modelo.Album = this.Album.IdAlbum;
            modelo.Nuevo = this.Nuevo;
            modelo.ThumbnailImageSrc = this.ThumbnailImageSrc;
            modelo.TipoNoticia = this.TipoNoticia;
            modelo.Fijar = this.Fijar;
            modelo.EsActivo = this.EsActivo;
            modelo.FechaModificacion = this.FechaModificacion;
            



            return modelo;


        }

        public static NoticiasDTO ToDto(Noticia objecto)
        {
            NoticiasDTO modelo = new NoticiasDTO();


            modelo.IdNoticia = objecto.IdNoticia;
            modelo.Titulo = objecto.Titulo;
            modelo.Subtitulo = objecto.Subtitulo;
            modelo.Fecha = objecto.Fecha;
            modelo.Contenido = objecto.Contenido;
            modelo.Portada = objecto.Portada;
            modelo.Nuevo = objecto.Nuevo;
            modelo.ThumbnailImageSrc = objecto.ThumbnailImageSrc;
            modelo.TipoNoticia = objecto.TipoNoticia;
            modelo.EsActivo = objecto.EsActivo;
            modelo.Fijar = objecto.Fijar;
            modelo.FechaModificacion = objecto.FechaModificacion;
            return modelo;
        }

    }


}
