using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
using Microsoft.AspNetCore.Authorization;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorService _jugadorService;
        private readonly string rutaServidor;
        public JugadorController(IJugadorService jugadorService, IConfiguration configuration)
        {
            _jugadorService = jugadorService;
            rutaServidor = @"wwwroot\archivos\Jugadores\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<JugadorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _jugadorService.Lista();
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
        public async Task<IActionResult> Guardar([FromForm] JugadorDTO jugador)
        {

            var rsp = new Response<JugadorDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _jugadorService.Crear(jugador);
                rsp.msg = "Se ha creado el Jugador correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Editar([FromForm] JugadorDTO jugador)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _jugadorService.Editar(jugador);

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
                rsp.value = await _jugadorService.Eliminar(id);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Buscar/{id:int}")]
        public async Task<IActionResult> Buscar(int id)
        {

            var rsp = new ResponseModel<JugadorDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _jugadorService.BuscarJugador(id);

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
