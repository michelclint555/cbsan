using LazZiya.ImageResize;
using CBSANTOMERA.BLL.Servicios.Contrato;

using System.Drawing;
using System.Drawing.Imaging;
using System;
using Microsoft.AspNetCore.Http;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.BLL.Servicios
{
    public class ArchivosService : IArchivosService
    {


        private readonly string rutaServidor = @"wwwroot\archivos\";


        public bool guardarArchivo(IFormFile file, string ruta, string filename, string tipoObjeto, int id, string extension, bool thumbnail, bool completeFile) // 
        {
            string rutaObjeto = "";
            string rutaImagen = ruta;
            if (tipoObjeto != null)
            {
                if (!Directory.Exists(rutaServidor + tipoObjeto))
                {
                    Directory.CreateDirectory(rutaServidor + tipoObjeto);
                }
                rutaObjeto = rutaServidor + tipoObjeto;
            }


            if (id != null && tipoObjeto != null)
            {
                string carpetaID = id + "\\";


                if (!Directory.Exists(rutaObjeto + carpetaID))
                {
                    Directory.CreateDirectory(rutaObjeto + carpetaID);
                }
                rutaImagen = Path.Combine(rutaObjeto, carpetaID);
            }





            if (extension == null)
            {
                extension = Path.GetExtension(file.FileName);
            }

            try
            {

                using (FileStream newFile = System.IO.File.Create(rutaImagen + filename + extension))
                {
                    file.CopyTo(newFile);



                    newFile.Flush();


                }
            }
            catch (Exception ex)
            {

            }

            if (thumbnail == true)
            {
                this.RedimensionarImagen120(rutaImagen + filename + extension, rutaImagen + filename + "_thumbnail", "jpg");
            }

            if (completeFile == false)
            {
                this.EliminarArchivo(rutaImagen + "archivo" + extension);
            }



            return true;
        }


        bool editarArchivo(IFormFile file, string filepath, string filename)
        {


            return true;
        }




        bool actualizarArchivo(IFormFile file, string filepath, string filename)
        {
            return true;
        }


        /*bool RedimendiosarImagen() {

            bool guardado = false;

            using (Image image = Image.Load("aspose-logo.jpg"))
            {
                // Cambiar el tamaño de la imagen y guardar la imagen redimensionada
                image.Resize(300, 300);
                image.Save("SimpleResizing_out.jpg");
            }

            return guardado;

        }*/


        public bool EliminarArchivo(string ruta)
        {
            bool guardado = false;
            string directoryName = Path.GetDirectoryName(ruta) + '\\';


            try
            {

                if (Directory.Exists(directoryName))
                {
                    if (File.Exists(ruta)) //SI la ruta existe
                    {
                        File.Delete(ruta); //Se borra la imagen del directorio
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido eliminar el archivo.", ex);
            }





        }

        bool ActualizarArchivo(string ruta)
        {
            bool guardado = false;

            return guardado;
        }


        /*   public void RedimensionarImagen12_1( string rutafilename, string ruta) {
               Image image = Image.FromFile(rutafilename);
               Image thumb = image.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
               thumb.Save(Path.ChangeExtension(rutafilename, "thumb"));


           }
           */


        public void RedimensionarImagen120(string imageFileName, string thumbnailFileName, string extension)
        {
            const int thumbnailSize = 300;
            using (var image = Image.FromFile(imageFileName))
            {
                var imageHeight = image.Height;
                var imageWidth = image.Width;
                if (imageHeight > imageWidth)
                {
                    imageWidth = (int)(((float)imageWidth / (float)imageHeight) * thumbnailSize);
                    imageHeight = thumbnailSize;
                }
                else
                {
                    imageHeight = (int)(((float)imageHeight / (float)imageWidth) * thumbnailSize);
                    imageWidth = thumbnailSize;
                }

                using (var thumb = image.GetThumbnailImage(imageWidth, imageHeight, () => false, IntPtr.Zero))
                {

                    thumb.Save(Path.ChangeExtension(thumbnailFileName, extension));

                }

                //Save off the new thumbnail

            }


        }


        public void RedimensionarImagen(string imageFileName, string thumbnailFileName, int dimension)
        {
            int thumbnailSize = dimension;
            string extension = Path.GetExtension(imageFileName);
            using (var image = Image.FromFile(imageFileName))
            {

                var imageHeight = image.Height;
                var imageWidth = image.Width;
                if (imageHeight > imageWidth)
                {
                    imageWidth = (int)(((float)imageWidth / (float)imageHeight) * thumbnailSize);
                    imageHeight = thumbnailSize;
                }
                else
                {
                    imageHeight = (int)(((float)imageHeight / (float)imageWidth) * thumbnailSize);
                    imageWidth = thumbnailSize;
                }

                using (var thumb = image.GetThumbnailImage(imageWidth, imageHeight, () => false, IntPtr.Zero))
                {

                    thumb.Save(Path.ChangeExtension(thumbnailFileName, extension));

                }

                //Save off the new thumbnail

            }


        }


        public bool RedimensionarImagenProporcional(FileStream file, string rutafilename, string ruta, string filename, int X, int Y)
        {
            bool valor = false;

            Image imagen = Image.FromStream(file);
            int newWidth = imagen.Width / X;
            //imagen.ResizeWidthProportionally(newWidth);
            int newHeight = imagen.Height / Y;
            var newImage = new Bitmap(newWidth, newHeight);

            using (var a = Graphics.FromImage(newImage))
            {
                newWidth = imagen.Width / X;
                //imagen.ResizeWidthProportionally(newWidth);
                newWidth = imagen.Height / Y;
                a.DrawImage(imagen, 0, 0, newWidth, newHeight);
                newImage.Save(ruta + filename, ImageFormat.Jpeg);
            }

            //scaleImage.SaveAs(rutafilename);

            /*
                        using (Image image = Image.Load(rutafilename))
                        {
                            // Cambiar el tamaño de la imagen y guardar la imagen redimensionada
                            image.Resize(X, Y, ResizeType.LanczosResample);
                            image.Save(rutafilename);
                            return true;
                        }*/

            return valor;
        }

        public bool RedimensionarImagenCuadrado(FileStream file, string rutafilename, string ruta, string filename, int tamano)
        {

            bool valor = false;

            /*var img = Image.FromFile(rutafilename);
            var scaleImage = ImageResize.Scale(img, 300, 300);
            scaleImage.SaveAs(rutafilename);*/
            /* using (Image imagen = Image.FromStream(file))
              {
                  // Caché de datos de imagen
                  if (!imagen.IsCached)
                  {
                      imagen.CacheData();
                  }

                  // Especificar ancho y alto
                  int newWidth = imagen.Width / tamano;
                  //imagen.ResizeWidthProportionally(newWidth);
                  int newHeight = imagen.Height / tamano;
                  //imagen.ResizeHeightProportionally(newHeight);

                  // Guardar imagen
                  imagen.Save(rutafilename);
                  valor = true;
                  return valor;

              }*/

            return false; ;
        }






        public bool ActualizarArchivo()
        {
            throw new NotImplementedException();
        }



        public bool ActualizarArchivo(string ruta, string filename_thumbnail)
        {
            throw new NotImplementedException();
        }

        /* public bool ActualizarArchivo(string rutaOrigen, string rutaDestino, IFormFile file)
         {
             try {


                 if (this.EliminarArchivo(rutaOrigen)) { 

                 }


             } catch {


             }
         }*/

        public bool guardarImagen(string rutaDestino, IFormFile file)
        {
            try
            {
                using (FileStream newFile = System.IO.File.Create(rutaDestino))
                {
                    file.CopyTo(newFile);

                    newFile.Flush();

                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido crear la imagen", ex);
            }
        }

        public bool guardarImagen(string rutaDestino, IFormFile file, int ancho, string filenameThumbnail)
        {
            try
            {
                if (this.guardarImagen(rutaDestino, file))
                {
                    this.RedimensionarImagen(rutaDestino, filenameThumbnail, ancho);

                }
                else
                {

                    throw new Exception("No se ha podido crear la imagen");
                }


                return true;

            }
            catch (Exception ex) { throw new Exception("No se ha podido crear la imagen", ex); }
        }

        public bool guardarImagen(string rutaDestino, IFormFile file, string filenameThumbnail)
        {
            throw new NotImplementedException();
        }



        public bool Ejecutar(AccionFile accion)
        {
            if (accion.accion == "Guardar" || accion.accion == "guardar" || accion.accion == "Crear" || accion.accion == "crear")
            {
                if (accion.thumbnail == false)
                {
                    if (accion.rutaDesten != null)
                    {

                        if (accion.file != null)
                        {
                            return this.guardarImagen(accion.rutaDesten, accion.file);


                        }
                    }
                }

                if (accion.thumbnail == true)
                {

                    if (accion.rutaDestenThumbnail != null)
                    {
                        return this.guardarImagen(accion.rutaDesten, accion.file, accion.sizeThumbnail, accion.rutaDestenThumbnail);
                    }

                }

            }

            if (accion.accion == "Actualizar" || accion.accion == "actualizar" || accion.accion == "actualiza" || accion.accion == "update" || accion.accion == "Update")
            {
                if (accion.rutaOrigen != null && accion.rutaDesten != null && accion.file != null)
                {
                    if (accion.thumbnail == false)
                    {
                        if (this.ActualizarImagen(accion.rutaOrigen, accion.rutaDesten, accion.file))
                        {

                            return true;
                        }
                        else
                        {
                            throw new Exception("No se ha podido actualizar la imagen");
                        }
                    }
                    if (accion.thumbnail == true)
                    {
                        if (this.ActualizarImagen(accion.rutaOrigen, accion.rutaDesten, accion.file, accion.rutaOrigenThumbnail, accion.rutaDestenThumbnail, accion.sizeThumbnail))
                        {

                            return true;
                        }
                        else
                        {
                            throw new Exception("No se ha podido actualizar la imagen");
                        }
                    }
                }


            }


            if (accion.accion == "Eliminar" || accion.accion == "eliminar" || accion.accion == "elimina" || accion.accion == "delete" || accion.accion == "Delete")
            {
                if (accion.rutaOrigen != null)
                {
                    if (accion.thumbnail == false)
                    {
                        if (this.EliminarImagen(accion.rutaOrigen))
                        {

                            return true;
                        }
                        else
                        {
                            throw new Exception("No se ha podido eliminar la imagen");
                        }
                    }
                    if (accion.thumbnail == true)
                    {
                        if (this.EliminarImagen(accion.rutaOrigen, accion.rutaOrigenThumbnail))
                        {

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }


                return false;
            }
            return false;
        }



        public bool ActualizarImagen(string rutaOrigen, string rutaDestino, IFormFile file)
        {
            try
            {
                if (this.EliminarArchivo(rutaOrigen))
                {
                    if (this.guardarImagen(rutaDestino, file))
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("No se ha podido crear el thumbnail de la imagen para la actualización");
                    }

                }
                else
                {
                    throw new Exception("No se ha podido crear la imagen para la actualización");
                }

            }
            catch (Exception ex)
            {

                throw new Exception("No se ha podido actualizar la imagen", ex);
            }
        }

        public bool ActualizarImagen(string rutaOrigen, string rutaDestino, IFormFile file, string rutaOrigenThumbnail, string rutaDestinoThumbnail, int ancho)
        {
            try
            {
                if (this.EliminarArchivo(rutaOrigen))
                {
                    if (this.guardarImagen(rutaDestino, file))
                    {
                        if (this.EliminarArchivo(rutaOrigenThumbnail))
                        {
                            this.RedimensionarImagen(rutaDestino, rutaDestinoThumbnail, ancho);

                            return true;
                        }
                        else
                        {
                            throw new Exception("No se ha podido crear la imagen");
                        }
                    }
                    else
                    {
                        throw new Exception("No se ha podido crear el thumbnail de la imagen");
                    }

                }
                else
                {
                    throw new Exception("No se ha podido crear la imagen");
                }

            }
            catch (Exception ex)
            {

                throw new Exception("No se ha podido crear la imagen", ex);
            }
        }

        public bool EliminarImagen(string rutaOrigen)
        {
            try
            {
                if (this.EliminarArchivo(rutaOrigen))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex) { throw new Exception("No se ha podido eliminar la imagen", ex); }
        }

        public bool EliminarImagen(string rutaOrigen, string rutaOrigenThumbnail)
        {
            try
            {
                if (this.EliminarArchivo(rutaOrigen))
                {
                    if (this.EliminarArchivo(rutaOrigenThumbnail))
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("No se ha podido eliminar la imagen thumbnail de la imagen");
                    }
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex) { throw new Exception("No se ha podido eliminar la imagen", ex); }
        }

        public bool ActualizarArchivo(string rutaOrigen, string rutaDestino, IFormFile file)
        {
            throw new NotImplementedException();
        }
    }


}
