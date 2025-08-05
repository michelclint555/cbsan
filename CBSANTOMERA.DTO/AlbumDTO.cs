
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
   public class AlbumDTOSmall  
    {

        public int IdAlbum { get; set; }

        public DateTime Fecha { get; set; }

        public string Nombre { get; set; } = null!;

        public string Portada { get; set; } 

        public string Descripcion { get; set; } = null!;
        
        public int Numero_Fotos { get; set; }
        public TemporadaDTOSmall Temporada { get; set; }
        public bool? EsActivo { get; set; }
        public bool? Fijar { get; set; }


        public Albume ToModelo()
        {

            Albume album = new Albume();
            album.IdAlbum = this.IdAlbum;
            album.Nombre = this.Nombre;
            album.Portada = this.Portada;
            album.Fecha = this.Fecha;
            album.Descripcion = this.Descripcion;
            album.Fijar = this.Fijar;
            album.EsActivo = this.EsActivo;


            return album;


        }

        public AlbumDTO ToAlbumDTO()
        {

            AlbumDTO album = new AlbumDTO();
            album.IdAlbum = this.IdAlbum;
            album.Nombre = this.Nombre;
            album.Portada = this.Portada;
            album.Fecha = this.Fecha;
            album.Descripcion = this.Descripcion;
            album.Fijar = this.Fijar;
            album.EsActivo = this.EsActivo;


            return album;


        }

        public static AlbumDTOSmall ToDto(Albume modelo)
        {
            AlbumDTOSmall album = new AlbumDTOSmall();

            album.IdAlbum = modelo.IdAlbum;
            album.Nombre = modelo.Nombre;
            album.Portada = modelo.Portada;
            album.Fecha = modelo.Fecha;
            album.Descripcion = modelo.Descripcion;
            album.Fijar = modelo.Fijar;
            album.EsActivo = modelo.EsActivo;
            return album;
        }





    }

    public  class AlbumDTO: AlbumDTOSmall
    {

        public int IdAlbum { get; set; }

        public DateTime Fecha { get; set; }

        public string Nombre { get; set; } = null!;

        public string Portada { get; set; }

        public string Descripcion { get; set; } = null!;
        public  IFormFile? imagen { get; set; } 
        public List<IFormFile>? fotos { get; set; } = null;
        public bool? EsActivo { get; set; }
        public bool? Fijar { get; set; }

        public virtual List<FotoAlbumDTO> FotosAlbum { get; set; } = new List<FotoAlbumDTO>();


        public static AlbumDTO ToDto(Albume modelo)
        {
            AlbumDTO album = new AlbumDTO();
            
            album.IdAlbum = modelo.IdAlbum;
            album.Nombre = modelo.Nombre;
            album.Portada = modelo.Portada;
            album.Fecha= modelo.Fecha;
            album.Descripcion= modelo.Descripcion;
            album.Numero_Fotos= album.FotosAlbum.Count;
            album.Fijar= modelo.Fijar;
            album.EsActivo= modelo.EsActivo;

            



            return album;
        }

        public Albume ToModelo()
        {

            Albume album = new Albume();
            album.IdAlbum = this.IdAlbum;
            album.Nombre = this.Nombre;
            album.Portada = this.Portada;
            album.Fecha = this.Fecha;
            album.Descripcion = this.Descripcion;
            
            album.Fijar = this.Fijar;
            album.EsActivo = this.EsActivo;
            album.Temporada = this.Temporada.Id;

            return album;


        }

        public AlbumDTOSmall ToAlbumSmallDTO()
        {

            AlbumDTOSmall album = new AlbumDTOSmall();
            album.IdAlbum = this.IdAlbum;
            album.Nombre = this.Nombre;
            album.Portada = this.Portada;
            album.Fecha = this.Fecha;
            album.Descripcion = this.Descripcion;

            album.Fijar = this.Fijar;
            album.EsActivo = this.EsActivo;
            album.Temporada.Id = this.Temporada.Id;

            return album;


        }

        public string ObtenerRuta()
        {
            string carpetaAlbum = this.IdAlbum + "\\";
            return Path.Combine("", this.Temporada.Nombre, "Albumes", carpetaAlbum);




        }


    }



}
