using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.MODEL
{
    public partial class AccionFile
    {
        public string accion { get; set; } = null!; //verbo de la accion a realizar por el momento guardar, actualizar y eliminar
        public Boolean thumbnail { get; set; } = false; //Si se quiere thumbnail
        public string rutaOrigen { get; set; } = null!; //la ruta de la imagen origen, si la accion es actualizar o eliminar
        public string rutaDesten { get;set; } //es la ruta donde vamos a guardar la imagen si la accion es guardar o actualizar
        public string rutaOrigenThumbnail { get; set; } = null!; //el origen de la imagen del thubnail, si la accion es eliminar o actualizar
        public string rutaDestenThumbnail { get; set; } //el destino de la imagen del thubmnail si la accion es guardar con thumnnail o actualizar
        public int sizeThumbnail{ get; set; } //el tamaño del thumbnail del ancho y se adapta al largo
        public IFormFile file { get; set; } = null!; //el fichero a guardar


    }
}
