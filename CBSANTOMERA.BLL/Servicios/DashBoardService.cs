using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CBSANTOMERA.BLL.Servicios.Contrato;
using CBSANTOMERA.DTO;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.MODEL;

namespace CBSANTOMERA.BLL.Servicios
{
    public class DashBoardService /*: IDashBoardService*/
    {
        /*private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;
        private object _Ventaquery;

        public DashBoardService(IGenericRepository<Producto> productoRepository, IVentaRepository ventaRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _ventaRepository = ventaRepository;
            _mapper = mapper;
        }


        private IQueryable<Venta> retornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias) {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);
            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value);
        
        }

        private async Task<int> TotalVentasUltimaSemana() {

            int total = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if (_ventaQuery.Count() > 0) {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                total = tablaVenta.Count();
            }

            return total;
        }

        private async Task<string> TotalIngresosUltimaSemana() {

            decimal resultado = 0;
            IQueryable<Venta> _VentaQuery = await _ventaRepository.Consultar();
            if (_VentaQuery.Count() > 0) {

                var tablaventa = retornarVentas(_VentaQuery, -7);
                resultado = tablaventa.Select(v => v.Total).Sum(v => v.Value);
            }

            return Convert.ToString(resultado, new CultureInfo("es-ES"));
        
        }

        private async Task<int> TotalProductos() {

            IQueryable<Producto> _productoQuery = await _productoRepository.Consultar();
            int total = _productoQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {

            Dictionary<string, int> resultado = new Dictionary<string, int>();

            IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();

            if (_ventaQuery.Count() > 0) {
                var tablaVenta = retornarVentas(_ventaQuery, -7);

                resultado = tablaVenta.GroupBy(v => v.FechaRegistro.Value.Date).OrderBy(g => g.Key).Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() }).ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }

            return resultado;
        }

        public async Task<DashBoardDTO> Resumen()
        {
           DashBoardDTO vmDashBoard = new DashBoardDTO();

            try
            {
               vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
               vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
               vmDashBoard.TotalProductos= await TotalProductos();
               List<VentasSemanaDTO>listaVenta = new List<VentasSemanaDTO>();

                foreach (KeyValuePair<string, int>item in await VentasUltimaSemana()) {
                    listaVenta.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

            }
            catch
            {
                throw;

            }

            return vmDashBoard;

        }*/
    }
}
