using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace CBSANTOMERA.BLL.Servicios
{
    public class TemporadaService : ITemporadaService
    {

        private readonly IGenericRepository<Temporada> _Repository;
        private readonly IMapper _mapper;
        private readonly string  rutaServidor = @"wwwroot\archivos\Temporadas\";
        //Estara fuera de la carpeta temporadas => NOTICIAS, PROMOCIONES/NOVEDADES,JUGADORES,CLUBES,

       
        //private readonly IEquipoService  _EquiposRepository;
        //private readonly IArchivosService _ArchivosRepository;
       
        public TemporadaService(IGenericRepository<Temporada> Repository, IMapper mapper )
        {
            _Repository = Repository;
            _mapper = mapper;
        
        }

        public async Task<TemporadaDTO> Crear(TemporadaDTO modelo)
        {
            try
            {
                

                modelo.FechaRegistro = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                modelo.Visible = false;
                int iniYear = modelo.Inicio.Year;
                int finYear = modelo.Fin.Year;
                string valor = "Temporada_"+iniYear+"-"+finYear;
                modelo.source = valor;
                modelo.Activo = false;
                var modeloCreado = await _Repository.Crear(_mapper.Map<Temporada>(modelo));
                
                string carpeta = valor +"\\";

                if (modeloCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la temporada en la BBDD.");


                }

                if (!Directory.Exists(rutaServidor))
                {
                    Directory.CreateDirectory(rutaServidor);
                }

                if (!Directory.Exists(rutaServidor + carpeta))
                {
                    Directory.CreateDirectory(rutaServidor+ carpeta);
                }
                string rutaImagen = Path.Combine(rutaServidor, carpeta);




                return TemporadaDTO.ToDto(modeloCreado);

            }
            catch
            {
                throw new TaskCanceledException("No se pudo crear la carpeta de la temporada.");

            }
        }

       

        public async Task<bool> Editar(TemporadaDTO modelo)
        {
            var encontrado = await _Repository.Obtener(u => u.Id == modelo.Id);
            try
            {


                modelo.FechaModificacion = DateTime.Now;
                if (encontrado == null) { 
                    throw new TaskCanceledException("No pudimos encontrar la temporada "+modelo.Nombre); 
                
                }
                var res = await _Repository.Editar(_mapper.Map<Temporada>(modelo));



                if (res == true)
                {

                    return true;
                }
            }

            //Procedemos a modifical las fotos, obtenemos todas la fotos del IDAlbum y la lista de las fotos a eliminar, comprobamos si la lista de fotos de la BBDD y la lista de fotos del cliente no se ha modificado.



            catch (Exception e) { 
                throw new TaskCanceledException("No se pudo editar la temporada en la BBDD"); 
            
            }

            return true;
        
    }

        public async Task<bool> Eliminar(int id)
        {
            return true;
        }


        public async Task<TemporadaDTO> Ver(int id)
        {
            try
            {

                var encontrado = await _Repository.Obtener(u => u.Id == id && u.Visible == true);
                TemporadaDTO modelo= _mapper.Map<TemporadaDTO>(encontrado);



                //club.ListaEquipos = await _EquiposRepository.ListaEquiposClub(id);
                return modelo;





            }
            catch (Exception e) { throw; }
        }

        public async Task<List<TemporadaDTO>> Lista()
        {
            try
            {
                var lista= await _Repository.Consultar();
                
                var lista1 = lista.OrderByDescending(x => x.FechaRegistro).ToList();
                List<TemporadaDTO>temporadas = new List<TemporadaDTO>();

                foreach (var item in lista1)
                {
                    temporadas.Add(TemporadaDTO.ToDto(item));
                }

                return temporadas;

            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }


        public async Task<List<TemporadaDTO>> ListaVisibles()
        {
            try
            {
                var lista = await _Repository.Consultar(u=> u.Visible == true);

                var lista1 = lista.OrderByDescending(x => x.FechaRegistro).ToList();
                List<TemporadaDTO> temporadas = new List<TemporadaDTO>();

                foreach (var item in lista1)
                {
                    temporadas.Add(TemporadaDTO.ToDto(item));
                }

                return temporadas;

            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }

        public async Task<TemporadaDTOSmall> TemporadaActiva()
        {
            try
            {
                var lista = await _Repository.ObtenerUnModelo(m => m.Activo == true && m.Visible == true);
               TemporadaDTO temporada = new TemporadaDTO();

              
               
                    temporada =TemporadaDTO.ToDto(lista);
               

                return temporada;



               

            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }

        public async Task<TemporadaDTOSmall> ObtenerTemporadaPorNombre(string nombre)
        {
            try
            {
                var lista = await _Repository.ObtenerUnModelo(m => m.Nombre == nombre && m.Activo == true);
                TemporadaDTO temporada = new TemporadaDTO();



                temporada = TemporadaDTO.ToDto(lista);


                return temporada;





            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }

        public async Task<TemporadaDTO> ObtenerTemporadaPorNombre1(string nombre)
        {
            try
            {
                var lista = await _Repository.ObtenerUnModelo(m => m.Nombre == nombre );
                TemporadaDTO temporada = new TemporadaDTO();



                temporada = TemporadaDTO.ToDto(lista);


                return temporada;





            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }

        public async Task<TemporadaDTO> TemporadaActivaFull()
        {
            try
            {
                var lista = await _Repository.ObtenerUnModelo(m => m.Activo == true && m.Visible == true );
                TemporadaDTO temporada = new TemporadaDTO();



                temporada = TemporadaDTO.ToDto(lista);


                return temporada;





            }
            catch
            {
                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");

            }
        }


        public async Task<bool> ActivarTemporada(int id)
        {
            try {
                var lista = await _Repository.Consultar(m => m.Activo == true && m.Visible == true);

                if (lista.Count() == 1 || lista.Count() == 0)
                {
                    if (lista.Count() == 0) {
                        var encontrado = await _Repository.Obtener(u => u.Id == id);

                        TemporadaDTO modelo = _mapper.Map<TemporadaDTO>(encontrado);
                        modelo.Activo = true;
                        var res =  await _Repository.Editar(_mapper.Map<Temporada>(modelo));
                        return res;
                    }

                    if (lista.Count() == 1)
                    {
                       var encontrado =  lista.FirstOrDefault();
                        encontrado.Activo = false;
                        TemporadaDTO modelo = _mapper.Map<TemporadaDTO>(encontrado);
                        await this.Editar(modelo);
                         modelo = await this.Ver(id);
                        modelo.Activo = true;
                        var res = await this.Editar(modelo);
                        return res;


                    }
                }
                else {
                    throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");
                   
                }

            } catch (Exception e) {

                throw new TaskCanceledException("No se pudo obtener la lista de la temporada.");
            }
            return false;

        }
    }
}
