using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.BLL.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categorium> _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categorium> categoriaRepository, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try {
                var listaCategorias = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());
            
            }catch {
                throw;
            
            }
        }

        public async Task<CategoriaDTO> Obtener(int id)
        {
            try
            {
                var Categoria = await _categoriaRepositorio.Obtener(c => c.Id ==id);
                return _mapper.Map<CategoriaDTO>(Categoria);

            }
            catch
            {
                throw;

            }
        }
    }
}
