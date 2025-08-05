using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CBSANTOMERA.BLL.Servicios
{
    public class messageService : IMessageService
    {
        static readonly HttpClient client = new HttpClient();
        public async Task<messageDTO> Enviar(messageDTO modelo)

        {
            string uri = "http://localhost:8000/preguntar";
            
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(modelo);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();

            string res = await response.Content.ReadAsStringAsync();
           return Newtonsoft.Json.JsonConvert.DeserializeObject<messageDTO>(res);

        }

       
    }
}
