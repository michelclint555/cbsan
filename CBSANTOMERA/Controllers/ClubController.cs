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
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly string rutaServidor;

        public ClubController(IClubService clubService, IConfiguration configuration)
        {
            _clubService = clubService;
            rutaServidor = @"wwwroot\archivos\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<ClubDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _clubService.Lista();
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
        public async Task<IActionResult> Guardar([FromForm] ClubDTO club)
        {

            var rsp = new Response<ClubDTO>();





            try
            {





                rsp.status = true;



                rsp.value = await _clubService.Crear(club);


            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromForm] ClubDTO club)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _clubService.Editar(club);

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
                rsp.status = await _clubService.Eliminar(id); ;
                
                rsp.msg = "El club se ha eliminado";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {

            var rsp = new Response<ClubDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _clubService.VerClub(id);
                rsp.msg = "El club se ha cargado correctamente";

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

