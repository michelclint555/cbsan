using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.Utilidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class TecnicoController : ControllerBase
    {
        private readonly ITecnicoService _tecnicoService;
        private readonly ITecnicoEquipoService _tecnicoEquipoService;

        private readonly string rutaServidor;
        public TecnicoController(ITecnicoService tecnicoService, ITecnicoEquipoService _tecnicoEquipoService, IConfiguration configuration)
        {
            _tecnicoService = tecnicoService;
            this._tecnicoEquipoService = _tecnicoEquipoService;
            rutaServidor = @"wwwroot\archivos\Jugadores\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<TecnicoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _tecnicoService.Lista();
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
        public async Task<IActionResult> Guardar([FromForm] TecnicoDTO tecnico)
        {

            var rsp = new Response<TecnicoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _tecnicoService.Crear(tecnico);
                rsp.msg = "Se ha creado el Jugador correctamente";

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

            var rsp = new ResponseModel<TecnicoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await this._tecnicoService.BuscarTecnico(id);

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
        public async Task<IActionResult> Editar([FromForm] TecnicoDTO modelo)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await this._tecnicoService.Editar(modelo);

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
                rsp.value = await this._tecnicoService.Eliminar(id);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("ListaTecnicos/{id:int}")] //devuelve todos los tecnicos que no estan en el id de equipo introducido
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> ListaTodosTecnicos(int id)
        {

            var rsp = new Response<List<TecnicoEquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await this._tecnicoEquipoService.ListaTecnicosEquipoNoEstan(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("ListaEquipoTecnicos/{id:int}")] // devuelve todos los tecnicos de un equipo en concreto
        public async Task<IActionResult> ListaUnEquipoTecnicos(int id)
        {

            var rsp = new Response<List<TecnicoEquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await this._tecnicoEquipoService.ListaTecnicosUnEquipo(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpPost]
        [Route("GuardarTecncicoEquipo")]
        public async Task<IActionResult> Guardar([FromForm] EquipoDTO equipo)
        {

            var rsp = new Response<EquipoDTO>();

            try
            {

                /*if (equipo.TecnicoEquipos.Count == 0) {
                    rsp.status = false;
                    rsp.value = null;
                    rsp.msg = "La lista de Cuerpos técnicos está vacía";
                }*/
              
                    var ent = 0;


                    foreach (var item in equipo.TecnicoEquipos)
                    {
                        if (item.Funcion == "Entrenador" )
                        {
                            ent++;
                            if (ent > 1) {
                                rsp.msg = "Sólo puede haber un Cuerpo tecnico con la función de entrenador";
                                break;
                            }
                            continue;
                           

                        }
                       
                    }

                    if (ent == 1)
                    {
                        rsp.status = true;
                        rsp.value = await this._tecnicoEquipoService.ADDUpdateDeleteTecnicosEquipo(equipo);
                        rsp.msg = "cuerpos tecnicos creados correctamente";

                    }
                    if(ent == 0) {
                        rsp.status = false;
                        rsp.value = null;
                        rsp.msg = "El equipo debe de tener al menos un entrenador asignado";
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

