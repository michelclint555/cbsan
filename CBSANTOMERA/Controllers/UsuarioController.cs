using CBSANTOMERA.BLL.Servicios;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CBSANTOMERA.Utilidad;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Claims;
using CBSANTOMERA.MODEL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CBSANTOMERA.UTILITY.Mappers;
namespace CBSANTOMERA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   // ⬅️ Todas las rutas requieren JWT
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly ISessionService _sessionService;

        public UsuarioController(IUsuarioService usuarioService, IRolService rolService, ISessionService _sessionService)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            this._sessionService = _sessionService;
        }
        

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {

            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> Roles()
        {

            var rsp = new Response<List<RolDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _rolService.Lista();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);

        }

        [HttpPost]
        [Route("IniciarSesion")]
        [AllowAnonymous]   // ⬅️ Esta acción NO requiere token
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {

            var rsp = new Response<TokenApiDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("EliminarSesion")]
        public async Task<IActionResult> EliminarSesion([FromBody] LoginDTO login)
        {

            var rsp = new Response<TokenApiDTO>();


          



            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var rToken = JwtDTO.ValidarToken(identity);

                if (!rToken.success)
                {
                    return rToken;
                }
                Usuario usuario = await _usuarioService.Ver(rToken.result);
                if (usuario.IdRol != null && usuario.IdRol != 0) {
                    Session session = new Session();
                    session.Token = identity.ToString();
                    session.IdUsuario = rToken.result;

                   rsp.status = await this._sessionService.Eliminar(session);
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
        [Route("Ver")]
        public async Task<IActionResult> Ver([FromQuery] int Usuario)
        {

            var rsp = new Response<UsuarioDTO>();

            try
            {
                rsp.status = true;

                rsp.value = await _usuarioService.Ver(Usuario);

                if (rsp.value == null) { 
                    
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
        public async Task<IActionResult> Guardar([FromForm] UsuarioDTO usuario)
        {

            var rsp = new Response<UsuarioDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Crear(usuario);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromForm] UsuarioDTO usuario)
        {

            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _usuarioService.Editar(usuario);

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
                rsp.value = await _usuarioService.Eliminar(id);

            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous] // No requiere JWT
        public async Task<IActionResult> RefreshToken([FromBody] TokenApiDTO tokenDto)
        {
            var rsp = new Response<TokenApiDTO>();

            try
            {
                var refreshToken = tokenDto.RefreshToken;

                var query = await _sessionService.Consultar(x => x.Token == refreshToken);
                var session = await query.FirstOrDefaultAsync();

                if (session == null || session.FechaExpiracion < DateTime.UtcNow || session.Usado.GetValueOrDefault())
                {
                    rsp.status = false;
                    rsp.msg = "Refresh token inválido o expirado";
                    return Ok(rsp);
                }

                // Marcar como usado
                session.Usado = true;
                await _sessionService.Editar(session);

                // Generar nuevos tokens
                var user = await _usuarioService.Ver(session.IdUsuario ?? 0);
                Usuario modelo = UsuarioMapper.ToModel(user);
                var newAccessToken = _sessionService.CreateJwt(modelo);
                var newRefreshToken = await _sessionService.CreateRefreshToken();

                // Guardar nuevo token
                await _sessionService.Crear(new Session
                {
                    Token = newRefreshToken,
                    IdUsuario = user.IdUsuario,
                    FechaCreacion = DateTime.UtcNow,
                    FechaExpiracion = DateTime.UtcNow.AddDays(7),
                    Usado = false
                });

                rsp.status = true;
                rsp.value = new TokenApiDTO()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
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
