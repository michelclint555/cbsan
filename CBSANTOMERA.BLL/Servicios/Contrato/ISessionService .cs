using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ISessionService
    {
        public string CreateJwt(Usuario user);
        public Task<string> CreateRefreshToken();
        Task<Session> CrearSesion(Usuario user);
        Task<bool> Eliminar(Session sesion);
        Task<Session> Crear(Session entity);
        Task<bool> Editar(Session entity);
        Task<IQueryable<Session>> Consultar(Expression<Func<Session, bool>> filtro = null);



    }
}
