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
        private readonly IGenericRepository<FasesCompeticion> _repository;

        public FaseCompeticionService(
            IGenericRepository<FasesCompeticion> repository)
        {
            _repository = repository;
        }

        public async Task<FaseCompeticionDTO> Crear(
            FaseCompeticionDTO modelo,
            CompeticionDTO competicion)
        {
            modelo.FechaCreacion = DateTime.UtcNow;
            modelo.FechaModificacion = DateTime.UtcNow;

            var fase = modelo.ToModel();
            fase.Competicion = competicion.Id;
            fase.Estado ??= "Pendiente";

            var creada = await _repository.Crear(fase);
            return FaseCompeticionDTO.ToDTO(creada);
        }

        public async Task<bool> Editar(FaseCompeticionDTO modelo)
        {
            modelo.FechaModificacion = DateTime.UtcNow;
            return await _repository.Editar(modelo.ToModel());
        }

        public async Task<FaseCompeticionDTO> Ver(int idFase)
        {
            var fase = await _repository.ObtenerUnModelo(f => f.Id == idFase)
                ?? throw new Exception("La fase no existe");

            return FaseCompeticionDTO.ToDTO(fase);
        }

        public async Task<bool> Eliminar(int id)
        {
            var fase = await _repository.ObtenerUnModelo(f => f.Id == id)
                ?? throw new Exception("La fase no existe");

            return await _repository.Eliminar(fase);
        }

        public async Task<List<FaseCompeticionDTO>> Listar(int idCompeticion)
        {
            var fases = await _repository.Consultar(f => f.Competicion == idCompeticion);
            return fases.Select(FaseCompeticionDTO.ToDTO).ToList();
        }


       

        public async Task CerrarFaseAsync(int faseId)
        {
            var fase = await _repository.ObtenerUnModelo(f => f.Id == faseId);
            if (fase == null || fase.Estado == "Cerrada") return;

            fase.Estado = "Cerrada";
            fase.FechaModificacion = DateTime.UtcNow;

            await _repository.Editar(fase);
        }

        public Task<List<FaseCompeticionDTO>> Listar()
        {
            throw new NotImplementedException();
        }
    }


}
