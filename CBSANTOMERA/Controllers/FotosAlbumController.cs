using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
using Microsoft.AspNetCore.Authorization;
namespace CBSANTOMERA.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class FotosAlbumController : ControllerBase
    {

        private readonly IFotoAlbumService _FotosAlbumService;

        public FotosAlbumController(IFotoAlbumService _albumService)
        {
            _FotosAlbumService = _albumService;
        }


    }
}
