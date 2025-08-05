using CBSANTOMERA.BLL.Servicios;
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
    public class CompeticionController : ControllerBase
    {
        private readonly ICompeticionService _Service;
        private readonly IEquipoCompeticionService _EquipoService;
        private readonly IFaseCompeticionService _faseCompeticionService ;
        private readonly ILigaService _ligaService ;
        private readonly string rutaServidor;
        public CompeticionController(ICompeticionService Service, IConfiguration configuration, IEquipoCompeticionService equipoService, IFaseCompeticionService faseCompeticionService, ILigaService _ligaService)
        {
            _Service = Service;
            _EquipoService = equipoService;
            _faseCompeticionService = faseCompeticionService;
            this._ligaService = _ligaService;
            //rutaServidor = @"wwwroot\archivos\Jugadores\";
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<CompeticionDTO>>();
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
        [Route("Ver-Liga")]
        public async Task<IActionResult> VerLiga([FromQuery] int competicion)
        {

            var rsp = new Response<List<LigaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await this._ligaService.VerLiga(competicion);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("Lista_Equipos")]
        public async Task<IActionResult> ListaEquipos([FromQuery] int idCompeticion)
        {

            var rsp = new Response<List<EquipoCompeticionDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await this._EquipoService.Listar(idCompeticion);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("Lista_Todos_Equipos")]
        public async Task<IActionResult> ListaEquipos([FromQuery] int idCompeticion, int idClub, int idCategoria)
        {

            var rsp = new Response<List<EquipoCompeticionDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await this._EquipoService.Listar(idCategoria,idClub,idCompeticion);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        //cambiar a post y pasar el objeto competicion
        [HttpGet]
        [Route("Configurar")]
        public async Task<IActionResult> Configurar([FromQuery] int idCompeticion)
        {

            var rsp = new Response<List<List<JornadaDTOSimple>>>();
            try
            {
                CompeticionDTO competicion = await this._Service.VerCompeticion(idCompeticion);
                rsp.status = true;
                rsp.value = await this._Service.IniciarCompeticion(competicion); 
                
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("Jornadas")]
        public async Task<IActionResult> Lista_Jornadas([FromQuery] int competicion)
        {

            var rsp = new Response<List<List<JornadaDTOSimple>>>();
            try
            {
                CompeticionDTO competicion0 = await this._Service.VerCompeticion(competicion);
                if (competicion0.Estado == "Creado")
                {

                    rsp.status = false;
                    rsp.value = null;
                    rsp.msg = "El estado de la competicion es " + competicion0.Estado + ", no permite crear inicializar la competicion.";
                }

                if (competicion0.Estado == "Configurado") {
                    var Jornadas = await this._ligaService.ListaJornadas(competicion0);
                    var liga = await this._ligaService.VerLiga(competicion0.Id);

                    if (liga.Count == competicion0.NumEquipos)
                    {

                        if (competicion0.IdTipo == 1)
                        {
                            if (Jornadas.Count == (competicion0.NumEquipos-1) * 2)
                            {
                                competicion0.Estado = "Preparado";
                                await this._Service.EditarCompeticion(competicion0);
                                rsp.value= await this._ligaService.ListaJornadas(competicion0);
                                rsp.status = true;
                                return Ok(rsp);
                            }
                            else {

                                rsp.value = await this._Service.IniciarCompeticion(competicion0);
                                rsp.status = true;
                                rsp.status = true;
                                return Ok(rsp);
                            }

                        }

                        if (competicion0.IdTipo == 2)
                        {
                            if (Jornadas.Count == competicion0.NumEquipos)
                            {
                                competicion0.Estado = "Preparado";
                                await this._Service.EditarCompeticion(competicion0);
                                rsp.value = await this._ligaService.ListaJornadas(competicion0);
                                rsp.status = true;
                                return Ok(rsp);
                            }
                            else {
                                rsp.value = await this._Service.IniciarCompeticion(competicion0);
                                rsp.status = true;
                                return Ok(rsp);
                            }

                        }
                    }
                    else {
                        rsp.value = await this._Service.IniciarCompeticion(competicion0);
                        rsp.status = true;
                    }
                       
                }
                if (competicion0.Estado == "Preparado" || competicion0.Estado == "Finalizado" || competicion0.Estado == "Iniciado")
                {
                    rsp.value = await this._ligaService.ListaJornadas(competicion0);
                    rsp.status = true;

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
        public async Task<IActionResult> Guardar([FromForm] CompeticionDTO modelo)
        {

            var rsp = new Response<CompeticionDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _Service.CrearCompeticion(modelo);
                rsp.msg = "Se ha creado la competición correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("Guardar_Equipo")]
        public async Task<IActionResult> Guardar([FromForm] EquipoCompeticionDTO modelo)
        {

            var rsp = new Response<EquipoCompeticionDTO>();

            try
            {

                var competicion = await this._Service.VerCompeticion(modelo.Competicion);
                if (competicion.Estado == "Iniciado" || competicion.Estado == "Finalizado")
                {
                    rsp.status = false;
                    rsp.msg = "El estado de la competición no permite añadir nuevos equipos.";


                }

                if (competicion.Estado == "Creado" || competicion.Estado == "Configurado" || competicion.Estado == "Preparado")
                {
                    rsp.status = await _EquipoService.Crear(modelo);

                    rsp.value = null;

                }

                if (rsp.status == true)
                {


                    var lista = await this._EquipoService.Listar(modelo.Competicion);

                    if (lista.Count() < 3)
                    {

                        competicion.Estado = "Creado";
                        competicion.NumEquipos = lista.Count();
                        rsp.status = await this._Service.EditarCompeticion(competicion);

                    }
                    else
                    {

                        competicion.Estado = "Configurado";
                        competicion.NumEquipos = lista.Count();
                        rsp.status = await this._Service.EditarCompeticion(competicion);
                    }



                }

                

                

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("Eliminar_Equipo")]
        public async Task<IActionResult> Eliminar([FromQuery]int idEquipo, int idCompeticion)
        {

           var rsp = new Response<EquipoCompeticionDTO>();
            try
            {

                var competicion = await this._Service.VerCompeticion(idCompeticion);

                if (competicion.Estado == "Iniciado" || competicion.Estado == "Finalizado" )
                {
                    rsp.status = false;
                    rsp.msg = "El estado de la competición no permite eliminar aequipos.";


                }
                if (competicion.Estado == "Creado" || competicion.Estado == "Configurado" || competicion.Estado == "Preparado") {
                    rsp.status = await _EquipoService.Eliminar(idEquipo, idCompeticion);
                    
                    rsp.value = null;

                }
                

                if (rsp.status == true)
                {
                    

                    var lista = await this._EquipoService.Listar(idCompeticion);

                    if (lista.Count() < 3)
                    {

                        competicion.Estado = "Creado";
                        competicion.NumEquipos = lista.Count();
                        rsp.status = await this._Service.EditarCompeticion(competicion);

                    }
                    else {

                        competicion.Estado = "Configurado";
                        competicion.NumEquipos = lista.Count();
                       //eliminar todas las jornadas
                        rsp.status = await this._Service.EditarCompeticion(competicion);

                    }



                }

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar-Partido")]
        public async Task<IActionResult> Editar([FromForm] JornadaDTOSimple modelo)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _ligaService.Editar( modelo);

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
        public async Task<IActionResult> Editar([FromForm] CompeticionDTO modelo)
        {

            var rsp = new Response<bool>();

            try
            {


                rsp.value = await _Service.EditarCompeticion(modelo);
                if (rsp.value == null)
                {
                    rsp.status = false;
                }
                else {
                    rsp.status = true;
                }



             
               
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
                rsp.value = await _Service.EliminarCompeticion(id);

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

            var rsp = new ResponseModel<CompeticionDTO>();

            try
            {

              
                rsp.value = await _Service.VerCompeticion(id);

                if (rsp.value.Estado == "Creado" ) //Se crea la competicion 
                {
                    rsp.status = true;

                }

                if (rsp.value.Estado == "Configurado")// una vez añadido los equipos
                {
                    if (rsp.value.NumEquipos < 3) //Se crea la competicion 
                    {
                        rsp.msg = "El número de equipos mínimo es de 3.";
                        rsp.status = false;
                    }
                    else
                    {
                        rsp.status = true;
                        //generar las jornadas
                    }
                }

                if (rsp.value.Estado == "Preparado")//Una vez inicializadas y creadas las jornadas
                {
                    //obtener las jornadas y la clasificacion desde la bbdd
                    rsp.status = true;
                    return Ok(rsp);
                }

                if (rsp.value.Estado == "Iniciado") //una vez que la fecha de una jornada haya pasado de fecha
                {
                    //obtener las jornadas y la clasificacion desde la bbdd
                }

                if (rsp.value.Estado == "Finalizado") //una vez se haya jugado todos los partidos
                {
                    //obtener las jornadas y la clasificacion desde la bbdd
                }


                if (rsp.value == null)
                {

                    rsp.status = false;
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
