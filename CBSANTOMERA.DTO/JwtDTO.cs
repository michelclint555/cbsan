using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.DTO
{
    public class JwtDTO
    {
        public String Key { get; set; }
       
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public String Subject { get; set; }


        public static dynamic ValidarToken(ClaimsIdentity identity) {
                try {
                        if (identity.Claims.Count() == 0) {
                            return new {

                                            success = false,
                                            message = "Verificar si estas enviando un token vacío",
                                            result = ""
                                      };
                        }

                        var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value; //quiero que me obtenga el valor id que eespecificamos a la hora de crear el token
                return new
                {

                    success = false,
                    message = "Exito",
                    result = id,
                };

            }
                catch (Exception ex) { 
            
                        return new {

                            success = false,
                            message = ex.Message,
                            result = "",
                       };
    }
             
            }
    }


}
