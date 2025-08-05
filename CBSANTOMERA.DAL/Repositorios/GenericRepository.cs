//using CBSANTOMERA2.DAL.DataContext;
//using CBSANTOMERA.DAL.Repositorios.Contrato;
using CBSANTOMERA.DAL.Repositorios.Contrato;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CBSANTOMERA.DAL;


namespace CBSANTOMERA.DAL.Repositorios
{
    public class GenericRepository<TModelo>:IGenericRepository<TModelo> where TModelo : class
    {
        private readonly CbsantomeraContext _dbcontext;
        

        public GenericRepository(CbsantomeraContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

       

        public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
        {
            try
            {
                _dbcontext.ChangeTracker.Clear(); //GUARDAMOS LOS CAMBIOS
                TModelo modelo = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                await _dbcontext.SaveChangesAsync();
                return modelo;
            }
            catch
            {

                throw;
            }
        }

        public async Task<TModelo> ObtenerUnModelo(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                _dbcontext.ChangeTracker.Clear(); //GUARDAMOS LOS CAMBIOS
                TModelo modelo1 = await _dbcontext.Set<TModelo>().FirstOrDefaultAsync(filtro);
                await _dbcontext.SaveChangesAsync();
                return modelo1;
            }
            catch
            {

                throw;
            }
        }
        public async Task<TModelo> Crear(TModelo modelo)
        {
            try
            {
                _dbcontext.ChangeTracker.Clear();
                var local = _dbcontext.Set<TModelo>().Add(modelo);
                /*if (local != null) {
                    _dbcontext.Entry(local).State = EntityState.Detached;
                }*/

                await _dbcontext.SaveChangesAsync();

                
                //GUARDAMOS LOS CAMBIOS
                return modelo;
            }
            catch
            (Exception ex)
            { throw; }
        }

        public async Task<bool> Editar(TModelo modelo)
        {
            try
            {
                _dbcontext.ChangeTracker.Clear(); //GUARDAMOS LOS CAMBIOS
                _dbcontext.Set<TModelo>().Update(modelo);
                await _dbcontext.SaveChangesAsync();
               
                return true;
            }

            catch

            { throw; }
        }

        public async Task<bool> Eliminar(TModelo modelo)
        {
            try
            {
                _dbcontext.ChangeTracker.Clear(); //GUARDAMOS LOS CAMBIOS
                _dbcontext.Set<TModelo>().Remove(modelo);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            (Exception ex)
            { throw; }
        }

        public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>> filtro = null)
        {
            try
            {
                IQueryable<TModelo> queryModelo = filtro == null ? _dbcontext.Set<TModelo>() : _dbcontext.Set<TModelo>().Where(filtro);
                _dbcontext.ChangeTracker.Clear(); //GUARDAMOS LOS CAMBIOS
                await _dbcontext.SaveChangesAsync();
                _dbcontext.ChangeTracker.Clear();
                return queryModelo;
            }
            catch
            (Exception ex)
            { throw; }
        }

       
    }
}
