//using Aspose.Imaging;
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
    public interface IArchivosService
    {
        bool ActualizarImagen(string rutaOrigen, string rutaDestino, IFormFile file);

        bool ActualizarImagen(string rutaOrigen, string rutaDestino, IFormFile file, string rutaOrigenThumbnail,  string rutaDestinoThumbnail, int ancho);
        bool ActualizarArchivo(string rutaOrigen,string rutaDestino, IFormFile file);

        bool EliminarImagen(string rutaOrigen);
        bool EliminarImagen(string rutaOrigen, string rutaOrigenThumbnail);

        bool Ejecutar(AccionFile accion);
        bool guardarImagen(string rutaDestino, IFormFile file);
        bool guardarImagen(string rutaDestino, IFormFile file, int ancho, string filenameThumbnail);


        bool guardarImagen(string rutaDestino, IFormFile file, string filenameThumbnail);
        bool ActualizarArchivo(string ruta, string filename_thumbnail);
        bool RedimensionarImagenCuadrado(FileStream file,string rutafilename, string ruta, string filename, int tamano);
        bool RedimensionarImagenProporcional(FileStream file , string rutafilename, string ruta, string filename, int X, int Y);
        void RedimensionarImagen120(string imageFileName, string thumbnailFileName, string extension);
        bool EliminarArchivo(string ruta);
        bool guardarArchivo(IFormFile file, string ruta, string filename, string tipoObjeto, int id, string extension, bool thumbnail, bool completeFile);
    }
}
