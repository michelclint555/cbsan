using AutoMapper;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using CBSANTOMERA.BLL.Servicios.Contrato;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace CBSANTOMERA.BLL.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IArchivosService _ArchivoService;
        public readonly IGenericRepository<Usuario> _genericRepository;
        public readonly ISessionService _sersionRepository;
        public readonly IMapper _mapper;
        public readonly PasswordHasher hasher;
        public readonly IRolService _rolService;
        private string rutaServidor = @"wwwroot\archivos\Usuarios\";
        private string carpetaLocal = "Usuarios";
        private readonly string mirutajugadores = @"Jugadores\";

        public UsuarioService(IGenericRepository<Usuario> genericRepository, IMapper mapper,ISessionService _sersionRepository, IRolService _rolService, IArchivosService archivoService)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            this._sersionRepository = _sersionRepository;
            this._rolService = _rolService;
            _ArchivoService = archivoService;
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {

                if (modelo.imagen == null)
                {

                  

                    
                        modelo.Foto = "default_profile.png";
                      
                    

                }
                //Comprueba el uusario
                if (await CheckEmailExisteAsync(modelo.Correo) != false) {
                    throw new TaskCanceledException("No se pudo crear el usuario, El usuario ya existe");
                }

                string clave = "Junio.2024";
                   var passMessage = CheckPasswordStrength(clave);

                if (!string.IsNullOrEmpty(passMessage))
                {
                    throw new TaskCanceledException(passMessage);
                }
                //var queryUsuario = await _genericRepository.Consultar(u => u.IdUsuario == modelo.IdUsuario);
                
                modelo.Clave = PasswordHasher.HashPassword(password: clave);
                
                modelo.Token = "";
                //modelo.IdRol = 1;
                modelo.FechaRegistro = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.ContrasenaActivada = false;
                var usuarioCreado = await _genericRepository.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el usuario");
                }

                string filename = " ";
                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = usuarioCreado.ToString() + DateTime.Now;
                    string name = hasha.GetHashCode().ToString();


                    filename = name + extension;

                    usuarioCreado.Foto = filename;
                  //  jugadorCreado.ThumbnailImageSrc = filename;



                    

                    try
                    {
                        if (!Directory.Exists(rutaServidor))
                        {
                            Directory.CreateDirectory(rutaServidor);
                        }

                       

                       

                        

                        //rutaImagen = Path.Combine(rutaServidor, rutaImagen);


                    }
                    catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                    try
                    {
                        AccionFile accion = new AccionFile();

                        accion.accion = "Guardar";
                        accion.file = modelo.imagen;
                        accion.thumbnail = false;
                        accion.rutaDesten = rutaServidor + usuarioCreado.Foto;
                        //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                        // accion.sizeThumbnail = 120;
                        this._ArchivoService.Ejecutar(accion);



                    }
                    catch (Exception e)
                    {
                        throw new TaskCanceledException("No se pudo Crear el archivo");
                    }


                }
                usuarioCreado.IdRol = modelo.Rol.Id;
               
                await this._genericRepository.Editar(usuarioCreado);

                return _mapper.Map<UsuarioDTO>(usuarioCreado);
            }
            catch
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Ver(int id)
        {
            try
            {


                var usuarioEncontrado = await _genericRepository.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }


                UsuarioDTO usuario =  _mapper.Map<UsuarioDTO>(usuarioEncontrado);
                usuario.Rol = await this._rolService.Ver(usuarioEncontrado.IdRol);
                return usuario;
            }
            catch (Exception ex) {
                throw new TaskCanceledException(ex.Message);
            }


    


        }
            public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _genericRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }


                if (usuarioEncontrado.ContrasenaActivada== true && modelo.ContrasenaActivada == false) {
                    var res = await this.ResetPassword(usuarioEncontrado.IdUsuario, "Junio.2024");

                    if (!res)
                    {

                        throw new TaskCanceledException("Ha habido un error la cambiar de estado el usuario.");
                    }
                    else {
                        usuarioEncontrado.ContrasenaActivada = modelo.ContrasenaActivada;

                    }
                }

                

                var filename = "";


                if (modelo.imagen != null)
                {
                    string extension = Path.GetExtension(modelo.imagen.FileName);

                    string hasha = usuarioEncontrado.ToString() + DateTime.Now;
                    string name = hasha.GetHashCode().ToString();
                    filename = name + extension;
                    var filepath = Path.Combine(this.mirutajugadores, filename);
                    if (usuarioEncontrado.Foto == "default_profile.png")//si el jugador tiene la foto predetermianada segun el sexo para crear la imagen porque hay una sola imagen predeterminada para todos
                    {

                        modelo.Foto = filename;


                        string rutaImagen = rutaServidor;

                        try
                        {
                            if (!Directory.Exists(rutaServidor))
                            {
                                Directory.CreateDirectory(rutaServidor);
                            }






                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del jugador."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;
                            accion.rutaDesten = rutaImagen + modelo.Foto;
                            accion.rutaOrigen = rutaImagen + modelo.Foto;
                            //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                            // accion.sizeThumbnail = 120;
                            this._ArchivoService.Ejecutar(accion);



                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }
                    }
                    else
                    {

                        modelo.Foto = filename;
                        //jugadorEncontrado.Foto = filename;

                        string rutaImagen = this.mirutajugadores;


                        try
                        {
                            if (!Directory.Exists(rutaServidor))
                            {
                                Directory.CreateDirectory(rutaServidor);
                            }



                            rutaImagen = rutaServidor;


                        }
                        catch (Exception ex) { throw new Exception("Ha habido un error al crear la estructura de fichero del usuario."); }


                        try
                        {
                            AccionFile accion = new AccionFile();

                            accion.accion = "Actualizar";
                            accion.file = modelo.imagen;
                            accion.thumbnail = false; ;
                            accion.rutaDesten = rutaImagen + modelo.Foto;
                            accion.rutaOrigen = rutaImagen + usuarioEncontrado.Foto;
                            //accion.rutaDestenThumbnail = rutaImagen + jugadorCreado.Foto;
                            // accion.sizeThumbnail = 120;
                            this._ArchivoService.Ejecutar(accion);



                        }
                        catch (Exception e)
                        {
                            throw new TaskCanceledException("No se pudo Crear el archivo");
                        }

                    }
                    usuarioEncontrado.Nombre = usuarioModelo.Nombre;
                    usuarioEncontrado.Apellidos = usuarioModelo.Apellidos;
                    usuarioEncontrado.IdRol = modelo.Rol.Id;
                    usuarioEncontrado.Correo = usuarioModelo.Correo;
                    //usuarioEncontrado.Clave = usuarioModelo.Clave;
                    usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
                    //usuarioEncontrado.ContrasenaActivada = usuarioModelo.ContrasenaActivada;
                    usuarioEncontrado.FechaModificacion = DateTime.Now;
                    //usuarioEncontrado.Foto = modelo.Foto;
                    bool respuesta = await _genericRepository.Editar(usuarioEncontrado);

                    if (!respuesta)
                    {

                        throw new TaskCanceledException("No se pudo editar el usuario");

                    }
                    else { return respuesta; }

                    return respuesta;

                }
                else {
                    usuarioEncontrado.Nombre = usuarioModelo.Nombre;
                    usuarioEncontrado.Apellidos = usuarioModelo.Apellidos;
                    usuarioEncontrado.IdRol = modelo.Rol.Id;
                    usuarioEncontrado.Correo = usuarioModelo.Correo;
                    //usuarioEncontrado.Clave = usuarioModelo.Clave;
                    usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;
                    //usuarioEncontrado.ContrasenaActivada = usuarioModelo.ContrasenaActivada;
                    usuarioEncontrado.FechaModificacion = DateTime.Now;
                    //usuarioEncontrado.Foto = modelo.Foto;
                    bool respuesta = await _genericRepository.Editar(usuarioEncontrado);

                    if (!respuesta)
                    {

                        throw new TaskCanceledException("No se pudo editar el usuario");

                    }
                    else { return respuesta; }

                    return respuesta;

                }
                return false;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ResetPassword(int id, string contraseña) {

            //si el usuario existe
            try {


                var usuarioEncontrado = await _genericRepository.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                string clave = contraseña;
                var passMessage = CheckPasswordStrength(clave);

                if (!string.IsNullOrEmpty(passMessage))
                {
                    throw new TaskCanceledException(passMessage);
                }
                //var queryUsuario = await _genericRepository.Consultar(u => u.IdUsuario == modelo.IdUsuario);

                usuarioEncontrado.Clave = PasswordHasher.HashPassword(password: clave);

                usuarioEncontrado.ContrasenaActivada = false;

                bool respuesta = await _genericRepository.Editar(usuarioEncontrado);

                if (!respuesta)
                {

                    throw new TaskCanceledException("No se pudo editar el usuario");

                }
                else { return respuesta; }



            } catch (Exception ex) { }
            


            return true;
        }



        public async Task<bool> Eliminar(int id)
        {
            try
            {
                
                var usuarioEncontrado = await _genericRepository.Obtener(u => u.IdUsuario == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

              
                bool respuesta = await _genericRepository.Eliminar(usuarioEncontrado);

                if (!respuesta)
                {

                    throw new TaskCanceledException("No se pudo eliminar el usuario");

                }
                else {

                    AccionFile accion = new AccionFile();
                    accion.accion = "Eliminar";
                    accion.rutaOrigen = this.rutaServidor + usuarioEncontrado.Foto;
                    accion.thumbnail = false;
                    this._ArchivoService.Ejecutar(accion);
                    return respuesta; 
                
                }


            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _genericRepository.Consultar();
                var listaUsuario = queryUsuario.ToList();
                List < UsuarioDTO> lista=  _mapper.Map<List<UsuarioDTO>>(listaUsuario);
                for (int i= 0; i < listaUsuario.Count; i++)
                {
                    lista[i].Rol = await this._rolService.Ver(listaUsuario[i].IdRol);
                }
                return lista;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TokenApiDTO> ValidarCredenciales(string correo, string clave)
        {
            try
            {


                
                var queryUsuario = await _genericRepository.Consultar(u=> u.Correo == correo );

                if (queryUsuario.FirstOrDefault() == null) {
                    throw new TaskCanceledException("Credenciales incorrectas");
                }


               
               /* if (!PasswordHasher.VerifyPassword(clave, queryUsuario.FirstOrDefault().Clave))
                {
                    throw new TaskCanceledException("Credenciales incorrectas");
                }*/

              



                //Usuario devolverUsuario = queryUsuario.Include(Rol => Rol.IdRol).First();
                Usuario devolverUsuario = queryUsuario.FirstOrDefault();

             

                SesionDTO sesion = new SesionDTO();
                    Session ses= await this._sersionRepository.CrearSesion(devolverUsuario);
                // cerrar todas las sesiones de ese usuario
                /*await _sersionRepository.*/

                if (ses== null) {
                    return null;
                }


                return
                    new TokenApiDTO
                    {
                        AccessToken = ses.Token,
                        RefreshToken = ses.RefreshToken
                    };


              
            }
            catch
            {
                throw;
            }
        }


        private async Task<bool> CheckEmailExisteAsync(string email) {

           
                var queryUsuario = await _genericRepository.Consultar(u => u.Correo == email);

                if (queryUsuario.FirstOrDefault() == null)
                {
                    
                    return false;
                }
                else {
                    return true;
                }

        }


        private string CheckPasswordStrength(string clave) { 
        
                StringBuilder sb = new StringBuilder();

            if (clave.Length < 9) { 
                sb.Append("El tamaño mínimo de la contraseña debe ser de 8 "+ Environment.NewLine);
            }

           /* if (!(Regex.IsMatch(clave, "[a-z]")&& Regex.IsMatch(clave, "[A-Z]")&& Regex.IsMatch(clave, "[0-9]"))) { 
                    sb.Append("La contraseña debe de ser alfanumérico"+Environment.NewLine);
            }*/

            if (!Regex.IsMatch(clave, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]")) {
                sb.Append("La contraseña debe de tener un caracter especial" + Environment.NewLine);
            }

            return sb.ToString();

        }
    }
}
