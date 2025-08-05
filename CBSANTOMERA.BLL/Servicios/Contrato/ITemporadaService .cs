using CBSANTOMERA.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSANTOMERA.BLL.Servicios.Contrato
{
    public interface ITemporadaService
    {
        //Task<List<TemporadaDTO>> ListaCompleta();
        Task<List<TemporadaDTO>> Lista();
        Task<TemporadaDTO> ObtenerTemporadaPorNombre1(string nombre);
        Task<TemporadaDTOSmall> ObtenerTemporadaPorNombre(string nombre);
        Task<TemporadaDTO> Crear(TemporadaDTO modelo);
        Task<TemporadaDTO> Ver(int id);
        Task<TemporadaDTOSmall> TemporadaActiva();
        Task<bool> ActivarTemporada(int id);

        Task<bool> Editar(TemporadaDTO modelo);
        //void Dimension(AlbumDTO album);
        Task<bool> Eliminar(int id);
        Task<TemporadaDTO> TemporadaActivaFull();
        Task<List<TemporadaDTO>> ListaVisibles();



        }
}
