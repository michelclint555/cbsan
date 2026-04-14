using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
using CBSANTOMERA.BLL.Servicios;
using System.Text.Json;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Authorization;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class NoticiasController : ControllerBase
    {

        private readonly INoticiaService _noticiaService;
        private readonly IOpenIAService _IAService;
        private readonly string rutaServidor;
       
        public NoticiasController(INoticiaService noticiaService, IConfiguration configuration, IOpenIAService _IAService)
        {
            _noticiaService = noticiaService;
            rutaServidor = @"wwwroot\archivos\";
            this._IAService = _IAService;
        }
        [HttpGet]
        [Route("Lista")]
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<NoticiasDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _noticiaService.Lista_Noticias_Activas();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("Lista_Full")]//para el admin lista todo tanto activo o no
        public async Task<IActionResult> Lista_full()
        {

            var rsp = new Response<List<NoticiasDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _noticiaService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }




        [HttpGet]
        [Route("Lista_Noticia")]
        public async Task<IActionResult> Lista_Noticia_Empresa()
        {

            var rsp = new Response<List<NoticiasDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _noticiaService.Lista_Noticias_Empresas();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }



        [HttpGet]
        [Route("Lista-Inicio")]
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> ListaInicio()
        {

            var rsp = new Response<List<NoticiasDTOSmall>>();
            try
            {
                rsp.status = true;
                rsp.value = await _noticiaService.ListaInicio();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("ListaTipo")]
        public async Task<IActionResult> ListaTipo()
        {

            var rsp = new Response<List<TipoNoticium>>();
            try
            {
                rsp.status = true;
                rsp.value = await _noticiaService.ListaTipo();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromForm] NoticiasDTO noticia)
        {
           

            var rsp = new Response<NoticiasDTO>();



            try
            {
                rsp.status = true;



                rsp.value = await this._noticiaService.Crear(noticia);
                rsp.msg = "La noticia con título:  " + rsp.value.Titulo+ " se creado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] NoticiasDTO noticia)
        {

            var rsp = new Response<NoticiasDTO>();



            try
            {


                rsp.status = await _noticiaService.Editar(noticia);
                rsp.value = await _noticiaService.VerNoticia(noticia.IdNoticia);
                rsp.msg = "La Noticia " + rsp.value.Titulo + " se ha creado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpPost]
        [Route("Peticion")]
        public async Task<IActionResult> peticion([FromForm] string value)
        {

            var rsp = new Response<NoticiasDTO>();
            try
            {
                
                rsp.status = true;
                
                rsp.value = await this._IAService.Preguntar(value);
               /* NoticiasDTO? noticia =
               JsonSerializer.Deserialize<NoticiasDTO>(val);*/
               /* string[] partes = val.Split(new string[] { "**Título:", "**Subtítulo:", "**Contenido:" }, StringSplitOptions.None);

                string titulo = partes[1].Trim();
                string subtitulo = partes[2].Trim();
                string contenido = partes[3].Trim();


                NoticiasDTO noticias = new NoticiasDTO();
                noticias.Titulo = titulo;
                noticias.Contenido = contenido;*/
               // rsp.value = val;
               

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }
        [HttpGet]
        [Route("VerNoticia/{id:int}")]
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> VerNoticia(int id)
        {

            var rsp = new Response<NoticiasDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await this._noticiaService.VerNoticia(id);
                rsp.msg = "La noticia se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Empresa")]
        public async Task<IActionResult> VerEmpresa([FromQuery]string empresa)
        {

            var rsp = new Response<NoticiasDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await this._noticiaService.VerNoticiaEmpresa(empresa);
                rsp.msg = "La noticia se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Ver-Tipo/{id:int}")]
        public async Task<IActionResult> VerTipo(int id)
        {

            var rsp = new Response<TipoNoticium>();

            try
            {
                rsp.status = true;
                rsp.value = await this._noticiaService.VerTipoNoticia(id);
                rsp.msg = "El tipo de noticia se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> EliminarNoticia(int id)
        {

            var rsp = new Response<NoticiasDTO>();

            try
            {

                rsp.status = await _noticiaService.Eliminar(id);

                if (rsp.status == true)
                {
                    rsp.msg = "El album se ha eliminado correctamente";
                }
                else
                {
                    rsp.msg = "Ha habido un error al eliminar el album";
                }


            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

    }
}
