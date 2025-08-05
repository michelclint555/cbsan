using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

            var jwt = this.configuration.GetSection("Jwt");
            JwtDTO jwtDTO = new JwtDTO();
            jwtDTO.Issuer = jwt.GetSection("Issuer").Value;
            jwtDTO.Audience = jwt.GetSection("Audience").Value;
            jwtDTO.Subject = jwt.GetSection("Subject").Value;
            jwtDTO.Key = jwt.GetSection("Key").Value;
            //var tj = jwt.GetSection("Audience").Value;
            //encapsulamos todo lo que va a formar nuestro token

            var claims = new[] {
                    //new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                     new Claim("id",user.IdUsuario.ToString()),
                     new Claim("correo",user.Correo.ToString()),
                      new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.Sub, DateTime.UtcNow.ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtDTO.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                jwtDTO.Issuer,
                 jwtDTO.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: singIn

                );

            return new JwtSecurityTokenHandler().WriteToken(token);




          /*  var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysceret..fasdfasdfadsfasdfadsfadsfasdfasdfasdfasdfasdfsadfasdfsadfdasfsdafasdfsdafasfsdafasdfasdfasfasdffasdfasdfsadfsadfasdfasdfasdfasdfasdfasdfadsfasdfasdfasdfasdfasdfsadfasdfasdfasdfasdfasdfasdfadsf...");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.IdRol.ToString()),
                new Claim(ClaimTypes.Name,$"{user.Nombre}{user.Apellidos}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);*/
        }


        public string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = this._SessionRepository.Consultar(a => a.Token == refreshToken);
                
            if (tokenInUser != null)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        public async Task<bool> Eliminar(Session sesion)
        {
            var tokenInUser = await this._SessionRepository.Consultar(a => a.Token == sesion.Token || a.IdUsuario == sesion.IdUsuario);
            Session session = tokenInUser.FirstOrDefault();
            
            
            return await this._SessionRepository.Eliminar(session);

        }

            public async Task <Session> CrearSesion(Usuario user) {


            SesionDTO sesion = new SesionDTO();

            try {

                var tokenInUser = await this._SessionRepository.Consultar(a => a.Token == user.Token || a.IdUsuario ==  user.IdUsuario);

                if (tokenInUser != null)
                {
                    if (tokenInUser.Count() > 1)
                    {
                        foreach (var item in tokenInUser)
                        {
                            await this._SessionRepository.Eliminar(item);
                        }
                    }
                    if (tokenInUser.Count() == 1) {

                        if (user.IdUsuario == tokenInUser.First().IdUsuario) {

                            return tokenInUser.First();
                        }
                    }
                    
                }

             


                sesion.Token = this.CreateJwt(user);
                var newAccessToken = sesion.Token;
                var newRefreshToken = "refrescar";
                sesion.IdUsuario = user.IdUsuario;
                sesion.RefreshToken = newRefreshToken;
                sesion.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);

                Session ses = this._mapper.Map<Session>(sesion);


                var sesi = await this._SessionRepository.Crear(ses);
                return ses;

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
           

            
            //ses.RefreshToken = this.CreateRefreshToken();
           /* if (!await this._SessionRepository.Editar(ses)) {
                return null;        
               }*/

           

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
