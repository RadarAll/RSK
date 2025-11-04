using Microsoft.EntityFrameworkCore;
using RSK.Dominio.Entidades;
using RSK.Dominio.IRepositorios;
using System.Linq.Expressions;


namespace RSK.Infraestrutura.Repositorios
{
    public class RepositorioBaseAssincrono<TEntity, TContext> : IRepositorioBaseAssincrono<TEntity>
        where TEntity : EntidadeBase
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositorioBaseAssincrono(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Consulta => _dbSet.AsNoTracking();

        public virtual async Task<IEnumerable<TEntity>> ObterTodosAssincrono()
        {
            return await Consulta.ToListAsync();
        }

        public virtual async Task<TEntity?> ObterPorIdAssincrono(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> BuscarAssincrono(Expression<Func<TEntity, bool>> predicate)
        {
            return await Consulta.Where(predicate).ToListAsync();
        }

        public virtual async Task AdicionarAssincrono(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual Task AtualizarAssincrono(TEntity entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual async Task DeletarAssincrono(object id)
        {
            var entity = await ObterPorIdAssincrono(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public virtual async Task<int> SalvarAlteracoesAssincrono()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
