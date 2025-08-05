using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class CategoriaJugadorService : ICategoriaJugadorService
    {
        private readonly IGenericRepository<CategoriaJugador> _categoriaRepository;
       // private readonly IGenericRepository<Club> _clubRepository;
        private readonly IMapper _mapper;
        //private readonly int _Miclub = 38;
        //private readonly string rutaServidor = @"wwwroot\archivos\Clubes\";
        //private readonly string mirutaequipos = @"wwwroot\archivos\MisEquipos\";
        public CategoriaJugadorService(IGenericRepository<CategoriaJugador> categoriaRepository,  IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
           
            _mapper = mapper;
        }

        public async Task<CategoriaJugadorDTO> Crear(CategoriaJugadorDTO modelo)
        {
            try
            {
                var categoriaCreado = await _categoriaRepository.Crear(_mapper.Map<CategoriaJugador>(modelo));

                if (categoriaCreado.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }

                return _mapper.Map<CategoriaJugadorDTO>(categoriaCreado);

            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Editar(CategoriaJugadorDTO modelo)
        {
            try
            {
                var categoriaModelo = _mapper.Map<CategoriaJugador>(modelo);
                var equipoEncontrado = await _categoriaRepository.Obtener(u => u.Id == categoriaModelo.Id);

                if (equipoEncontrado == null) { throw new TaskCanceledException("El equipo no existe"); }

                equipoEncontrado.Nombre = categoriaModelo.Nombre;
                equipoEncontrado.Id = categoriaModelo.Id;
                equipoEncontrado.Nombre =categoriaModelo.Nombre;
                equipoEncontrado.Sexo = categoriaModelo.Sexo;
                equipoEncontrado.EsActivo = categoriaModelo.EsActivo;
                equipoEncontrado.PrimerAnio = categoriaModelo.PrimerAnio;
                equipoEncontrado.SegundoAnio = categoriaModelo.SegundoAnio;

                bool respuesta = await _categoriaRepository.Editar(equipoEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo editar");
                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var categoriaEncontrado = await _categoriaRepository.Obtener(p => p.Id == id);
                if (categoriaEncontrado == null) { throw new TaskCanceledException("El producto no existe"); }
                bool respuesta = await _categoriaRepository.Eliminar(categoriaEncontrado);
                if (!respuesta)
                {
                    throw new TaskCanceledException("No se pudo Eliminar");
                }

                return respuesta;

            }
            catch
            {
                throw;

            }
        }


        public async Task<CategoriaJugadorDTO> Obtener(int id)
        {
            try
            {
                var categoriaEncontrado = await _categoriaRepository.Obtener(p => p.Id == id);
               return _mapper.Map<CategoriaJugadorDTO>(await _categoriaRepository.Obtener(p => p.Id == id));
                
               
                

            }
            catch
            {
                throw;

            }
        }



        public async Task<List<CategoriaJugadorDTO>> Lista()
        {
            try
            {
                var listaCategorias = await _categoriaRepository.Consultar();

                //var listaEquipos = queryEquipos.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<CategoriaJugadorDTO>>(listaCategorias.ToList());

            }
            catch
            {
                throw;

            }
        }


    }
}
