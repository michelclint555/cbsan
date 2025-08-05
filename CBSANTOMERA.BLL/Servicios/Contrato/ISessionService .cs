using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ISessionService
    {
        public string CreateJwt(Usuario user);
        public string CreateRefreshToken();
        Task<Session> CrearSesion(Usuario user);
        Task<bool> Eliminar(Session sesion);



    }
}
