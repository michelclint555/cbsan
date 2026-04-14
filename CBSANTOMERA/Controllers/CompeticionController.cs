using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Authorization;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class CompeticionController : ControllerBase
    {
        private readonly ICompeticionService _competicionService;
        
      

        public CompeticionController(
            ICompeticionService competicionService
           )
        {
            _competicionService = competicionService;
    
        }
        [HttpPost]
        [Route("Guardar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Crear([FromForm] CompeticionDTO dto)
        {
            var rsp = new Response<CompeticionDTO>();

            try
            {
                if (string.IsNullOrEmpty(dto.Nombre))
                    throw new Exception("El nombre es obligatorio");

                rsp.status = true;
                rsp.value = await _competicionService.CrearCompeticion(dto);
                rsp.msg = "Competición creada correctamente";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditarCompeticion(int id, [FromBody] CompeticionDTO dto)
        {
            dto.Id = id;
            var ok = await _competicionService.EditarCompeticion(dto);
            return ok ? Ok() : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCompeticion(int id)
        {
            await _competicionService.EliminarCompeticion(id);
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> VerCompeticion(int id)
        {
            var comp = await _competicionService.VerCompeticion(id);
            return Ok(comp);
        }

        [HttpGet("lista")]
        public async Task<IActionResult> Lista()
        {
            var comp = await _competicionService.Lista();
            return Ok(comp);
        }

        [HttpPost("{id}/iniciar")]
        public async Task<IActionResult> IniciarCompeticion(int id)
        {
            await _competicionService.IniciarCompeticionAsync(id);
            return Ok();
        }


    }
}
