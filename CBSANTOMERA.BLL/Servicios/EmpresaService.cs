
using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
//using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Microsoft.AspNetCore.Http;
using CBSANTOMERA.DAL.Repositorios.Contrato;

namespace CBSANTOMERA.BLL.Servicios
{
    public class EmpresaService : IEmpresaService
    {

        private readonly IGenericRepository<Empresa> _Repository;
       // private readonly IContratoEmpresaService contratoEmpresaService;
        //private readonly INoticiaService noticiaService;
        //private readonly IFotoPromocionService _FotosAlbumRepository;
        private readonly IArchivosService _ArchivosRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Empresas\";

        public EmpresaService(IGenericRepository<Empresa> Repository,  IMapper mapper,  IArchivosService _ArchivosRepository)
        {
            this._ArchivosRepository = _ArchivosRepository;
            _Repository = Repository;
            _mapper = mapper;
          // this.contratoEmpresaService = contratoEmpresaService;
          //  this.noticiaService = noticiaService;
           


        }

        public async Task<EmpresaDTO> Crear(EmpresaDTO modelo)
        {
            var modeloCreado = new Empresa();
            
            string name = "";
            string filename = "";
            string hasha = modeloCreado.Id.ToString();
            name = hasha.GetHashCode().ToString();
            string extension = "";


            try {
               
                
                modelo.FechaRegistro = DateTime.Now;
                modelo.EsActivo = false;
                var modeloEncontrado = await _Repository.Obtener(u => u.Nombre == modelo.Nombre);

                if (modelo.imagen == null ) {
                    if (modeloEncontrado == null)
                    {
                        //modelo.Logo =null;
                        modelo.Logo = "logo_empresa.png";
                        modeloCreado = await _Repository.Crear(_mapper.Map<Empresa>(modelo));
                        return _mapper.Map<EmpresaDTO>(modeloCreado);

                    }
                    else {
                        throw new Exception("Ya hay una empresa con el nombre: "+modelo.Nombre);
                    }


                    throw new Exception("Ya hay una empresa con el nombre: " + modelo.Nombre);


                }
                
                extension = Path.GetExtension(modelo.imagen.FileName);
                
                
              
               
                modeloCreado = await _Repository.Crear(_mapper.Map<Empresa>(modelo));

                 hasha = modeloCreado.Id.ToString();
                name = hasha.GetHashCode().ToString();
                filename = name + extension;
                modelo.Logo = filename;
               
            }

            catch (Exception e)
            {
                throw new Exception("No se ha podido crear la empresa en la BBDD", e);
            }




            string carpetaAlbum = "";

            if (modeloCreado.Id == 0)
            {
                throw new TaskCanceledException("No se pudo crear la Empresa"+ modeloCreado.Nombre);


            }

            if (!Directory.Exists(rutaServidor))
            {
                Directory.CreateDirectory(rutaServidor);
            }

            if (!Directory.Exists(rutaServidor + carpetaAlbum))
            {
                Directory.CreateDirectory(rutaServidor + carpetaAlbum);
            }

            string rutaImagen = Path.Combine(rutaServidor, carpetaAlbum);


            try
            {
                AccionFile accion = new AccionFile();
                accion.accion = "Crear";
                accion.rutaDesten = Path.Combine( rutaImagen, filename);
                accion.thumbnail = false;
                accion.file = modelo.imagen;
                modelo.Id = modeloCreado.Id;
                modelo.Logo = filename;
                modelo.FechaModificacion = DateTime.Now;
                modelo.FechaRegistro = DateTime.Now;
               
                this._ArchivosRepository.Ejecutar(accion);
                modelo.imagen = null;
                var res = await _Repository.Editar(_mapper.Map<Empresa>(modelo));

            }
            catch (Exception e)
            {
                throw new TaskCanceledException("Error al crear las imagen de la empresa.");
            }

            return _mapper.Map<EmpresaDTO>(modeloCreado);
        }

        public async Task<bool> Editar(EmpresaDTO modelo)
        {
            try

            {

                string name = "";
                var modeloEncontrado = await _Repository.Obtener(u => u.Id == modelo.Id);
                

                
               
              


                if (modelo != null)
                {

                    if (modelo.imagen != null)
                    {
                        string hasha = modelo.Id.ToString()+DateTime.Now;
                        name = hasha.GetHashCode().ToString();
                        string extension = Path.GetExtension(modelo.imagen.FileName);
                      
                        string filename = name + extension;
                        
                        string carpeta = "";
                        string rutaImagen = Path.Combine(rutaServidor, carpeta);
                        string filepath = Path.Combine(rutaImagen, modelo.Logo);
                        //thumbnail =  Path.GetFileName (modelo.imagen.FileName);
                        //thumbnail = Path.GetFileName(modelo.Thumbnail) + extension;
                        AccionFile accion = new AccionFile();
                        string filepathNewImage= Path.Combine(rutaImagen, filename);
                        string logo = modelo.Logo.Trim();
                        if (logo == "logo_empresa.png")
                        {
                          
                            accion.accion = "Crear";
                            
                            accion.rutaDesten = Path.Combine(rutaImagen, filename);

                            accion.thumbnail = false;
                            accion.file = modelo.imagen;
                            modelo.Logo = filename;
                        }
                        else {
                           
                            accion.accion = "Actualizar";
                            accion.rutaOrigen = Path.Combine(rutaImagen, modelo.Logo);
                            accion.rutaDesten = Path.Combine(rutaImagen, filename);

                            accion.thumbnail = false;
                            accion.file = modelo.imagen;
                            modelo.Logo = filename;

                        }

                       
                      
                        //accion.sizeThumbnail = 300; 
                        this._ArchivosRepository.Ejecutar(accion);
                        
                      


                    }

                    modelo.FechaModificacion = DateTime.Now;

                    //Procedemos a modificar las fotos, obtenemos todas la fotos del IDAlbum y la lista de las fotos a mantenener en la BBDD, comprobamos si la lista de fotos de la BBDD y la lista de fotos del cliente no se ha modificado.



                    //Procedemos a añadir las nuevas fotos






                    return await _Repository.Editar(_mapper.Map<Empresa>(modelo));

                }
                else {
                    return false;
                }

                return true;

            }

            catch (Exception e) {

                throw new TaskCanceledException("Error al editar la empresa" +
                    " en la BBDD.");
            }
        }

        


