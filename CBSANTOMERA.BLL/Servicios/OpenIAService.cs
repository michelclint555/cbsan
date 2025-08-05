using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using OllamaSharp;
//using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CBSANTOMERA.BLL.Servicios
{
    public class OpenIAService : IOpenIAService
    {


        public OpenIAService()
        {
            
        }

        public async Task <NoticiasDTO> Preguntar(string pregunta)
        {

            
            string res = ""; //
            var uri = new Uri("http://localhost:11434"); //direccion de la Api de ollama
            var ollama = new OllamaApiClient(uri);
            ollama.SelectedModel = "mistral"; //modelo LLM
            var chat = new Chat(ollama);
            var message =  "En castellano, con el siguiente modelado de dato de noticia {titulo: string, subtitulo: string, contenido: string} crea una noticia sobre: "+pregunta  ; //mensaje para crear la noticia
            await foreach (var answerToken in chat.SendAsync(message)) res += answerToken; //enviamos la pregunta y esperamos a la respuesta que nos proporcione
            NoticiasDTO noticias = new NoticiasDTO(); //creamos un objeto noticia
            //realizamos un tratamiento datos para dar formato a la respuesta recibida.
            var regex = new Regex(
            @"Titulo:\s*(.*?)Subtitulo:\s*(.*?)Contenido:\s*(.*)",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Match match = regex.Match(res);

            if (match.Success)
            {
                var noticia = new Noticia
                {
                    Titulo = match.Groups[1].Value.Trim(),
                    Subtitulo = match.Groups[2].Value.Trim(),
                    Contenido = match.Groups[3].Value.Trim()
                };
                noticias.Titulo = noticia.Titulo;
                noticias.Subtitulo = noticia.Subtitulo;
                noticias.Contenido = noticia.Contenido;
                
            }
            else
            {
                
            }

            return noticias;
            
        }
    }
}
