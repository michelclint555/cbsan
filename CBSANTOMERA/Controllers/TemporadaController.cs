using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
using CBSANTOMERA.BLL.Servicios;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemporadaController : ControllerBase
    {
        private readonly ITemporadaService _Service;
        public TemporadaController(ITemporadaService Service)
        {
            _Service = Service;
        }

        [HttpGet]
        [Route("Lista_Full")]
        public async Task<IActionResult> Lista_Full()
        {

            var rsp = new Response<List<TemporadaDTO>>();
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
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<TemporadaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _Service.ListaVisibles();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("Temporada-Activa")]
        public async Task<IActionResult> ListaFull()
        {

            var rsp = new Response<TemporadaDTOSmall>();
            try
            {
                rsp.status = true;
                rsp.value = await _Service.TemporadaActiva();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("Activar/{id:int}")]
        public async Task<IActionResult> Activar(int id)
        {

            var rsp = new Response<AlbumDTO>();
            try
            {

                rsp.status = await this._Service.ActivarTemporada(id);

                if (rsp.status == true)
                {
                    rsp.msg = "La temporada se ha activado correctamente";
                }
                else
                {
                    rsp.msg = "Ha habido un error al activar la temporada";
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
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromForm] TemporadaDTO modelo)
        {

            var rsp = new Response<TemporadaDTO>();



            try
            {





                rsp.status = true;



                rsp.value = await _Service.Crear(modelo);
                rsp.msg = "La Temporada " + rsp.value.Nombre + " se ha creado correctamente";

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

            var rsp = new Response<TemporadaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.Ver(id);
                rsp.msg = "La Temporada se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Ver")]
        public async Task<IActionResult> TemporadaActiva()
        {

            var rsp = new Response<TemporadaDTOSmall>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.TemporadaActiva();
                rsp.msg = "La Temporada se ha cargado correctamente";

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
        public async Task<IActionResult> Eliminar(int id)
        {

            var rsp = new Response<TemporadaDTO>();

            try
            {

                rsp.status = await _Service.Eliminar(id);

                if (rsp.status == true)
                {
                    rsp.msg = "La Temporada se ha eliminado correctamente";
                }
                else
                {
                    rsp.msg = "Ha habido un error al eliminar la temporada";
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
        public async Task<IActionResult> Update([FromForm] TemporadaDTO modelo)
        {

            var rsp = new Response<TemporadaDTO>();



            try
            {


                rsp.status = await _Service.Editar(modelo);
                rsp.value = await _Service.Ver(modelo.Id);
                rsp.msg = "La temporada " + rsp.value.Nombre + " se creado correctamente";

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