        public async Task<EmpresaDTO> Ver(int id)
        {
            try {
                
                var encontrado = await _Repository.Obtener(u => u.Id == id);
                EmpresaDTO modelo = _mapper.Map<EmpresaDTO>(encontrado);
              
                return modelo;
              
                             

                

            } catch (Exception e) {
                throw new TaskCanceledException("Error al consultar la empresa en la BBDD.");
            }
        }


        


            public async Task<bool> Eliminar(int id)
        {
            try
            {

                //EmpresaDTO modelo = await this.Ver(id);
                var encontrado = await _Repository.Obtener(u => u.Id == id);

                try {

                    if (encontrado.Logo != null) {

                        string carpeta = "";
                        string rutaImagen = Path.Combine(rutaServidor, carpeta);
                        string filepath = Path.Combine(rutaImagen, encontrado.Logo);

                        AccionFile accion = new AccionFile();
                        accion.accion = "Eliminar";
                        accion.rutaOrigen = Path.Combine(rutaImagen, encontrado.Logo);


                        accion.thumbnail = false;



                        this._ArchivosRepository.Ejecutar(accion);
                    }







                    if (await _Repository.Eliminar(_mapper.Map<Empresa>(encontrado)))
                    {



                        return true;
                    }
                    else {
                    return false;
                    }
                   



                }
                catch (Exception ex) { 
                
                
                }



                






            }
            catch (Exception e) { throw; }

            return false;
        }


        

        public async Task<List<EmpresaDTO>> ListaCompleta()
        {
            try
            {
                var lista= await _Repository.Consultar();

               lista= lista.OrderByDescending(d => d.FechaRegistro);


                List<EmpresaDTO> modelos=  _mapper.Map<List<EmpresaDTO>>(lista.ToList());

                

                return modelos;

            }
            catch
            { 
                throw;

            }
        }


        public async Task<List<EmpresaDTOSmall>> Lista()
        {
            try
            {
                var lista = await _Repository.Consultar(m=> m.EsActivo == true);

                


                List<EmpresaDTOSmall> modelos = _mapper.Map<List<EmpresaDTOSmall>>(lista.ToList());



                return modelos;

            }
            catch
            {
                throw;

            }
        }

        string ComputeFourDigitStringHash(string cadena)
        {

            int hash = cadena.GetHashCode() % 10000;
            return hash.ToString("0000");
        }


 public async Task<EmpresaDTOSmall> VerPorNombre(string nombre)
{
    try
    {
        string Normalizar(string texto)
        {
            texto = texto.ToLower().Trim();

            texto = texto.Normalize(System.Text.NormalizationForm.FormD);
            var chars = texto.Where(c =>
                System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                != System.Globalization.UnicodeCategory.NonSpacingMark
            ).ToArray();

            return new string(chars);
        }

        // ⚠️ IMPORTANTE: traemos todos para comparar normalizado
        var empresas = await this.ListaCompleta();

        var encontrado = empresas.FirstOrDefault(e =>
            Normalizar(e.Nombre) == Normalizar(nombre)
        );

        if (encontrado == null)
            throw new Exception("Empresa no encontrada");

        EmpresaDTOSmall empresa = new EmpresaDTOSmall();
        empresa.Id = encontrado.Id;
        empresa.Nombre = encontrado.Nombre;
        empresa.Url = encontrado.Url;
        empresa.Logo = encontrado.Logo;

        return empresa;
    }
    catch (Exception)
    {
        throw new TaskCanceledException("Error al consultar la empresa en la BBDD.");
    }
}



        



        public async Task<EmpresaDTOSmall> VerSmall(int id)
        {
            try
            {

                var encontrado = await _Repository.Obtener(u => u.Id == id);

                EmpresaDTOSmall empresa = new EmpresaDTOSmall();
                empresa.Id = encontrado.Id;
                empresa.Nombre = encontrado.Nombre;
                empresa.Url = encontrado.Url;
                empresa.Logo = encontrado.Logo;
                //empresa.Tipo = encontrado.Tipo;
                //EmpresaDTOSmall modelo = _mapper.Map<EmpresaDTOSmall>(encontrado);

                return empresa;





            }
            catch (Exception e)
            {
                throw new TaskCanceledException("Error al consultar la empresa en la BBDD.");
            }
        }

       
    }

   
}
