using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Mvc;
using CBSANTOMERA.Utilidad;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.API.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<AlbumDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _albumService.Lista();
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
        public async Task<IActionResult> Guardar([FromForm] AlbumDTO album)
        {

            var rsp = new Response<AlbumDTO>();



            try
            {





                rsp.status = true;



                rsp.value = await _albumService.Crear(album);
                rsp.msg = "El Album " + rsp.value.Nombre + " se creado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("VerAlbum/{id:int}")]
        public async Task<IActionResult> VerAlbum(int id)
        {

            var rsp = new Response<AlbumDTO>();

            try
            {
                rsp.status = true;
                rsp.value =  await _albumService.VerAlbum(id);
                rsp.msg = "El album se ha cargado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("EliminarAlbum/{id:int}")]
        public async Task<IActionResult> EliminarAlbum(int id)
        {

            var rsp = new Response<AlbumDTO>();

            try
            {
                
                rsp.status = await _albumService.EliminarAlbum(id);

                if (rsp.status == true)
                {
                    rsp.msg = "El album se ha eliminado correctamente";
                }
                else {
                    rsp.msg = "Ha habido un error al eliminar el album";
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
        public async Task<IActionResult> Update([FromForm] AlbumDTO album)
        {

            var rsp = new Response<AlbumDTO>();



            try
            {


                rsp.status = await _albumService.Editar(album);
                rsp.value = await _albumService.VerAlbum(album.IdAlbum);
                rsp.msg = "El Album " + rsp.value.Nombre + " se creado correctamente";

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet]
        [Route("dimension/{id:int}")]
        public async Task<IActionResult> dimension(int id)
        {

            var rsp = new Response<AlbumDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _albumService.VerAlbum(id);
               this._albumService.Dimension(rsp.value);

                rsp.msg = "El album se ha cargado correctamente";

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
