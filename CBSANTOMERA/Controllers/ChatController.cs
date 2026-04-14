using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
using Microsoft.AspNetCore.Authorization;
namespace CBSANTOMERA_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class ChatController : Controller
    {


        private readonly IMessageService _Service;
        public ChatController(IMessageService _Service)
        {
            this._Service = _Service;
        }
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] messageDTO modelo)
        {
            
            var rsp = new Response<messageDTO>();
            


            try
            {

                rsp.status = true;

                rsp.value = await _Service.Enviar(modelo);
                rsp.msg = "El mensaje se ha creado correctamente";

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
