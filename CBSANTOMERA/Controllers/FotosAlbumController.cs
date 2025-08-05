using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
namespace CBSANTOMERA.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FotosAlbumController : ControllerBase
    {

        private readonly IFotoAlbumService _FotosAlbumService;

        public FotosAlbumController(IFotoAlbumService _albumService)
        {
            _FotosAlbumService = _albumService;
        }


    }
}
