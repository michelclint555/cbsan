
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
using System.Diagnostics.Contracts;

namespace CBSANTOMERA.BLL.Servicios
{
    public class ContratoEmpresaService : IContratoEmpresaService
    {

        private readonly IGenericRepository<ContratoEmpresa> _Repository;
        //private readonly IFotoPromocionService _FotosAlbumRepository;
        private readonly IArchivosService _ArchivosRepository;
        private readonly INoticiaService noticiaService;
        private readonly IEmpresaService _empresaService;
        private readonly ITemporadaService _TemporadaRepository;
        private readonly IMapper _mapper;
        private readonly string rutaServidor = @"wwwroot\archivos\Empresas\";

        public ContratoEmpresaService(IGenericRepository<ContratoEmpresa> Repository, IMapper mapper,  IArchivosService _ArchivosRepository, ITemporadaService temporadaService, IEmpresaService empresaService,  INoticiaService noticiaService)
        {
            this._ArchivosRepository = _ArchivosRepository;
            _Repository = Repository;
            this._empresaService = empresaService;
            this._TemporadaRepository = temporadaService;
            _mapper = mapper;
            this.noticiaService = noticiaService;
           


        }

        public async Task<ContratoEmpresaDTO> Crear(ContratoEmpresaDTO modelo)
        {
            var modeloCreado = new ContratoEmpresa();
            
            string name = "";
            string filename = "";
            string extension = "";


            try {
               
                
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                TemporadaDTOSmall temporada = await this._TemporadaRepository.TemporadaActiva();
                /*modeloCreado.Url = modelo.Url;
                modeloCreado.Id = modelo.Id;
                modeloCreado.Temporada = modelo.Temporada.Id;
                modeloCreado.Condiciones = modelo.Condiciones;
                modeloCreado.Contribucion = modelo.Contribucion;
                modeloCreado.Empresa = modelo.Empresa.Id;
                modeloCreado.FechaCreacion = DateTime.Now;
                modeloCreado.FechaModificacion = DateTime.Now;
                modeloCreado.Tipo = modelo.Tipo;
                //modeloCreado.TemporadaNavigation = _mapper.Map<Temporada>(modelo.Temporada);
                //modeloCreado.EmpresaNavigation = _mapper.Map<Empresa>(modelo.Empresa);
                */
                ContratoEmpresa contrato0 = modelo.ToModelo();
                contrato0.Temporada = temporada.Id;
                //modelo.Logo =null;
                modeloCreado = await _Repository.Crear(contrato0);

                if (modeloCreado == null) {
                    throw new Exception("No se ha podido crear la empresa en la BBDD");

                }

                ContratoEmpresaDTO contrato = ContratoEmpresaDTO.ToDto(modeloCreado);

                return contrato;
               
            }

            catch (Exception e)
            {
                throw new Exception("No se ha podido crear la empresa en la BBDD", e);
            }
        }

        public async Task<bool> Editar(ContratoEmpresaDTO modelo)
        {
            try

            {

                string name = "";
                var modeloEncontrado = await _Repository.Obtener(u => u.Id == modelo.Id);
                
                if (modeloEncontrado != null)
                {
                    //ContratoEmpresaDTO contrato = new ContratoEmpresaDTO();
                    modelo.FechaCreacion = modeloEncontrado.FechaCreacion;
                    modelo.FechaModificacion = modeloEncontrado.FechaModificacion;
                    ContratoEmpresa contrato =modelo.ToModelo();
                    if (contrato.Noticia== -1) { contrato.Noticia = null; }
                    return await _Repository.Editar(contrato);
                    

                }
                else {
                    return false;
                }

                //return true;

            }

            catch (Exception e) {

                throw new TaskCanceledException("Error al editar la empresa" +
                    " en la BBDD.");
            }
        }

        


