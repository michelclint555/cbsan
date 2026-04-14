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
    public class EquipoController : ControllerBase
    {

        private readonly IEquipoService _equipoService;
        private readonly string rutaServidor;
        public EquipoController(IEquipoService equipoService, IConfiguration configuration)
        {
            _equipoService = equipoService;
            rutaServidor = @"wwwroot\archivos\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<EquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("MisEquipos")]
        public async Task<IActionResult> MisEquipos([FromQuery] int Temporada)
        {

            var rsp = new Response<List<EquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.ListaEquiposMiClub(Temporada);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet("Lista-Equipos")]

        public async Task<IActionResult> ListaEquipos([FromQuery] int Club, int Temporada)
        {

            var rsp = new Response<List<EquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.ListaEquiposClub(Club, Temporada);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> MiEquipo( [FromQuery]int id)
        {

            var rsp = new Response<List<EquipoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.ListaEquiposClub(id );
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
        public async Task<IActionResult> Guardar([FromForm] EquipoDTO equipo)
        {

            var rsp = new Response<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.Crear(equipo);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("Guardar_Small")]
        public async Task<IActionResult> Guardar_Small([FromForm] EquipoDTO equipo)
        {

            var rsp = new Response<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.Crear(equipo);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpGet]
        [Route("club/{id:int}")]
        public async Task<IActionResult> VerClub(int id)
        {

            var rsp = new Response<List<EquipoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.ListaEquiposClub(id);
                rsp.msg = "Los equipos se han cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("club-Categoria/{id:int}")]
        public async Task<IActionResult> VerClubCategoria(int id )
        {

            var rsp = new Response<List<EquipoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.ListaEquiposClub(id);
                rsp.msg = "Los equipos se han cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("GuardarFotoEquipo")]
        public async Task<IActionResult> CrearFotoEquipo([FromForm] EquipoDTO equipo)
        {

            var rsp = new Response<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.CrearFotoEquipo(equipo);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpPost]
        [Route("Add-Patrocinador")]
        public async Task<IActionResult> AddPatrocinador([FromBody] patrocinadorEquipo equipo)
        {

            var rsp = new Response<EquipoDTO>();
            patrocinadorEquipo equipo1 = new patrocinadorEquipo( );

            equipo1.equipo = equipo.equipo;
            equipo1.temporada = equipo.patrocinador;
            equipo1.patrocinador = equipo.patrocinador;
            try
            {

                
                rsp.status = true;
                rsp.value = await _equipoService.AddPatrocinador(equipo1.equipo, equipo1.temporada, equipo1.patrocinador);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }





        [HttpPost]
        [Route("GuardarMiEquipo")]
        public async Task<IActionResult> CrearMiEquipo([FromForm] EquipoDTO equipo)
        {

            var rsp = new Response<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.CrearMiEquipo(equipo);

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
        public async Task<IActionResult> Editar([FromForm] EquipoRivalDTO equipo)
        {

            var rsp = new Response<bool>();

            try
            {
               // rsp.status = true;
                rsp.status = await _equipoService.Editar(equipo);
                if (rsp.status == false) {
                    throw new Exception("No se ha podido editar el equipo.");
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
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoService.Eliminar(id);

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

            var rsp = new Response<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await this._equipoService.BuscarEquipo(id);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Buscar")]
        public async Task<IActionResult> Buscarv([FromQuery]int idClub, int idCategoria)
        {

            var rsp = new Response<List<EquipoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await this._equipoService.ListaEquiposClubCategoria(idClub, idCategoria);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpGet]
        [Route("BuscarPrimerEquipo")]
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> BuscarPrimerEquipo()
        {

            var rsp = new ResponseModel<EquipoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await this._equipoService.BuscarEquipoPricipal();

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }



        [HttpGet]
        [Route(" BuscarEquiposCantera")]
        public async Task<IActionResult> BuscarEquiposCantera([FromQuery] int temporada)
        {

            var rsp = new ResponseModel<List<EquipoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await this._equipoService.BuscarEquiposCantera();

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
