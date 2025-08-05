using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaJugadorController : ControllerBase
    {
        private readonly ICategoriaJugadorService _categoriaService;
        //private readonly string rutaServidor;
        public CategoriaJugadorController(ICategoriaJugadorService categoriaService, IConfiguration configuration)
        {
            _categoriaService = categoriaService;
            //rutaServidor = @"wwwroot\archivos\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<CategoriaJugadorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _categoriaService.Lista();
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
        public async Task<IActionResult> Guardar([FromForm] CategoriaJugadorDTO categoria)
        {

            var rsp = new Response<CategoriaJugadorDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _categoriaService.Crear(categoria);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromForm] CategoriaJugadorDTO categoria)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _categoriaService.Editar(categoria);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _categoriaService.Eliminar(id);

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
