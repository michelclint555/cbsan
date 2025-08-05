using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CBSANTOMERA.BLL.Servicios
{
    public class LigaService : ILigaService
    {
        private readonly IGenericRepository<Liga> _Repository;
        private readonly IGenericRepository<Jornada> _JornadaRepository;
        private readonly IEquipoService _EquipoService;
        private readonly ITemporadaService _TemporadaService;
        private readonly IEquipoCompeticionService _EquipoCompeticionRepository;
        private readonly IFaseCompeticionService _FaseCompeticionService;
        private readonly IClubService _ClubService;
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = @"Competiciones\";
        
        public LigaService(IGenericRepository<Liga> repository, IGenericRepository<Jornada> jornadaRepository, IEquipoService equipoService, ITemporadaService _TemporadaService, IEquipoCompeticionService equipoCompeticionRepository,IFaseCompeticionService _FaseCompeticionService, IClubService _ClubService)
        {
            _Repository = repository;
            _JornadaRepository = jornadaRepository;
            _EquipoService = equipoService;
            this._TemporadaService = _TemporadaService;
            _EquipoCompeticionRepository = equipoCompeticionRepository;
             this._FaseCompeticionService= _FaseCompeticionService;
            this._ClubService = _ClubService;
    }

        public async Task<LigaDTO> Crear(LigaDTO modelo)
        {
            try
            {
                //comprobar si el equipo existe, si existe si esta en una competicion
                Liga t = modelo.ToModel();
                //modelo.Equipo = await this._EquipoRepository.BuscarEquipo(modelo.Equipo.IdEquipo);
                if (modelo.Equipo == null)
                {
                    throw new Exception("EL equipo no existe");
                }

                var liga = await this._Repository.Crear(t);
                if (liga == null)
                {
                    throw new Exception("No se ha podido crear la liga");
                }
                modelo.Id = liga.Id;
                return modelo;
            }
            catch (Exception e) { throw; }
        }


        public async Task<JornadaDTO> Crear(JornadaDTO modelo)
        {
            try
            {
                TemporadaDTOSmall temporada = await this._TemporadaService.TemporadaActiva();
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.Temporada = new TemporadaDTO();
                modelo.Temporada.Id = temporada.Id;
                
                Jornada t = modelo.ToModel();
                t.Temporada = temporada.Id;
                t.Estado = "Creado";
                t.Ubicacion = "Pabellon";
                var jornada = await this._JornadaRepository.Crear(t);
                if (jornada == null)
                {
                    throw new Exception("No se ha podido crear la jornada");
                }
                modelo.Id = jornada.Id;
                return modelo;
            }
            catch (Exception e) { throw; }
        }


        /// <summary>
        /// A partir de un PDf de calendario de la FBRM obtenemos todas las jornadas, competiciones sin iniciar
        /// </summary>
        /// <param name="equipos"></param>
        /// <param name="competicion"></param>
        /// <param name="fase"></param>
        /// <returns></returns>
        public async Task<List<List<JornadaDTOSimple>>> InicializarJornadasCalendarioPDF(List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase)
        {
            TemporadaDTOSmall temporada = await this._TemporadaService.TemporadaActiva();
            rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal,competicion.Id.ToString(), "calendario.pdf");


            List<List<JornadaDTO>> jornadas = this.ExtraerCalendarioInicial(rutaServidor, equipos, competicion, fase);

            List<List<JornadaDTOSimple>> jornads_SImple = new List<List<JornadaDTOSimple>>();
            //Para calendario ya iniciado, comprobar si a medio o ya finalizado
            ///List<List<JornadaDTO>> jornadas = ExtraerPartidosDesdePdf0(rutaServidor, equipos, competicion, fase);
            ///
            ///
            foreach (var item in jornadas)
            {
                List<JornadaDTOSimple> lista = new List<JornadaDTOSimple>();
                foreach (JornadaDTO subitem in item)
                {
                    subitem.FechaCreacion = DateTime.Now;
                    subitem.FechaModificacion = DateTime.Now;
                    subitem.Estado = "Creado";
                    subitem.Fase = fase;
                    subitem.Competicion = competicion;
                    await this.Crear(subitem);
                    JornadaDTOSimple jornada = new JornadaDTOSimple();
                    jornada.Jornada1 = subitem.Jornada1;
                    jornada.Local = new EquipoDTOSimple();
                    jornada.Local.Nombre = subitem.Local.Nombre;
                    jornada.Local.IdClub = subitem.Local.IdClub;
                    ClubDTO club = await this._ClubService.VerClub(subitem.Local.IdClub);
                    jornada.Local.logo = club.Foto;
                    jornada.Visitante = new EquipoDTOSimple();
                    jornada.Visitante.Nombre = subitem.Visitante.Nombre;
                    club = await this._ClubService.VerClub(subitem.Visitante.IdClub);
                    jornada.Visitante.logo = club.Foto;
                    jornada.Visitante.IdClub = subitem.Visitante.IdClub;
                    jornada.Fecha = subitem.Fecha;
                    jornada.Ubicacion = subitem.Ubicacion;

                    lista.Add(jornada);

                }
                jornads_SImple.Add(lista);
            }




            return jornads_SImple;

        }

            public async Task<List<List<JornadaDTOSimple>>> InicializarJornadas(List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase)
        {
            try
            {

               TemporadaDTOSmall temporada = await this._TemporadaService.TemporadaActiva();
                rutaServidor = Path.Combine(rutaServidor, temporada.Nombre, this.carpetaLocal, "calendario.pdf");


                List<List<JornadaDTO>> jornadas =new  List<List<JornadaDTO>>();

                List <List<JornadaDTOSimple>> jornads_SImple = new List<List<JornadaDTOSimple>>();
                //Para calendario ya iniciado, comprobar si a medio o ya finalizado
                ///List<List<JornadaDTO>> jornadas = ExtraerPartidosDesdePdf0(rutaServidor, equipos, competicion, fase);
                ///
                ///
                




                //return jornads_SImple;


                //jornadas.Add(jorn);
                // Si número impar, agregamos un equipo "ficticio"
                var tieneBye = false;
                var numeroEquipos = equipos.Count;
                EquipoCompeticionDTO e0 = new EquipoCompeticionDTO();
                if (numeroEquipos % 2 != 0)
                {
                    
                    EquipoCompeticionDTO[] equipos0 = new EquipoCompeticionDTO[6];
                    equipos.Add(e0);
                    equipos[numeroEquipos].Incluido = true;
                    equipos[numeroEquipos].Equipo =new EquipoDTO();
                    equipos[numeroEquipos].Competicion = equipos[0].Competicion;
                    equipos[numeroEquipos].Equipo.Nombre = "Descansa";
                    equipos[numeroEquipos].Equipo.IdEquipo = 0;
                    equipos[numeroEquipos].Equipo.IdClub = 0;

                    equipos.CopyTo(equipos0, 0);
                    
                    numeroEquipos++;
                    tieneBye = true;
                }
                int totalJornadas = numeroEquipos - 1;
                int partidosPorJornada = numeroEquipos / 2;
                int[] equiposEntero = Enumerable.Range(1, numeroEquipos).ToArray();
                for (int jornada = 0; jornada < totalJornadas; jornada++)
                {
                    List<JornadaDTO> jorn = new List<JornadaDTO>();
                    for (int i = 0; i < partidosPorJornada; i++)
                    {
                        int local = equiposEntero[i];
                        int visitante = equiposEntero[numeroEquipos - 1 - i];

                        //if (!tieneBye)
                        //{
                            JornadaDTO jornada1 = new JornadaDTO();
                            jornada1.Jornada1 = jornada + 1;
                            jornada1.Local = equipos[local-1].Equipo;
                            var t = equiposEntero[numeroEquipos - 1 - i];
                            jornada1.Visitante = equipos[t-1].Equipo;
                            jornada1.Ubicacion = "Ubicacion del partido";
                            jornada1.Fecha = DateTime.Now;
                            jornada1.Fase = fase;
                        jornada1.Competicion = competicion;
                        jorn.Add(jornada1);
                         await this.Crear(jornada1);
                       // }
                       /* else {
                            if (local != numeroEquipos && visitante != numeroEquipos) // Ignorar BYE
                            {

                                JornadaDTO jornada1 = new JornadaDTO();
                                jornada1.Jornada1 = jornada + 1;
                                jornada1.Local = equipos[local].Equipo;
                                jornada1.Visitante = equipos[visitante].Equipo;
                                jornada1.Ubicacion = "Ubicacion del partido";
                                jornada1.Fecha = DateTime.Now;
                                jorn.Add(jornada1);

                            }
                        }*/
                        
                        
                    }
                    jornadas.Add(jorn);
                    // Rotar equipos (excepto el primero)
                    int temp = equiposEntero[1];
                    for (int i = 1; i < numeroEquipos - 1; i++)
                    {
                        equiposEntero[i] = equiposEntero[i + 1];
                    }
                    equiposEntero[numeroEquipos - 1] = temp;

                }

               jornadas =JornadasNoConsecutivas(jornadas, equipos);
                if (competicion.Tipo== "Ida") {
                   // return jornadas;
                }
                //return jornadas;
                // Generar segunda vuelta (ida y vuelta)
                int totalPartidos = jornadas[0].Count;

                for (int j= 1; j<= totalJornadas; j++) {
                    List<JornadaDTO> jorn = new List<JornadaDTO>();
                    foreach (var item in jornadas[j])
                    {
                        
                        JornadaDTO ida = item;
                        JornadaDTO jornada1 = new JornadaDTO();
                        jornada1.Jornada1 = j + totalJornadas;
                        //ida.Jornada1 = ida.Jornada1 + totalJornadas;
                        EquipoDTO eL = ida.Local;
                        EquipoDTO eV = ida.Visitante;
                        //ida.Local = ida.Visitante;
                        jornada1.Local = eV;
                        jornada1.Visitante = eL;
                        jornada1.Ubicacion = "Ubicacion del partido";
                        jornada1.Fecha = DateTime.Now;
                        //ida.Visitante = eL;
                        //ida.Ubicacion = "Ubicacion del partido";
                        //ida.Fecha = DateTime.Now;
                        jornada1.Fase = fase;
                        jornada1.Competicion = competicion;
                        jorn.Add(jornada1);

                        //await this.Crear(jornada1);
                    }
                    jornadas.Add(jorn);
                }

              
               /* for (int i = 0; i < totalPartidos; i++)
                {

                    
                    JornadaDTO ida = jornadas[0][i];
                    JornadaDTO jornada1 = new JornadaDTO();
                    jornada1.Jornada1 = ida.Jornada1 + totalJornadas;
                    //ida.Jornada1 = ida.Jornada1 + totalJornadas;
                    EquipoDTO eL = ida.Local;
                    EquipoDTO eV = ida.Visitante;
                    //ida.Local = ida.Visitante;
                    jornada1.Local = eV ;
                    jornada1.Visitante = eL;
                    jornada1.Ubicacion = "Ubicacion del partido";
                    jornada1.Fecha = DateTime.Now;
                    //ida.Visitante = eL;
                    //ida.Ubicacion = "Ubicacion del partido";
                    //ida.Fecha = DateTime.Now;
                    jornadas[0].Add(jornada1);

                }*/
               // await this._Repository.Consultar();
               // return jornadas;

                /* foreach (var local in equipos)
                 {
                     foreach (var visitante in equipos)
                     {
                         JornadaDTO jornada = new JornadaDTO();
                         jornada.Local = local;
                         jornada.Visitante = visitante;

                         await this.Crear(liga);

                     }
                     LigaDTO liga = new LigaDTO();
                     liga.Inicializar();
                     liga.Equipo = equipo;
                     liga.Fase = fase.Id;

                     await this.Crear(liga);

                 }*/

            }
            catch (Exception ex) { }
            return null;
        }

        // Intentamos ajustar jornadas para evitar más de 2 partidos seguidos como L o V
        public List<List<JornadaDTO>> JornadasNoConsecutivas(List<List<JornadaDTO>> jornadas, List<EquipoCompeticionDTO> equipos)
        {
            List<List<JornadaDTO>> jornadas0 = new List<List<JornadaDTO>>();
            // Diccionario para registrar historial local/visitante por equipo
            Dictionary<string, List<string>> historial = equipos.ToDictionary(
           eq => eq.Equipo.Nombre,
           eq => new List<string>()
       );
            // Recorremos todas las jornadas
            for (int j = 0; j < jornadas.Count; j++)
            {
                foreach (var partido in jornadas[j])
                {
                    string local = partido.Local.Nombre;
                    string visitante = partido.Visitante.Nombre;

                    historial[local].Add("L");
                    historial[visitante].Add("V");
                }
            }

            // Intentamos ajustar jornadas para evitar más de 2 partidos seguidos como L o V
            for (int i = 0; i < jornadas.Count; i++)
            {
                for (int j = 0; j < jornadas[i].Count; j++)
                {
                    //var (local, visitante) = jornadas[i][j];
                    var local = jornadas[i][j].Local.Nombre;
                    var visitante = jornadas[i][j].Visitante.Nombre;

                    var histLocal = historial[local];
                    var histVisitante = historial[visitante];

                    // Revisamos solo a partir de la tercera jornada
                    if (i >= 1)
                    {
                        bool localConsecutivo = histLocal[i - 1] == "L" && histLocal[i] == "L";
                        bool visitanteConsecutivo = histVisitante[i - 1] == "V" && histVisitante[i] == "V";

                        if (localConsecutivo || visitanteConsecutivo)
                        {
                            // Intercambiamos local y visitante

                            JornadaDTO ida = jornadas[i][j];
                            JornadaDTO jornada1 = new JornadaDTO();
                            
                            //ida.Jornada1 = ida.Jornada1 + totalJornadas;
                            EquipoDTO eL = ida.Local;
                            EquipoDTO eV = ida.Visitante;
                            //ida.Local = ida.Visitante;
                            jornada1.Local = eV;
                            jornada1.Visitante = eL;
                            //jornadas0.Add(jornada1);
                            //jornadas[i][j] = (visitante, local);
                            historial[local][i] = "V";
                            historial[visitante][i] = "L";
                        }
                    }
                }
            }
                
            return jornadas0;
        }





        public List<List<JornadaDTO>> ExtraerCalendarioInicial(string ruta, List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase)
        {

            var jornadas = new List<List<JornadaDTO>>();
            int numEquipos = equipos.Count;
            if (numEquipos % 2 == 1)
            {
                numEquipos = numEquipos + 1;
            }

            int numJornadas = 0;
            if (competicion.IdTipo == 2)
            { //Ida
                numJornadas = numEquipos - 1;
            }
            if (competicion.IdTipo == 1) //Ida y Vuelta
            {
                numJornadas = (numEquipos - 1) * 2;
            }
            int jornadaActual = 0;
            var jornadasPDF = 0;




            using var doc = PdfDocument.Open(ruta);
            var partidoRegex = new Regex(
            @"(?<local>[A-Z0-9\s\""]+?)\s{1,2}(?<visitante>[A-Z0-9\s\""]+?)\s+\d{5}\s*-\s*\w+\s+(?<fecha>\d{2}/\d{2}/\d{4})\s+(?<hora>\d{2}:\d{2})\s+(?<campo>.+)",
            RegexOptions.IgnoreCase);


            foreach (var page in doc.GetPages())
            {

              //  Regex regex = new Regex(@"([A-ZÑÁÉÍÓÚ0-9\s\""]{5,})\s+(Murcia|Cartagena|San Javier|Santomera|Los Dolores)", RegexOptions.Multiline);
                string text = page.Text.Replace("\n", "").Replace("\r", "");
                
                string cadena0 = "JORNADA";
                int inicio = 0;
                int fin = text.IndexOf(cadena0);
              

                



                string texto = page.Text.Replace("\n", "").Replace("\r", "");

                var jornadaMatches = Regex.Matches(texto, @"JORNADA (\d+).*?(?=JORNADA \d+|$)", RegexOptions.Singleline);

               


                foreach (Match jornadaMatch in jornadaMatches)
                {

                    List<JornadaDTO> jornada = new List<JornadaDTO>();
                    jornadaActual = int.Parse(jornadaMatch.Groups[1].Value);
                    string bloque = jornadaMatch.Groups[0].Value;
                    // string jornadaTexto = jornadaMatch.Value;
                    string cadena = "Campo juego";
                    inicio = bloque.IndexOf(cadena); 
                    fin = bloque.Length-1;

                    bloque = fin > -1 ? bloque[inicio..fin] : bloque[inicio..];
                    bloque = bloque.Replace(cadena, "");

                    MatchCollection fechas = Regex.Matches(bloque, @"(\d{2}/\d{2}/\d{4})(\d{2}:\d{2})");

                    List<string> fechasUnicas = new List<string>();
                    foreach (Match match in fechas)
                    {

                        string fecha = match.Groups[1].Value;
                        string hora = match.Groups[2].Value;
                        fechasUnicas.Add(fecha +" "+hora);
                    }
                    

                    string pattern = @"(?=\d{2}/\d{2}/\d{4})";

                    string[] partidos = Regex.Split(bloque, pattern);

                    Console.WriteLine("Partidos detectados:");
                    int contador = 0;
                    foreach (string partido in partidos)
                    {

                        if (jornada.Count == 3) { 
                            break;
                        }
                        string pattern1 = @"\d{5}-\w+|\d{2}/\d{2}/\d{4}";
                        string limpio = Regex.Replace(partido, pattern1, "|");
                        limpio = limpio.Trim();
                        if (!string.IsNullOrEmpty(limpio))
                        {
                            var enfrentamiento = "";
                              
                                  var valor = 1000;
                                  JornadaDTO jornada0 = new JornadaDTO();
                            foreach (EquipoCompeticionDTO local in equipos)
                            {
                                foreach (EquipoCompeticionDTO visitante in equipos)
                                {
                                    try
                                    {
                                        if (local.Equipo.Nombre == "C.B. SANTOMERA AMARILLO" && visitante.Equipo.Nombre == " REAL MURCIA BALONCESTO \"A\"")
                                        {
                                            var re = true;
                                        }
                                        if (local.Equipo.Nombre == visitante.Equipo.Nombre) continue;
                                        int startIndex = limpio.IndexOf(local.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);
                                        int endIndex = limpio.IndexOf(visitante.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);

                                        if (startIndex < 0 || endIndex < 0)
                                        {

                                            var equiposJuntos = local.Equipo.Nombre + visitante.Equipo.Nombre;
                                            if (limpio.Length < equiposJuntos.Length)
                                            {
                                                if (contador == 0)
                                                {
                                                    enfrentamiento = limpio.Substring(0, limpio.Length);
                                                }
                                                else
                                                {
                                                    enfrentamiento = limpio;
                                                }

                                            }
                                            else
                                            {
                                                if (contador == 0)
                                                {
                                                    enfrentamiento = limpio.Substring(0, equiposJuntos.Length);
                                                }
                                                else
                                                {
                                                    var t = limpio.Length - equiposJuntos.Length - 2;
                                                    enfrentamiento = limpio.Substring(t);
                                                }

                                            }


                                            var res = Calculate(equiposJuntos, enfrentamiento);

                                            if (res <= valor)
                                            {
                                                valor = res;

                                                jornada0.Jornada1 = jornadaActual;
                                                jornada0.Local = local.Equipo;
                                                jornada0.Visitante = visitante.Equipo;


                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            var jor = ExtraerSubcadenaEntreEquipos0(limpio, local, visitante, equipos);
                                        }





                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("NO SE PUDO");
                                    }



                                }   
                                         

                                  }

                                  if (valor <= 20) {
                                CultureInfo cultura = new CultureInfo("es-ES");
                                var listaFechas = fechasUnicas.ToList();
                                var neededDateTime = DateTime.ParseExact(listaFechas[contador], "dd/MM/yyyy HH:mm",cultura);
                                jornada0.Fecha = neededDateTime;
                                      jornada.Add(jornada0);

                                  }

                            contador++;




                              
                        }
                    }



                    
                    

                    // Buscar partidos dentro del bloque

                    jornadasPDF++;




                    if ((numJornadas / 2) + 1 == jornadaActual)
                    {
                        //Generamos la sugunda vuelta

                        for (int j = 0; j < numJornadas / 2; j++)
                        {
                            List<JornadaDTO> jorn = new List<JornadaDTO>();
                            foreach (var item in jornadas[j])
                            {

                                JornadaDTO ida = item;
                                JornadaDTO jornada1 = new JornadaDTO();
                                jornada1.Jornada1 = j + numEquipos;
                                //ida.Jornada1 = ida.Jornada1 + totalJornadas;
                                EquipoDTO eL = ida.Local;
                                EquipoDTO eV = ida.Visitante;
                                //ida.Local = ida.Visitante;
                                jornada1.Local = eV;
                                jornada1.Visitante = eL;
                                jornada1.Ubicacion = "Ubicacion del partido";
                                jornada1.Fecha = DateTime.Now;
                                jornada1.PuntosLocal = 0;
                                jornada1.PuntosVisitante = 0;
                                //ida.Visitante = eL;
                                //ida.Ubicacion = "Ubicacion del partido";
                                //ida.Fecha = DateTime.Now;
                                jornada1.Fase = fase;
                                jornada1.Competicion = competicion;
                                jorn.Add(jornada1);

                                //await this.Crear(jornada1);
                            }
                            jornadas.Add(jorn);
                        }
                        return jornadas;

                    }
                    if (numJornadas / 2 < jornadaActual)
                    {
                        return jornadas;
                    }
                    else
                    {

                        
                        //jornada = ExtraerEquiposDesdeCadena(bloque, equipos, jornadaActual);
                        

                    }
                    //string textoFecha = fecha;
                   /* jornada.ForEach(data => {
                        data.Jornada1 = jornadaActual;
                    });*/


                    // Crear patrón del partido


                    jornadas.Add(jornada);
                }
            }

            if (jornadasPDF != numJornadas)
            {
                throw new Exception("EL numero de jornadas no coincide con el formato de la competición.");
            }

            return jornadas;

        }




        public List<List<JornadaDTO>> ExtraerPartidosDesdePdf0(string ruta, List<EquipoCompeticionDTO> equipos, CompeticionDTO competicion, FaseCompeticionDTO fase)
        {

            var jornadas = new List<List<JornadaDTO>>();
            int numEquipos = equipos.Count;
            if (numEquipos % 2 == 1)
            {
                numEquipos = numEquipos + 1;
            }

            int numJornadas = 0;
            if (competicion.IdTipo == 2)
            { //Ida
                numJornadas = numEquipos - 1;
            }
            if (competicion.IdTipo == 1) //Ida y Vuelta
            {
                numJornadas = (numEquipos - 1) * 2;
            }
            int jornadaActual = 0;

            using var doc = PdfDocument.Open(ruta);
            foreach (var page in doc.GetPages())
            {
                string texto = page.Text.Replace("\n", "").Replace("\r", "");

                var jornadaMatches = Regex.Matches(texto, @"JORNADA:RE\.RE\.(\d+)", RegexOptions.IgnoreCase);

                if (jornadaMatches.Count != numJornadas) {
                    throw new Exception("EL numero de jornadas no coincide con el formato de la competición.");
                }


                foreach (Match jornadaMatch in jornadaMatches)
                {

                    List<JornadaDTO> jornada = new List<JornadaDTO>();
                    jornadaActual = int.Parse(jornadaMatch.Groups[1].Value);
                    
                    int inicio = jornadaMatch.Index;
                    int fin = texto.IndexOf("JORNADA:RE.RE.", inicio + 1);
                    string bloque = fin > -1 ? texto[inicio..fin] : texto[inicio..];
                    string fecha =bloque.Substring(bloque.Length - 17);
                    string  bloquete = bloque.Substring(0, bloque.Length - 17);

                    // Buscar partidos dentro del bloque



                    


                    if ((numJornadas / 2)+1 == jornadaActual)
                    {
                        //Generamos la sugunda vuelta

                        for (int j = 0; j < numJornadas / 2; j++)
                        {
                            List<JornadaDTO> jorn = new List<JornadaDTO>();
                            foreach (var item in jornadas[j])
                            {

                                JornadaDTO ida = item;
                                JornadaDTO jornada1 = new JornadaDTO();
                                jornada1.Jornada1 = j + numJornadas;
                                //ida.Jornada1 = ida.Jornada1 + totalJornadas;
                                EquipoDTO eL = ida.Local;
                                EquipoDTO eV = ida.Visitante;
                                //ida.Local = ida.Visitante;
                                jornada1.Local = eV;
                                jornada1.Visitante = eL;
                                jornada1.Ubicacion = "Ubicacion del partido";
                                jornada1.Fecha = DateTime.Now;
                                jornada1.PuntosLocal = 0;
                                jornada1.PuntosVisitante = 0;
                                //ida.Visitante = eL;
                                //ida.Ubicacion = "Ubicacion del partido";
                                //ida.Fecha = DateTime.Now;
                                jornada1.Fase = fase;
                                jornada1.Competicion = competicion;
                                jorn.Add(jornada1);

                                //await this.Crear(jornada1);
                            }
                            jornadas.Add(jorn);
                        }
                        return jornadas;

                    }
                    if (numJornadas / 2 < jornadaActual) {
                        return jornadas;
                    }
                    else
                    {
                        jornada = ExtraerEquiposDesdeCadena(bloquete, equipos, jornadaActual);

                    }
                    string textoFecha = fecha;
                    jornada.ForEach(data => {
                        data.Jornada1 = jornadaActual;
                    });
                    

                    // Crear patrón del partido


                    jornadas.Add(jornada);
                }
            }

            return jornadas;

        }



        static List<JornadaDTO> ExtraerEquiposDesdeCadenaCalendarioInicial(string texto, List<EquipoCompeticionDTO> equipos, int jornada)
        {

            List<JornadaDTO> lista = new List<JornadaDTO>();

            var regex = new Regex(@"(?<local>[^\n\r]+?)\s+🆚?\s*(?<visitante>[^\n\r]+?)\s*\d{5}-\w+\s*(?<fecha>\d{2}/\d{2}/\d{4})\s*(?<hora>\d{2}:\d{2})\s*(?<campo>.+?)\s*(?=\d{5}-|$)", RegexOptions.Singleline);
            var regexSimplificado = new Regex(@"(?<local>[A-Z0-9\s\""]+?)\s+(?<visitante>[A-Z0-9\s\""]+?)\s+(\d{5})-\w+\s+[\w]+\s+(?<fecha>\d{2}/\d{2}/\d{4})\s+(?<hora>\d{2}:\d{2})\s+(?<campo>.+)", RegexOptions.Multiline);
            // Regex para detectar: bloques de texto seguidos de dos números de 1-3 dígitos
           

            var matches = regex.Matches(texto);

            foreach (Match match in matches)
            {
                string bloqueTexto = match.Groups[1].Value.Trim();
                int puntosLocal = int.Parse(match.Groups[2].Value);
                int puntosVisitante = int.Parse(match.Groups[3].Value);

                string cadena = "JORNADA " + jornada.ToString();
                bloqueTexto = bloqueTexto.Replace(cadena, "");
                JornadaDTO jornada0 = new JornadaDTO();
                foreach (var Local in equipos)
                {
                    foreach (var Visitante in equipos)
                    {
                        int resultado;
                        if (Local.Equipo.Nombre == Visitante.Equipo.Nombre) continue;
                        if (bloqueTexto.Length < 10) continue;
                        if (int.TryParse(cadena, out resultado)) continue;

                        jornada0 = ExtraerSubcadenaEntreEquipos(bloqueTexto, Local, Visitante, equipos);
                        if (jornada0 == null)
                        {
                            continue;
                        }
                        else
                        {

                            break;
                        }


                    }
                    if (jornada0 != null)
                    {
                        jornada0.PuntosLocal = puntosLocal;
                        jornada0.PuntosVisitante = puntosVisitante;
                        jornada0.Fecha = DateTime.Now;
                        jornada0.FechaCreacion = DateTime.Now;
                        jornada0.FechaModificacion = DateTime.Now;
                        lista.Add(jornada0);
                        break;
                    }
                    else
                    {

                        continue;
                    }
                }




            }
            return lista;

        }




        static List<JornadaDTO> ExtraerEquiposDesdeCadena(string texto, List<EquipoCompeticionDTO> equipos, int jornada)
        {

            List<JornadaDTO> lista = new List<JornadaDTO>();
            // Regex para detectar: bloques de texto seguidos de dos números de 1-3 dígitos
            var regex = new Regex(@"([A-ZÁÉÍÓÚÑ0-9\.\(\)\s]+?)(\d{2,3})(\d{2,3})");

            var matches = regex.Matches(texto);

            foreach (Match match in matches)
            {
                string bloqueTexto = match.Groups[1].Value.Trim();
                int puntosLocal = int.Parse(match.Groups[2].Value);
                int puntosVisitante = int.Parse(match.Groups[3].Value);
               
                string cadena = "RE.RE." + jornada.ToString();
                bloqueTexto = bloqueTexto.Replace(cadena, "");
                JornadaDTO jornada0 = new JornadaDTO();
                foreach (var Local in equipos)
                {
                    foreach (var Visitante in equipos)
                    {
                        int resultado;
                        if (Local.Equipo.Nombre ==Visitante.Equipo.Nombre) continue;
                        if (bloqueTexto.Length < 10) continue;
                        if (int.TryParse(cadena, out resultado)) continue;

                        jornada0 =  ExtraerSubcadenaEntreEquipos(bloqueTexto, Local, Visitante, equipos);
                        if (jornada0 == null)
                        {
                            continue;
                        }
                        else {
                           
                            break;
                        }

                        
                    }
                    if (jornada0 != null)
                    {
                        jornada0.PuntosLocal = puntosLocal;
                        jornada0.PuntosVisitante = puntosVisitante;
                        jornada0.Fecha = DateTime.Now;
                        jornada0.FechaCreacion = DateTime.Now;
                        jornada0.FechaModificacion = DateTime.Now;
                        lista.Add(jornada0);
                        break;
                    }
                    else {

                        continue;
                    }
                }


               
                
            }
            return lista;

        }

        public static JornadaDTO ExtraerSubcadenaEntreEquipos(string texto, EquipoCompeticionDTO equipoInicio, EquipoCompeticionDTO equipoFin, List<EquipoCompeticionDTO> equipos)
        {

           

            if (equipoInicio.Equipo.Nombre == "ACEITUNAS FRUYPER C.B. SAN JOSÉ" && equipoFin.Equipo.Nombre == "UCAM MURCIA ( JUNIOR)") {
                string hola = "";
            }


            JornadaDTO jornada = new JornadaDTO();



            int startIndex = texto.IndexOf(equipoInicio.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);
            int endIndex = texto.IndexOf(equipoFin.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                EquipoCompeticionDTO equipo = new EquipoCompeticionDTO();
                if (startIndex == -1 && endIndex == -1)//si no hemos encontrado ningun equipo
                {


                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    string local= texto.Substring(0, equipoInicio.Equipo.Nombre.Length);  
                    foreach (var item in equipos)
                    {

                        if (item.Equipo.Nombre.Length >= texto.Length) {
                            continue;
                        }
                       local = texto.Substring(0, item.Equipo.Nombre.Length);
                        int var = Calculate(item.Equipo.Nombre, local);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }
                    jornada.Local = equipo.Equipo;


                    //jornada.Local = 
                    //texto = texto.Replace(equipoFin, "");
                    mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    
                    foreach (var item in equipos)
                    {

                        if (item.Equipo.Nombre.Length >= texto.Length)
                        {
                            continue;
                        }
                        string visitante = texto.Substring(texto.Length - item.Equipo.Nombre.Length);
                        int var = Calculate(item.Equipo.Nombre, visitante);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }

                    jornada.Visitante = equipo.Equipo;
                    
                    return jornada;
                }
                if (startIndex > -1 && endIndex == -1)//si encuentro el equipo local
                {
                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    texto = texto.Replace(equipoInicio.Equipo.Nombre, "");
                    foreach (var item in equipos)
                    {
                        int var = Calculate(item.Equipo.Nombre, texto);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }
                        
                    }

                    jornada.Visitante = equipo.Equipo;
                    jornada.Local = equipoInicio.Equipo;
                    return jornada;

                }

                if (startIndex == -1 && endIndex > -1)//si encuentro el equipo visitante
                {
                    texto = texto.Replace(equipoFin.Equipo.Nombre, "");
                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    foreach (var item in equipos)
                    {
                        int var = Calculate(item.Equipo.Nombre, texto);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }
                      
                    }

                    jornada.Visitante = equipoFin.Equipo;
                    jornada.Local = equipo.Equipo;
                    return jornada;
                }

                if (startIndex > -1 && endIndex > -1)//si hemos encontrado los dos equipos
                {
                    jornada.Visitante = equipoFin.Equipo;
                    jornada.Local = equipoInicio.Equipo;
                    return jornada;
                }
                return null;
            }
            return null;

            
        }



        public static JornadaDTO ExtraerSubcadenaEntreEquipos0(string texto, EquipoCompeticionDTO equipoInicio, EquipoCompeticionDTO equipoFin, List<EquipoCompeticionDTO> equipos)
        {



            if (equipoInicio.Equipo.Nombre == "ACEITUNAS FRUYPER C.B. SAN JOSÉ" && equipoFin.Equipo.Nombre == "UCAM MURCIA ( JUNIOR)")
            {
                string hola = "";
            }


            JornadaDTO jornada = new JornadaDTO();



            int startIndex = texto.IndexOf(equipoInicio.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);
            int endIndex = texto.IndexOf(equipoFin.Equipo.Nombre, StringComparison.OrdinalIgnoreCase);

            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                EquipoCompeticionDTO equipo = new EquipoCompeticionDTO();
                if (startIndex == -1 && endIndex == -1)//si no hemos encontrado ningun equipo
                {


                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    string local = texto.Substring(0, equipoInicio.Equipo.Nombre.Length);
                    foreach (var item in equipos)
                    {

                        if (item.Equipo.Nombre.Length >= texto.Length)
                        {
                            continue;
                        }
                        local = texto.Substring(0, item.Equipo.Nombre.Length);
                        int var = Calculate(item.Equipo.Nombre, local);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }
                    jornada.Local = equipo.Equipo;


                    //jornada.Local = 
                    //texto = texto.Replace(equipoFin, "");
                    mayor = 100;
                    equipo = new EquipoCompeticionDTO();

                    foreach (var item in equipos)
                    {

                        if (item.Equipo.Nombre.Length >= texto.Length)
                        {
                            continue;
                        }
                        string visitante = texto.Substring(texto.Length - item.Equipo.Nombre.Length);
                        int var = Calculate(item.Equipo.Nombre, visitante);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }

                    jornada.Visitante = equipo.Equipo;

                    return jornada;
                }
                if (startIndex > -1 && endIndex == -1)//si encuentro el equipo local
                {
                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    texto = texto.Replace(equipoInicio.Equipo.Nombre, "");
                    foreach (var item in equipos)
                    {
                        int var = Calculate(item.Equipo.Nombre, texto);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }

                    jornada.Visitante = equipo.Equipo;
                    jornada.Local = equipoInicio.Equipo;
                    return jornada;

                }

                if (startIndex == -1 && endIndex > -1)//si encuentro el equipo visitante
                {
                    texto = texto.Replace(equipoFin.Equipo.Nombre, "");
                    int mayor = 100;
                    equipo = new EquipoCompeticionDTO();
                    foreach (var item in equipos)
                    {
                        int var = Calculate(item.Equipo.Nombre, texto);
                        if (var < mayor)
                        {
                            equipo = item;
                            mayor = var;
                        }

                    }

                    jornada.Visitante = equipoFin.Equipo;
                    jornada.Local = equipo.Equipo;
                    return jornada;
                }

                if (startIndex > -1 && endIndex > -1)//si hemos encontrado los dos equipos
                {
                    jornada.Visitante = equipoFin.Equipo;
                    jornada.Local = equipoInicio.Equipo;
                    return jornada;
                }
                return null;
            }
            return null;


        }



        public static int Calculate(string source1, string source2) //O(n*m)
        {
            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return matrix[source1Length, source2Length];
        }
        static int LevenshteinRecursive(string str1, string str2, int m, int n)
        {
            // If str1 is empty, the distance is the length of str2
            if (m == 0)
            {
                return n;
            }

            // If str2 is empty, the distance is the length of str1
            if (n == 0)
            {
                return m;
            }

            // If the last characters of the strings are the same
            if (str1[m - 1] == str2[n - 1])
            {
                return LevenshteinRecursive(str1, str2, m - 1, n - 1);
            }

            // Calculate the minimum of three operations:
            // Insert, Remove, and Replace
            return 1 + Math.Min(
                Math.Min(
                    // Insert
                    LevenshteinRecursive(str1, str2, m, n - 1),
                    // Remove
                    LevenshteinRecursive(str1, str2, m - 1, n)
                ),
                // Replace
                LevenshteinRecursive(str1, str2, m - 1, n - 1)
            );
        }



        public  List<List<JornadaDTO>> ExtraerPartidosDesdePdf(string ruta, List<EquipoCompeticionDTO>equipos, CompeticionDTO competicion, FaseCompeticionDTO fase)
        {
            var partidos = new List<List<JornadaDTO>>();
            int jornadaActual = 0;

            using var documento = PdfDocument.Open(ruta);

            foreach (var pagina in documento.GetPages())
            {
                string textoPlano = pagina.Text;

                // Buscar todas las jornadas
                var regexJornada = new Regex(@"JORNADA:RE\.RE\.(\d+)", RegexOptions.IgnoreCase);
                var matchesJornadas = regexJornada.Matches(textoPlano);

                foreach (Match jornadaMatch in matchesJornadas)
                {
                    jornadaActual = int.Parse(jornadaMatch.Groups[1].Value);

                    // Cortamos el bloque de texto desde esa jornada
                    int inicio = jornadaMatch.Index;
                    int fin = textoPlano.IndexOf("JORNADA:RE.RE.", inicio + 1);
                    string bloqueJornada = fin != -1 ? textoPlano.Substring(inicio, fin - inicio) : textoPlano.Substring(inicio);

                    // Buscar partidos en este bloque
                    var regexPartido = new Regex(@"([A-ZÁÉÍÓÚÑ0-9\s\.\-]+)([A-ZÁÉÍÓÚÑ0-9\s\.\-]+)(\d{1,3})\s+(\d{1,3})");

                    var partidosMatch = regexPartido.Matches(bloqueJornada);
                    foreach (Match m in partidosMatch)
                    {
                        var local = m.Groups[1].Value.Trim();
                        var visitante = m.Groups[2].Value.Trim();
                        equipos[0].Equipo.Nombre = local;
                        equipos[1].Equipo.Nombre = visitante;
                        int puntosLocal = int.Parse(m.Groups[3].Value);
                        int puntosVisitante = int.Parse(m.Groups[4].Value);

                        // Validación básica: evitar falsos positivos
                        if (local.Length > 3 && visitante.Length > 3)
                        {
                            partidos[0].Add(new JornadaDTO
                            {
                                Jornada1 = jornadaActual,
                                Ubicacion = "",
                                FechaCreacion = DateTime.Now,
                                Local = equipos[0].Equipo,
                                Visitante = equipos[1].Equipo,
                                PuntosLocal = puntosLocal,
                                PuntosVisitante = puntosVisitante,
                                Fase = fase,
                                Competicion = competicion,

                            });

                        }
                    }
                }
            }

            return partidos;
        }
        public async Task<bool> Editar(LigaDTO modelo)
        {
            return await _Repository.Editar( modelo.ToModel());
        }

        public async Task<bool> Editar(JornadaDTOSimple modelo)
        {
            var partido = await this._JornadaRepository.ObtenerUnModelo(j => j.Id == modelo.Id);

            partido.Fecha = modelo.Fecha;
            partido.FechaModificacion = DateTime.Now;
            partido.Ubicacion = modelo.Ubicacion;
            partido.PuntosLocal = modelo.PuntosLocal;
            partido.PuntosVisitante = modelo.PuntosVisitante;
            if ((partido.PuntosLocal != 0 || partido.PuntosVisitante != 0) && partido.Fecha < DateTime.Now) {

                var ligaLocal = await _Repository.ObtenerUnModelo(l => l.Equipo == partido.Local && l.Competicion == partido.Competicion);
                var ligaVisitante= await _Repository.ObtenerUnModelo(l => l.Equipo == partido.Visitante && l.Competicion == partido.Competicion);
                if (partido.PuntosLocal > partido.PuntosVisitante) {
                    if (ligaLocal.Racha <= 0)
                    {
                        ligaLocal.Racha = 1;
                    }
                    else {
                        ligaLocal.Racha++;
                    }
                    ligaLocal.PartidosGanados++;
                    ligaLocal.PartidosJugados++;
                    ligaLocal.PartidosJugadosLocal++;
                    ligaLocal.DifetenciaPuntos = ligaLocal.DifetenciaPuntos + (partido.PuntosLocal-partido.PuntosVisitante);
                    ligaLocal.PartidosGanadosLocal++;
                    ligaLocal.Puntos = ligaLocal.Puntos + 2;
                    ligaLocal.PuntosTotalesLocal = ligaLocal.PuntosTotalesLocal + partido.PuntosLocal;

                   
                    await _Repository.Editar(ligaLocal);
                    if (ligaVisitante.Racha >= 0)
                    {
                        ligaVisitante.Racha = -1;
                    }
                    else {
                        ligaVisitante.Racha--;
                    }
                   
                    
                    ligaVisitante.PartidosJugados++;
                    ligaVisitante.PartidosJugadosLocal++;
                    ligaVisitante.DifetenciaPuntos = ligaVisitante.DifetenciaPuntos + (partido.PuntosVisitante - partido.PuntosLocal);
                    ligaVisitante.PartidosPerdidos++;
                    ligaVisitante.PatidosPerdidosVisitante++;
                    ligaVisitante.Puntos = ligaVisitante.Puntos + 1;
                    ligaVisitante.PuntosTotalesVisitante= ligaVisitante.PuntosTotalesVisitante+ partido.PuntosVisitante;

                    await _Repository.Editar(ligaVisitante);
                }
                if (partido.PuntosLocal < partido.PuntosVisitante)
                {

                    if (ligaVisitante.Racha <= 0)
                    {
                        ligaVisitante.Racha = 1;
                    }
                    else {
                        ligaVisitante.Racha++;
                    }
                    ligaVisitante.PartidosGanados++;
                    ligaVisitante.PartidosJugados++;
                    ligaVisitante.PartidosJugadosVisitante++;
                    ligaVisitante.DifetenciaPuntos = ligaLocal.DifetenciaPuntos + (  partido.PuntosVisitante -partido.PuntosLocal);
                    ligaVisitante.PartidosGanadosLocal++;
                    ligaVisitante.Puntos = ligaVisitante.Puntos + 2;
                    ligaVisitante.PuntosTotalesVisitante = ligaVisitante.PuntosTotalesVisitante + partido.PuntosVisitante;

                    
                    await _Repository.Editar(ligaVisitante);
                    if (ligaLocal.Racha >= 0)
                    {
                        ligaLocal.Racha = -1;
                    }
                    else {
                        ligaLocal.Racha--;
                    }

                    
                    ligaLocal.PartidosJugados++;
                    ligaLocal.PartidosPerdidos++;
                    ligaLocal.PartidosJugadosLocal++;
                    ligaLocal.DifetenciaPuntos = ligaLocal.DifetenciaPuntos + (partido.PuntosLocal - partido.PuntosVisitante );
                    ligaLocal.PartidosPerdidosLocal++;
                    ligaLocal.Puntos = ligaLocal.Puntos + 1;
                    ligaLocal.PuntosTotalesLocal = ligaLocal.PuntosTotalesLocal + partido.PuntosLocal;

                    await _Repository.Editar(ligaLocal);
                }

                partido.Estado = "Finalizado";
            }
           return  await this._JornadaRepository.Editar(partido);


          
        }

        public Task<bool> EliminarJornada(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarLigaEntera(int idCompeticion)
        {
            try
            {
                var ligaEntera = await this._Repository.Consultar(l => l.Competicion == idCompeticion);
                foreach (var item in ligaEntera)
                {
                    if (!await this._Repository.Eliminar(item)) {
                        throw new Exception("La liga no se ha podido eliminar correctamente debido a un error en la clasificación");
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<bool> EliminarJornadaEntera(int idFase, int competicion)
        {
            try
            {
                var JornadasEntera = await this._JornadaRepository.Consultar(l => l.Fase == idFase);
                foreach (var item in JornadasEntera)
                {
                    if (!await this._JornadaRepository.Eliminar(item))
                    {
                        throw new Exception("La Jornada no se ha podido eliminar correctamente debido a un error.");
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public async Task<bool> EliminarJornadaEnteraPorCompeticion(int competicion)
        {
            try
            {
                var JornadasEntera = await this._JornadaRepository.Consultar(l => l.Competicion == competicion);
                foreach (var item in JornadasEntera)
                {
                    if (!await this._JornadaRepository.Eliminar(item))
                    {
                        throw new Exception("La Jornada no se ha podido eliminar correctamente debido a un error en la competición");
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }



        public async Task<bool> EliminarUnRegistroLiga(int id)
        {
            try
            {

                LigaDTO liga = await this.VerUnRegistoLiga(id);
                liga.ToModel();
                return await this._Repository.Eliminar(liga.ToModel());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }



        public async Task<List<LigaDTO>> Lista()
        {
            try
            {
                List<LigaDTO> ligue = new List<LigaDTO> ();
                var liga = await this._Repository.Consultar();
                if (liga == null)
                {
                    throw new Exception("No existen registros en la tabla");
                }

                foreach (var item in liga)
                {
                    LigaDTO ligue0 = LigaDTO.ToDTO(item);
                    ligue0.Equipo = await this._EquipoService.BuscarEquipo(item.Id);
                    ligue.Add(ligue0);
                }
               
                return ligue;


            }
            catch (Exception ex) { throw; }
        }

        public async Task<List<List<JornadaDTOSimple>>> ListaJornadas(CompeticionDTO competicion)
        {
            try
            {

                var numPartidos = 0;
                List<List<JornadaDTOSimple>> jornadas = new List<List<JornadaDTOSimple>>();
                var jornadas0 = await this._JornadaRepository.Consultar(c => c.Competicion == competicion.Id);
                if (jornadas0 == null || jornadas0.Count() == 0)
                {
                    return jornadas;
                }
                List<JornadaDTOSimple>jornadas1 = new List<JornadaDTOSimple>();//todos los partidos de la competicion
                foreach (var item in jornadas0) { 
                    JornadaDTO jor = JornadaDTO.ToDTO(item);
                    JornadaDTOSimple jor0 = new JornadaDTOSimple();
                    TemporadaDTO temporada = await this._TemporadaService.TemporadaActivaFull();
                    jor.Local = await this._EquipoService.BuscarEquipo(item.Local);
                    jor.Visitante = await this._EquipoService.BuscarEquipo(item.Visitante);
                    jor0.Jornada1 = jor.Jornada1;
                    jor0.Local = new EquipoDTOSimple();
                    jor0.Visitante = new EquipoDTOSimple();
                    ClubDTO club = await this._ClubService.VerClub(jor.Local.IdClub);
                    jor0.Local.IdClub = jor.Local.IdClub;
                    jor0.Local.Nombre = jor.Local.Nombre;
                    jor0.Local.logo = club.Foto;
                    club = await this._ClubService.VerClub(jor.Visitante.IdClub);
                    jor0.Visitante.IdClub = jor.Visitante.IdClub;
                    jor0.Visitante.Nombre = jor.Visitante.Nombre;
                    jor0.Visitante.logo = club.Foto;

                    jor0.Ubicacion = jor.Ubicacion;
                    jor0.Estado = jor.Estado;
                    jor0.Fecha = jor.Fecha;
                    jor0.PuntosVisitante = jor.PuntosVisitante;
                    jor0.PuntosLocal = jor.PuntosLocal;
                    jor0.Id = jor.Id;
                    //jor.Temporada = temporada;
                    //jor.Competicion = competicion;
                    //jor.Fase = await this._FaseCompeticionService.Ver((int)item.Fase);
                    //jor.Fase.Competicion = competicion;
                   
                    jornadas1.Add(jor0);
                }
                var jornada0 = jornadas0.OrderBy(j => j.Jornada1);
                var numjornadas = jornada0.Max(j => j.Jornada1);

                
                List <EquipoCompeticionDTO> equipos  = await this._EquipoCompeticionRepository.Listar(competicion.Id);


                if (equipos.Count % 2 != 0)//si el numero de equipos es impar
                {
                   
                    numPartidos = equipos.Count+1/2;
                }
                else {
                    numPartidos = equipos.Count/2;
                }
                

                for (int i = 0; i < numjornadas; i++)
                {
                    jornadas.Add(new List<JornadaDTOSimple>());
                }
                for (int i = 0; i < jornadas1.Count(); i++)
                {
                    
                    
                    jornadas[jornadas1[i].Jornada1 - 1].Add(jornadas1[i]);
                    
                }

                return jornadas;


            }
            catch (Exception ex) { throw; }
        }

    





        public async Task<JornadaDTO> VerJornada(int idFase)
        {
            try
            {
                var jornada = await this._JornadaRepository.ObtenerUnModelo(l => l.Id == idFase);
                if (jornada.Id == 0 || jornada == null)
                {
                    throw new Exception("No existe registros con los datos solicitados.");
                }
                JornadaDTO partido = JornadaDTO.ToDTO(jornada);
                
                partido.Local= await this._EquipoService.BuscarEquipo(jornada.Local);
                partido.Local = await this._EquipoService.BuscarEquipo(jornada.Visitante);

                return partido;


            }
            catch (Exception ex) { throw; }
        }

        public async Task<LigaDTO> VerUnRegistoLiga(int id)
        {
            try
            {
                var liga = await this._Repository.ObtenerUnModelo(l => l.Id == id);
                if (liga.Id == 0 || liga == null)
                {
                    throw new Exception("La liga solicitada con existe.");
                }
                LigaDTO ligue = LigaDTO.ToDTO(liga);
                ligue.Equipo = await this._EquipoService.BuscarEquipo(liga.Id);

                return ligue;


            }
            catch (Exception ex) { throw; }
        }

        public async Task<List<LigaDTO>> VerLiga(int id) //id competicion
        {
            try
            {
                List<LigaDTO> ligue = new List<LigaDTO>();
                var liga = await this._Repository.Consultar(l => l.Competicion == id);
               liga = liga.OrderByDescending(t=> t.Puntos ).OrderByDescending(t=> t.DifetenciaPuntos);
                if ( liga == null)
                {
                    throw new Exception("La liga solicitada con existe.");
                }
                foreach (var item in liga)
                {
                    LigaDTO ligue0 = LigaDTO.ToDTO(item);
                    ligue0.Equipo = await this._EquipoService.BuscarEquipo((int)item.Equipo);
                    ligue.Add(ligue0);
                }

                return ligue;
                

            }
            catch (Exception ex) { throw; }
        }
        public async Task<bool> InicializarLiga(List<EquipoDTO> equipos, FaseCompeticionDTO fase)
        {
            try
            {
                foreach (var equipo in equipos)
                {
                    LigaDTO liga = new LigaDTO();
                    liga.Competicion = fase.Competicion.Id;
                    liga.Inicializar();
                    liga.Equipo = equipo;
                    liga.Fase = fase.Id;

                    await this.Crear(liga);

                }

            }
            catch (Exception ex) { throw; }


            return true;
        }

        
    }
}
