using EFCore.BulkExtensions;
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

        public IQueryable<TEntity> Consulta => _context.Set<TEntity>().AsNoTracking();

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

        public virtual void AtualizarAssincrono(TEntity entity)
        {
            var local = _context.Set<TEntity>().Local.FirstOrDefault(e => e.Id == entity.Id);
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task DeletarAssincrono(object id)
        {
            var entity = await ObterPorIdAssincrono(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public virtual async Task<int> SalvarAlteracoesAssincrono()
        {
            return await _context.SaveChangesAsync();
        }

        // ✅ Novo método: inserção em lote
        public virtual async Task BulkAdicionarAssincrono(IEnumerable<TEntity> entidades)
        {
            if (entidades == null || !entidades.Any())
                return;

            await _context.BulkInsertAsync(entidades.ToList());
        }

        // ✅ Atualização em lote
        public virtual async Task BulkAtualizarAssincrono(IEnumerable<TEntity> entidades)
        {
            if (entidades == null || !entidades.Any())
                return;

            await _context.BulkUpdateAsync(entidades.ToList());
        }
    }
}
