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

        public async Task<List<TEntity>> ObterTodosComoListaAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> ObterTodosAssincrono()
        {
            return await Consulta.ToListAsync();
        }

        public virtual async Task<TEntity?> ObterPorIdAssincrono(object id)
        {
            long longId = Convert.ToInt64(id); // converte com segurança

            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == longId);
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

        public virtual async Task BulkAdicionarAssincrono(IEnumerable<TEntity> entidades)
        {
            if (entidades == null || !entidades.Any())
                return;

            try
            {
                // Tentar bulk insert primeiro
                await _context.BulkInsertAsync(entidades.ToList());
            }
            catch (Exception ex) when (ex.Message.Contains("Loading local data is disabled") ||
                                      ex.Message.Contains("LOAD DATA LOCAL INFILE") ||
                                      ex.Message.Contains("local data loading") ||
                                      ex.InnerException?.Message.Contains("Loading local data is disabled") == true ||
                                      ex.InnerException?.Message.Contains("LOAD DATA LOCAL INFILE") == true ||
                                      ex.InnerException?.Message.Contains("local data loading") == true)
            {
                // Fallback para inserção normal se LOAD DATA LOCAL INFILE estiver desabilitado
                foreach (var entidade in entidades)
                {
                    await _context.Set<TEntity>().AddAsync(entidade);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log do erro não tratado
                throw;
            }
        }

        public virtual async Task BulkAtualizarAssincrono(IEnumerable<TEntity> entidades)
        {
            if (entidades == null || !entidades.Any())
                return;

            try
            {
                // Tentar bulk update primeiro
                await _context.BulkUpdateAsync(entidades.ToList());
            }
            catch (Exception ex) when (ex.Message.Contains("Loading local data is disabled") ||
                                      ex.Message.Contains("LOAD DATA LOCAL INFILE") ||
                                      ex.Message.Contains("local data loading") ||
                                      ex.InnerException?.Message.Contains("Loading local data is disabled") == true ||
                                      ex.InnerException?.Message.Contains("LOAD DATA LOCAL INFILE") == true ||
                                      ex.InnerException?.Message.Contains("local data loading") == true)
            {
                // Fallback para atualização normal se LOAD DATA LOCAL INFILE estiver desabilitado
                foreach (var entidade in entidades)
                {
                    _context.Set<TEntity>().Update(entidade);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log do erro não tratado
                throw;
            }
        }
    }
}
