using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
using CBSANTOMERA.MODEL;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoEmpresaController : ControllerBase
    {

        private readonly IContratoEmpresaService _Service;
        public ContratoEmpresaController(IContratoEmpresaService Service)
        {
            _Service = Service;
        }
        [HttpGet]
        [Route("Lista")] //Para el Fronted usuario obtenenemos la lista de empresas contrato con el club de la temporada introducida
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<EmpresaDTOSmall>>();
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
        public async Task<IActionResult> ListaFull([FromQuery] int temporada)
        {

            var rsp = new Response<List<ContratoEmpresaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _Service.ListaCompleta(temporada);
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
        public async Task<IActionResult> Guardar([FromForm] ContratoEmpresaDTO modelo)
        {

            var rsp = new Response<ContratoEmpresaDTO>();



            try
            {





                rsp.status = true;



                rsp.value = await _Service.Crear(modelo);
                rsp.msg = "El contrato de la empresa " + rsp.value.Empresa.Nombre + " se ha creado correctamente";

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

            var rsp = new Response<ContratoEmpresaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.Ver(id);
                rsp.msg = "El contrato de la empresa "+ rsp.value.Empresa.Nombre+" se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("Ver-Empresa/{id:int}")]
        public async Task<IActionResult> VerSmall(int id)
        {

            var rsp = new Response<ContratoEmpresaDTOSmall>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.ContratoEmpresaSmall(id);
                rsp.msg = "El contrato de la empresa se ha cargado correctamente";

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
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {

            var rsp = new Response<EmpresaDTO>();

            try
            {

                rsp.status = await _Service.Eliminar(id);

                if (rsp.status == true)
                {
                    rsp.msg = "La empresa se ha eliminado correctamente.";
                }
                else
                {
                    rsp.msg = "Ha habido un error al eliminar La empresa.";
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
        public async Task<IActionResult> Update([FromForm] ContratoEmpresaDTO modelo)
        {

            var rsp = new Response<ContratoEmpresaDTO>();



            try
            {


                rsp.status = await _Service.Editar(modelo);

                if (rsp.status == true)
                {
                    rsp.value = await _Service.Ver(modelo.Id);
                    rsp.msg = "El contrato de la empresa " + rsp.value.Empresa.Nombre + " se ha actualizado correctamente.";

                }
                else {
                    rsp.msg = "El contrato de la empresa " + rsp.value.Empresa.Nombre + " no se ha podido actualizar.";
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
