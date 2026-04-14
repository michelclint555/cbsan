using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class SessionService : ISessionService
    {

        public readonly IMapper _mapper;
        private readonly IGenericRepository<Session> _SessionRepository;
        public readonly IConfiguration configuration;
        public SessionService(  IGenericRepository<Session> _SessionRepository, IMapper _mapper, IConfiguration configuration) {
           
            this._SessionRepository = _SessionRepository;
            this._mapper = _mapper;
            this.configuration = configuration;
        }

        public string CreateJwt(Usuario user)
        {
            var jwt = configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim("id", user.IdUsuario.ToString()),
        new Claim("correo", user.Correo),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Correo), // Subject correcto
    };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<string> CreateRefreshToken()
        {
            string refreshToken;
            bool exists;

            do
            {
                refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                var query = await _SessionRepository.Consultar(a => a.Token == refreshToken);

                exists = await query.AnyAsync();   // ⬅️ Aquí sí funciona

            } while (exists);

            return refreshToken;
        }



        public async Task<bool> Eliminar(Session sesion)
        {
            var tokenInUser = await this._SessionRepository.Consultar(a => a.Token == sesion.Token || a.IdUsuario == sesion.IdUsuario);
            Session session = tokenInUser.FirstOrDefault();
            
            
            return await this._SessionRepository.Eliminar(session);

        }

           
            public async Task<Session> CrearSesion(Usuario user)
            {
                var sesionesExistentes = (await _SessionRepository
                    .Consultar(s => s.IdUsuario == user.IdUsuario))
                    .ToList();

                // Si existe una sesión válida, reutilizarla
                var sesionValida = sesionesExistentes
                    .FirstOrDefault(s => s.FechaExpiracion > DateTime.UtcNow);

                if (sesionValida != null)
                    return sesionValida;

                // Eliminar sesiones antiguas
                foreach (var sesion in sesionesExistentes)
                    await _SessionRepository.Eliminar(sesion);

                // Crear nueva sesión
                var nuevaSesionDto = new SesionDTO
                {
                    Token = CreateJwt(user),
                    RefreshToken = CreateRefreshToken().Result,
                    IdUsuario = user.IdUsuario,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5),
                    FechaExpiracion = DateTime.UtcNow.AddMinutes(60),
                   
                };

                var nuevaSesion = _mapper.Map<Session>(nuevaSesionDto);

                return await _SessionRepository.Crear(nuevaSesion);
            }

            public async Task<Session> Crear(Session entity)
        {
            return await _SessionRepository.Crear(entity);
        }

        public async Task<bool> Editar(Session entity)
        {
            return await _SessionRepository.Editar(entity);
        }

        public async Task<IQueryable<Session>> Consultar(Expression<Func<Session, bool>> filtro = null)
        {
            return await _SessionRepository.Consultar(filtro);
        }

        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("veryverysceret.....");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;

        }


    }
}
