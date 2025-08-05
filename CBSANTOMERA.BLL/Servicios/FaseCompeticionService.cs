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
    public class FaseCompeticionService : IFaseCompeticionService
    {
        private readonly IGenericRepository<FasesCompeticion> _Repository;
        
        private string rutaServidor = @"wwwroot\archivos\Temporadas\";
        private string carpetaLocal = @"Competiciones\";
        private readonly ITemporadaService _TemporadaRepository;
        private readonly ICategoriaJugadorService _CategoriaRepository;
        private readonly IEquipoService _EquipoRepository;
        private readonly IMapper _mapper;
        private readonly IArchivosService _ArchivoService;
       // private readonly ILigaService _LigaRepository;
        public FaseCompeticionService(IGenericRepository<FasesCompeticion> repository)
        {
            _Repository = repository;
            
            /*this._LigaRepository = _LigaRepository;*/
            
           
        }

        public async Task<FaseCompeticionDTO> Crear(FaseCompeticionDTO modelo, CompeticionDTO competicion)
        {
            try
            {
                FasesCompeticion fase = new FasesCompeticion();
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaModificacion = DateTime.Now;
                //modelo.NumEquipos = 0;
                fase = modelo.ToModel();
                fase.Competicion = competicion.Id;

                var faseCreado = await _Repository.Crear(fase);
                FaseCompeticionDTO faseDTO = FaseCompeticionDTO.ToDTO(faseCreado);
                return faseDTO;

            }
            catch (Exception ex) { throw; }

        }

        public async Task<bool> Editar(FaseCompeticionDTO modelo)
        {
            try
            {
                FasesCompeticion fase = new FasesCompeticion();

                modelo.FechaModificacion = DateTime.Now;
                //modelo.NumEquipos = 0;
                fase = modelo.ToModel();

                var faseCreado = await _Repository.Editar(fase);
                //FaseCompeticionDTO faseDTO = FaseCompeticionDTO.ToDTO(faseCreado);
                return faseCreado;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<FaseCompeticionDTO> Ver(int idFase)
        {
            try
            {
                FasesCompeticion fase = new FasesCompeticion();
                fase = await this._Repository.ObtenerUnModelo(f => f.Id == idFase);

                if (fase == null) {
                    throw new Exception("La fase no existe");
                }
                FaseCompeticionDTO fase0 = FaseCompeticionDTO.ToDTO(fase);
                
               

                
                return fase0;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<bool> EliminarCompeticion(int id)
        {
            try
            {
                var fase = await _Repository.ObtenerUnModelo(f => f.Id == id);
                if (fase == null)
                {
                    throw new Exception("La fase de competición no existe");
                }
                if (!await _Repository.Eliminar(fase))
                {
                    throw new Exception("No se ha podido eliminar la fase");
                }
                
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var fase0 = await _Repository.ObtenerUnModelo(f => f.Id == id);
               
                if (fase0 == null)
                {
                    throw new Exception("La fase de competición no existe");
                }
                if (!await _Repository.Eliminar(fase0))
                {
                    throw new Exception("No se ha podido eliminar la fase");
                }

                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<FaseCompeticionDTO>> Listar()
        {
            var fases = await _Repository.Consultar();
            List<FaseCompeticionDTO> lista = new List<FaseCompeticionDTO>();
            foreach (var item in fases)
            {

                lista.Add(FaseCompeticionDTO.ToDTO(item));
            }
            return lista;
        }

        public async Task<List<FaseCompeticionDTO>> Listar(int idCompeticion)
        {
            var fases = await _Repository.Consultar(f => f.Competicion == idCompeticion);
            List<FaseCompeticionDTO> lista = new List<FaseCompeticionDTO>();
            foreach (var item in fases)
            {

                lista.Add(FaseCompeticionDTO.ToDTO(item));

            }
            return lista;

        }
    }
}
