using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.MODEL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios
{
    public class PartidoService : IPartidoService
    {
        private readonly IGenericRepository<Partido> _repository;
        private readonly IMapper _mapper;
       

        public PartidoService(
            IGenericRepository<Partido> repository,
            IMapper mapper
           )
        {
            _repository = repository;
            _mapper = mapper;
           
        }




        public async Task<PartidoDTO> ActualizarAsync(int id, PartidoDTO dto)
        {
            var partido = await _repository.Obtener(p => p.Id == id);

            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado");

            ValidarPartidoEditable(partido);
            ValidarEquipos(dto);

            bool cerrarPartido =
                partido.Estado != "Finalizado" &&
                dto.Estado == "Finalizado";

            partido.Fecha = dto.Fecha;
            partido.Ubicacion = dto.Ubicacion;
            partido.Estado = dto.Estado;
            partido.PuntosLocal = dto.PuntosLocal;
            partido.PuntosVisitante = dto.PuntosVisitante;
            partido.Local = dto.Local.IdEquipo;
            partido.Visitante = dto.Visitante.IdEquipo;
            partido.Jornada = dto.Jornada.Id;
            partido.FechaModificacion = DateTime.UtcNow;

            await _repository.Editar(partido);

    

            return _mapper.Map<PartidoDTO>(partido);
        }




        public async Task<PartidoDTO> CrearAsync(PartidoDTO dto)
        {
            var partido = dto.ToModel();

            partido.FechaCreacion = DateTime.UtcNow;
            partido.Estado ??= "Programado";
            

            partido = await _repository.Crear(partido);

            return _mapper.Map<PartidoDTO>(partido);
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var partido = await _repository.Obtener(p => p.Id == id);
            return partido != null && await _repository.Eliminar(partido);
        }



        public async Task<PartidoDTO?> ObtenerPorIdAsync(int id)
        {
            var partido = await _repository.Obtener(p => p.Id == id);

            return partido != null
                ? _mapper.Map<PartidoDTO>(partido)
                : null;
        }


        public async Task<List<PartidoDTO>> ObtenerPorJornadaAsync(int jornadaId)
        {
            var query = await _repository.Consultar(p => p.Jornada == jornadaId);

            var partidos = query.ToList();

            return _mapper.Map<List<PartidoDTO>>(partidos);
        }

        private void ValidarPartidoEditable(Partido partido)
        {
            if (partido.Estado == "Finalizado")
                throw new InvalidOperationException("No se puede modificar un partido finalizado");
        }
        private void ValidarEquipos(PartidoDTO dto)
        {
            if (dto.Local.IdEquipo == dto.Visitante.IdEquipo)
                throw new InvalidOperationException("El equipo local y visitante no pueden ser el mismo");
        }



      


    }
}
