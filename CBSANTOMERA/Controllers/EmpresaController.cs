using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private readonly IEmpresaService _Service;
        public EmpresaController(IEmpresaService Service)
        {
            _Service = Service;
        }
        [HttpGet]
        [Route("Lista")]
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
        public async Task<IActionResult> ListaFull()
        {

            var rsp = new Response<List<EmpresaDTO>>();
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
        public async Task<IActionResult> Guardar([FromForm] EmpresaDTO modelo)
        {

            var rsp = new Response<EmpresaDTO>();



            try
            {





                rsp.status = true;



                rsp.value = await _Service.Crear(modelo);
                rsp.msg = "La empresa " + rsp.value.Nombre + " se ha creado correctamente";

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

            var rsp = new Response<EmpresaDTO>();

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


        [HttpGet]
        [Route("Ver-Empresa/{id:int}")]
        public async Task<IActionResult> VerSmall(int id)
        {

            var rsp = new Response<EmpresaDTOSmall>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.VerSmall(id);
                rsp.msg = "La empresa se ha cargado correctamente";

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
        public async Task<IActionResult> Update([FromForm] EmpresaDTO modelo)
        {

            var rsp = new Response<EmpresaDTO>();



            try
            {


                rsp.status = await _Service.Editar(modelo);

                if (rsp.status == true)
                {
                    rsp.value = await _Service.Ver(modelo.Id);
                    rsp.msg = "La empresa " + rsp.value.Nombre + " se ha actualizado correctamente";
                }
                else {
                    rsp.msg = "La empresa " + rsp.value.Nombre + " no se ha actualizado";
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
