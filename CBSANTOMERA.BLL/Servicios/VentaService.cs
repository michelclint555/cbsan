namespace CBSANTOMERA.BLL.Servicios
{
    public class VentaService/* : IVentaService*/
    {
       /* private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;

        public VentaService(IGenericRepository<DetalleVenta> detalleVentaRepository, IVentaRepository ventaRepository, IMapper mapper)
        {
            _detalleVentaRepository = detalleVentaRepository;
            _ventaRepository = ventaRepository;
            _mapper = mapper;
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {

            IQueryable<Venta> query = await _ventaRepository.Consultar();
           
            var listaResultado = new List<Venta>();
            try
            {
                if (buscarPor == "fecha")
                {

                    DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));
                    DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));

                    listaResultado = await query.Where(v =>
                    v.FechaRegistro.Value.Date >= fech_Inicio.Date &&
                    v.FechaRegistro.Value.Date <= fech_Fin.Date).Include(dv => dv.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();


                }
                else {
                    listaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta ).Include(dv => dv.DetalleVenta).ThenInclude(p => p.IdProductoNavigation).ToListAsync();


                }



            }
            catch
            {
                throw;

            }

            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventaRepository.Registrar(_mapper.Map<Venta>(modelo));

                if (ventaGenerada.Id == 0) {
                    throw new TaskCanceledException("No se pudo Crear la venta");
                }
                return _mapper.Map<VentaDTO>(ventaGenerada);

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {

            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var ListaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fech_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));
                DateTime fech_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new System.Globalization.CultureInfo("es-ES"));
                ListaResultado = await query.Include(p => p.IdVentaNavigation).Include(v => v.IdVentaNavigation).Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fech_Inicio.Date 
                 && dv.IdVentaNavigation.FechaRegistro.Value.Date <= fech_Fin.Date).ToListAsync();

            }
            catch
            {
                throw;

            }

            return _mapper.Map<List<ReporteDTO>>(ListaResultado); 
        }*/
    }
}