        public async Task<ContratoEmpresaDTO> Ver(int id)
        {
            try {
                
                var encontrado = await _Repository.Obtener(u => u.Id == id);
                ContratoEmpresaDTO modelo = ContratoEmpresaDTO.ToDto(encontrado);
                modelo.Temporada = await _TemporadaRepository.Ver(modelo.Temporada.Id);
                modelo.Empresa = await _empresaService.VerSmall(modelo.Empresa.Id);
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

                    if (encontrado != null) {
                        if (await _Repository.Eliminar(_mapper.Map<ContratoEmpresa>(encontrado)))
                        {

                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                }
                catch (Exception ex) { 
                
                
                }



                






            }
            catch (Exception e) { throw; }

            return false;
        }


        

        public async Task<List<ContratoEmpresaDTO>> ListaCompleta()
        {
            try
            {
                var lista= await _Repository.Consultar();

               lista= lista.OrderByDescending(d => d.FechaCreacion);
                List<ContratoEmpresaDTO> modelos = new List<ContratoEmpresaDTO>();

                foreach (var item in lista)

                {

                    ContratoEmpresaDTO contrato = ContratoEmpresaDTO.ToDto(item);
                    contrato.Temporada = await _TemporadaRepository.Ver(contrato.Temporada.Id);
                    contrato.Empresa = await _empresaService.VerSmall(contrato.Empresa.Id);

                    modelos.Add(contrato);
                    
                    
                }

                //List<ContratoEmpresaDTO> modelos=  _mapper.Map<List<ContratoEmpresaDTO>>(lista.ToList());

                

                return modelos;

            }
            catch
            { 
                throw;

            }
        }

        public async Task<List<ContratoEmpresaDTO>> ListaCompleta(int temporada)
        {
            try
            {
                var lista = await _Repository.Consultar(t=>t.Temporada== temporada);

                lista = lista.OrderByDescending(d => d.FechaCreacion);
                List<ContratoEmpresaDTO> modelos = new List<ContratoEmpresaDTO>();

                foreach (var item in lista)

                {

                    ContratoEmpresaDTO contrato = ContratoEmpresaDTO.ToDto(item);
                    contrato.Temporada = await _TemporadaRepository.Ver(contrato.Temporada.Id);
                    contrato.Empresa = await _empresaService.VerSmall(contrato.Empresa.Id);

                    modelos.Add(contrato);


                }

                //List<ContratoEmpresaDTO> modelos=  _mapper.Map<List<ContratoEmpresaDTO>>(lista.ToList());



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

                var temporada = await _TemporadaRepository.TemporadaActiva();
                var lista = await _Repository.Consultar(m=> m.Temporada == temporada.Id );
                List<EmpresaDTOSmall> modelos = new List<EmpresaDTOSmall>();

                foreach (var item in lista)

                {

                    EmpresaDTOSmall empresa = new EmpresaDTOSmall();
                    
                    empresa = await _empresaService.VerSmall(item.Empresa);
                    empresa.Tipo = item.Tipo;

                    modelos.Add(empresa);


                }

                //List<EmpresaDTOSmall> modelos = _mapper.Map<List<EmpresaDTOSmall>>(lista.ToList());



                return modelos;

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<EmpresaDTOSmall>> Lista(int temporada)
        {
            try
            {

                var temporada0 = await _TemporadaRepository.Ver(temporada);
                var lista = await _Repository.Consultar(m => m.Temporada == temporada);
                List<EmpresaDTOSmall> modelos = new List<EmpresaDTOSmall>();

                foreach (var item in lista)

                {

                    EmpresaDTOSmall empresa = new EmpresaDTOSmall();

                    empresa = await _empresaService.VerSmall(item.Empresa);
                    empresa.Tipo = item.Tipo;

                    modelos.Add(empresa);


                }

                //List<EmpresaDTOSmall> modelos = _mapper.Map<List<EmpresaDTOSmall>>(lista.ToList());



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


        public async Task<NoticiasDTOSmall> VerPorNombre(string nombre)
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

                // ⚠️ traemos empresas con contrato
                var empresas = await this.ListaCompleta();

                var encontrado = empresas.FirstOrDefault(e =>
                    Normalizar(e.Empresa.Nombre) == Normalizar(nombre)
                );

                if (encontrado == null)
                    throw new Exception("Empresa no encontrada");

                if (encontrado.Noticia == null)
                    throw new Exception("La empresa no tiene noticia asociada");

                var noticia0 = await this.noticiaService.VerNoticia(encontrado.Noticia.Value);

                if (noticia0 == null)
                    throw new Exception("No se ha encontrado la noticia");

                // 🔥 MAPEADO COMPLETO
                NoticiasDTOSmall noticia = new NoticiasDTOSmall
                {
                    IdNoticia = noticia0.IdNoticia,
                    Titulo = noticia0.Titulo,
                    Subtitulo = noticia0.Subtitulo,
                    Contenido = noticia0.Contenido,
                    Portada = noticia0.Portada,
                    TipoNoticia = noticia0.TipoNoticia,
                    Fecha = noticia0.Fecha,
                    ThumbnailImageSrc = noticia0.ThumbnailImageSrc,
                    Nuevo = noticia0.Nuevo
                };

                return noticia;
            }
            catch (Exception ex)
            {
                throw new TaskCanceledException(
                    "Error al consultar la empresa en la BBDD: " + ex.Message
                );
            }
        }



        /*  public async Task<NoticiasDTOSmall> ObtenerNoticiaPatrocinador(string nombre)
          {
              try
              {
                  var empresa = await VerPorNombre(nombre);

                  if (empresa == null)
                      throw new Exception("Empresa no encontrada: " + nombre);

                  var patrocinador = await contratoEmpresaService.Ver(empresa.Id);

                  if (patrocinador == null || patrocinador.Noticia == null)
                      throw new Exception("No se ha encontrado el patrocinador de la empresa: " + nombre);

                  var noticia0 = await noticiaService.VerNoticia(patrocinador.Noticia.Value);

                  if (noticia0 == null)
                      throw new Exception("No se ha encontrado la noticia asociada");

                  return new NoticiasDTOSmall
                  {
                      Titulo = noticia0.Titulo,
                      Subtitulo = noticia0.Subtitulo,
                      TipoNoticia = noticia0.TipoNoticia,
                      Portada = noticia0.Portada,
                      Contenido = noticia0.Contenido,
                      Fecha = noticia0.Fecha
                  };
              }
              catch (Exception ex)
              {
                  throw new Exception("Error al obtener la noticia del patrocinador: " + ex.Message);
              }
          }*/

        public async Task<ContratoEmpresaDTOSmall> ContratoEmpresaSmall(int id)
        {
            try
            {

                var encontrado = await _Repository.Obtener(u => u.Id == id);
                ContratoEmpresaDTOSmall modelo = _mapper.Map<ContratoEmpresaDTOSmall>(encontrado);

                return modelo;





            }
            catch (Exception e)
            {
                throw new TaskCanceledException("Error al consultar la empresa en la BBDD.");
            }
        }

       
    }

   
}
