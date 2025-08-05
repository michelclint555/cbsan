using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        
        
            private readonly IPromocionService _Service;
            public PromocionController(IPromocionService Service)
            {
                _Service = Service;
            }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<PromocionDTOSmall>>();
            try
            {
                rsp.status = true;
                rsp.value = await _Service.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
            [Route("Lista_Full")]
            public async Task<IActionResult> ListaFull()
            {

                var rsp = new Response<List<PromocionDTO>>();
                try
                {
                    rsp.status = true;
                    rsp.value = await _Service.ListaCompleta();
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
            public async Task<IActionResult> Guardar([FromForm] PromocionDTO modelo)
            {

                var rsp = new Response<PromocionDTO>();



                try
                {






                rsp.value = await _Service.Crear(modelo);

                if (rsp.value != null)
                {
                    rsp.status = true;
                    rsp.msg = "La promoción " + rsp.value.Titulo + " se ha creado correctamente";
                }
                else {
                    rsp.status = false;
                    rsp.msg = "No se podido crear la Novedad con título " + rsp.value.Titulo;
                }
                    
                    

                }
                catch (Exception ex)
                {

                    rsp.status = false;
                    rsp.msg = ex.Message;
                }
                return Ok(rsp);
            }


            [HttpGet]
            [Route("Ver/{id:int}")]
            public async Task<IActionResult> Ver(int id)
            {

                var rsp = new Response<PromocionDTO>();

                try
                {
                    rsp.status = true;
                    rsp.value = await _Service.Ver(id);
                    rsp.msg = "La promoción se ha cargado correctamente";

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
            public async Task<IActionResult> Eliminar([FromRoute ]int id)
            {

                var rsp = new Response<PromocionDTO>();

                try
                {

                    rsp.status = await _Service.Eliminar(id);

                    if (rsp.status == true)
                    {
                        rsp.msg = "La promoción se ha eliminado correctamente";
                    }
                    else
                    {
                        rsp.msg = "Ha habido un error al eliminar La promoción";
                    }


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
            public async Task<IActionResult> Update([FromForm] PromocionDTO modelo)
            {

                var rsp = new Response<PromocionDTO>();



                try
                {


                    rsp.status = await _Service.Editar(modelo);
                    rsp.value = await _Service.Ver(modelo.Id);
                    rsp.msg = "La promoción " + rsp.value.Titulo + " se ha actualizado correctamente";

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
