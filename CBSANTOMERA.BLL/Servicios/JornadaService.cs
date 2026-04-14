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
    public class JornadaService : IJornadaService
    {
        private readonly IGenericRepository<Jornada> _repository;
        private readonly IMapper _mapper;

        public JornadaService(
            IGenericRepository<Jornada> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // 1️⃣ Crear
        public async Task<JornadaDTO> CrearAsync(JornadaDTO dto)
        {
            var jornada = dto.ToModel();
            jornada.FechaCreacion = DateTime.UtcNow;
            jornada.Estado ??= "Pendiente";

            await _repository.Crear(jornada);

            return JornadaDTO.ToDTO(jornada);
        }
        public async Task<bool> JornadaEstaAbiertaAsync(int jornadaId)
        {
            var jornada = await this._repository.Obtener(j => j.Id == jornadaId);
            return jornada != null && jornada.Estado != "Cerrada";
        }
        // 2️⃣ Actualizar
        public async Task<JornadaDTO?> ActualizarAsync(int id, JornadaDTO dto)
        {
            var jornada = await _repository.Obtener(j => j.Id == id);

            if (jornada == null)
                return null;

            jornada.Numero = dto.Numero;
            jornada.FechaInicio = dto.FechaInicio;
            jornada.FechaFin = dto.FechaFin;
            jornada.Estado = dto.Estado;
            jornada.FechaModificacion = DateTime.UtcNow;

            await _repository.Editar(jornada);

            return JornadaDTO.ToDTO(jornada);
        }

        // 3️⃣ Eliminar
        public async Task<bool> EliminarAsync(int id)
        {
            var jornada = await _repository.Obtener(j => j.Id == id);

            if (jornada == null)
                return false;

            return await _repository.Eliminar(jornada);
        }

        // 4️⃣ Obtener por Id
        public async Task<JornadaDTO?> ObtenerPorIdAsync(int id)
        {
            var jornada = await _repository.Obtener(j => j.Id == id);
            return jornada != null ? JornadaDTO.ToDTO(jornada) : null;
        }

        // 5️⃣ Obtener por competición
        public async Task<List<JornadaDTO>> ObtenerPorCompeticionAsync(int competicionId)
        {
            var query = await _repository.Consultar(j => j.Competicion == competicionId);

            return query
                .OrderBy(j => j.Numero)
                .Select(JornadaDTO.ToDTO)
                .ToList();
        }

        // 6️⃣ Obtener por fase
        public async Task<List<JornadaDTO>> ObtenerPorFaseAsync(int faseId)
        {
            var query = await _repository.Consultar(j => j.Fase == faseId);

            return query
                .OrderBy(j => j.Numero)
                .Select(JornadaDTO.ToDTO)
                .ToList();
        }

        




    }
}
