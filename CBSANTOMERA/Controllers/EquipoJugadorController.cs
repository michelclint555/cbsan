using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
//using CBSANTOMERA.Model;
using System.Collections.Generic;

namespace CBSANTOMERA.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquipoJugadorController : ControllerBase
    {
        private readonly IEquipoJugadorService _equipoJugadorService;
        public EquipoJugadorController(IEquipoJugadorService _equipoJugadorService)
        {
            this._equipoJugadorService = _equipoJugadorService;
        }

        [HttpGet]
        [Route("Lista")] //Lista de todos los jugadores que tienen asignados un equipo o dos o tres
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<EquipoJugadorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoJugadorService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("ListaJugadores/{id:int}")] //devuelve todos los jugadores que no estan en el id de equipo introducido
        public async Task<IActionResult> ListaTodosJugadores(int id)
        {

            var rsp = new Response<List<EquipoJugadorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoJugadorService.ListaJugadoresEquipoNoEstan(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }


        [HttpGet]
        [Route("ListaEquipoJugadores/{id:int}")] // devuelve todos los jugadores de un equipo en concreto
        public async Task<IActionResult> ListaUnEquipoJugadores(int id)
        {

            var rsp = new Response<List<EquipoJugadorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _equipoJugadorService.ListaJugadoresUnEquipo(id);
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
        public async Task<IActionResult> Editar([FromForm] EquipoJugadorDTO equipoJugador)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _equipoJugadorService.Editar(equipoJugador);

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

                rsp.value = await _equipoJugadorService.ADDUpdateDeleteJugadoresEquipo(equipo);

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
